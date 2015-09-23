using UnityEngine;
using System.Collections;

public class Combat : MonoBehaviour {

    public GameObject opponenet;            //Enemy game object assigned from another script MobNPC
    public AnimationClip attackClip;        //Assigned attack atack animation
    public int damage;                      //Damage of the mob
    public double imapactTime;              //this is the time to reduce the enemy health
    public bool impacted;                   //keep track of the impactTime to damage
    public float range;                     //Set the range of enemy
    Animation anim;
    public int health;
    public AnimationClip dieClip;
    bool startedDieAnimation;
    bool endedDieAnimation;
    void Awake()
    {
        anim = GetComponent<Animation>();
    }
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //By pressing space key player will attack the enemy
        if (Input.GetKey(KeyCode.Space) && inRange())
        {
            //Play the attack animation
            anim.CrossFade(attackClip.name);
            //set the static attack variable to true when the player is attacking
            ClickToMove.attack = true;
            /* look towards the opponent when it is not null if it is null
              it simply play the attack animation */
            if (opponenet != null)
            {
                //when click to attack then player should face towards the target
                transform.LookAt(opponenet.transform.position);
            }
        }
        //if attack animation is not playing 
        //if (!GetComponent<Animation>().IsPlaying(attack.name))
        if (anim[attackClip.name].time > 0.9 * anim[attackClip.name].length) 
        {
            //set the attack variable back to false when the player is not attacking
            ClickToMove.attack = false;
            //return to false because we need to impact again next time
            impacted = false;
        }
        Impact();
        Dead();
	}
    /// <summary>
    /// This impact method deals with the actual damage at the correct time
    /// </summary>
    void Impact()
    {
        //Check to see if opponent is not null and attack animation is playing and not impacted
        if (opponenet != null && anim.IsPlaying(attackClip.name) && !impacted)
        {
            //Check animation time exceeded to impact time then we will hit the enemy (Not clear)
            if ((anim[attackClip.name].time) > (anim[attackClip.name].length * imapactTime) && anim[attackClip.name].time < 0.9 * anim[attackClip.name].length) 
            {
                //Call the getHit method and pass the damage parameter for damage the mob health
                opponenet.GetComponent<MobNPC>().GetHit(damage);
                //True means we impacted the enemy
                impacted = true;
            }  
        }
    }
    public void GetHit(int damage)
    {
        health = health - damage;
        if (health < 0)
        {
            health = 0;
        }
    }
    /// <summary>
    /// This method checks that enemy is in range
    /// if it is in range it will return true otherwise false
    /// </summary>
    /// <returns></returns>
    bool inRange()
    {
        return (Vector3.Distance(opponenet.transform.position, transform.position) <= range);
    }
    /// <summary>
    /// This method checks your are dead
    /// or not if dead returns true else false
    /// </summary>
    /// <returns></returns>
    public bool isDead()
    {
        return health <= 0;
    }
    /// <summary>
    /// This method runs when player are going to dead
    /// </summary>
    void Dead()
    {
        if (isDead() && !endedDieAnimation)
        {
            if (!startedDieAnimation)
            {
                ClickToMove.die = true;
                anim.Play(dieClip.name);
                startedDieAnimation = true;
            }
            if (startedDieAnimation && !anim.IsPlaying(dieClip.name))
            {
                Debug.Log("You have died");
                endedDieAnimation = true;
                startedDieAnimation = false;
                ClickToMove.die = false;
            }
            
        }
    }
}
