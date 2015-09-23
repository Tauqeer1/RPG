using UnityEngine;
using System.Collections;

public class MobNPC : MonoBehaviour {

    public float speed;                     //speed of the mob
    public float range;                     //player is in range
    public Transform player;                //Mob has the transform of the player
    CharacterController controller;         //Mob Character controller
    public double impactTime = 0.35;        //this variable impactTime is used to reduce the player's health
    //private Combat opponent;
    private Combat opponent;
    private bool impacted;
    public AnimationClip idleClip;
    public AnimationClip runClip;
    public AnimationClip dieClip;
    public AnimationClip attackClip;
    Animation anim;
    public int health;                       //health of the mob
    public int damage;                      //Damage of the player

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();
    }
	// Use this for initialization
	void Start () {
        
        //Accessing the Combat script through the player object
        opponent = player.GetComponent<Combat>();
	}
	
	// Update is called once per frame
	void Update () {
       
        if (!isDead())
        {
            if (!inRange())
            {
                Chase();
            }
            else
            {
                //GetComponent<Animation>().CrossFade(idle.name);
                Attack();
                if (anim[attackClip.name].time > 0.9 * anim[attackClip.name].length)
                {
                    impacted = false;
                }
            }
        }
        else
        {
            Dead();
        }

        
	}
    /// <summary>
    /// This is the method to check the player is in range or not
    /// if is in range it will return true otherwise false
    /// </summary>
    bool inRange()
    {
        //Calculate the distance between own position and player position 
        //Return true if it is in range if not return false
        return Vector3.Distance(transform.position, player.position) < range;
    }
    /// <summary>
    /// First look the player charecter 
    /// then chase towards the player character
    /// </summary>
    void Chase()
    {
        //Calculate the new rotation 
        Quaternion newRotation = Quaternion.LookRotation(player.position - transform.position);
        newRotation.x = 0;  //Don't rotate along x-axis
        newRotation.z = 0;  //Don't rotate along z-axis
        //Rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        controller.SimpleMove(transform.forward * speed);   //Move towards the player
        anim.CrossFade(runClip.name);  //Play the animation smoothly
    }
    /// <summary>
    /// This method is used to attack the player
    /// </summary>
    void Attack()
    {
        GetComponent<Animation>().Play(attackClip.name);
        if (anim[attackClip.name].time > anim[attackClip.name].length * impactTime && !impacted && anim[attackClip.name].time < 0.9 * anim[attackClip.name].length)  
        {
            impacted = true;
            opponent.GetHit(damage);
            //player.GetComponent<Combat>().GetHit(damage);
        }

    }
    /// <summary>
    /// This method checks that enemy is dead or not
    /// if dead return true else false
    /// </summary>
    /// <returns></returns>
    bool isDead()
    {
        return health <= 0;
    }
    /// <summary>
    /// This method will run when enemy's health is less than or equal to zero
    /// </summary>
    void Dead()
    {
        anim.CrossFade(dieClip.name);
        if (anim[dieClip.name].time > anim[dieClip.name].length * 0.9)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// When the player attack the mob it will 
    /// reduce its health depending how much damage the mob has
    /// </summary>
    /// <param name="damage"></param>
    public void GetHit(int damage)
    {
        //reduce the health w.r.t damage
        health -= damage;   //health=health-damage
        if (health < 0)
        {
            health = 0;
        }
        //Debug.Log(health);
    }
    /// <summary>
    /// Mouse Over function get the 
    /// information of an object when the mouse
    /// is pointing to the object in a game
    /// </summary>
    void OnMouseOver()
    {
        //Assign the game object to player combat script variable opponent
        //So that player get an idea of the enemy 
        player.GetComponent<Combat>().opponenet = gameObject;
    }
}
