using UnityEngine;
using System.Collections;

public class ClickToMove : MonoBehaviour {

    Vector3 position;
    public float speed = 10; //hold the speed of the player
    CharacterController playerController;
    RaycastHit hit;
    Ray ray;
    public AnimationClip runClip;
    public AnimationClip idleClip;
    public static bool attack;
    public static bool die;
    void Awake()
    {
        playerController = GetComponent<CharacterController>();
    }
	void Start () {
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        //if the player is not attacking then it can move around 
        //but if the player is attacking it can not do anything until it finish the attacking
        if (!attack && !die)
        {
            if (Input.GetMouseButton(0))
            {
                //locate where the player click on the terrain
                locatePosition();
            }
            //Move towards the target
            moveToPosition();
        
        }
	}
    /// <summary>
    /// Locate the position by throwing raycast 
    /// </summary>
    void locatePosition()
    {
        //Cast the ray from main camera to input position
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Check the raycast if it something on its way
        if (Physics.Raycast(ray, out hit, 1000))
        {
            //work when the player is not clicking himself and not clicking on enemy
            if (hit.collider.tag != "Player" && hit.collider.tag!="Enemy")
            {
                //Get the mouse position vector
                position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            }
        }
    }
    /// <summary>
    /// First look towards the position where the mouse click by doing ray cast
    /// then move towards that position
    /// </summary>
    void moveToPosition()
    {
        //Only move the player when the distance between transform and target position is greater than one
        if (Vector3.Distance(transform.position, position) > 1)
        {
            //Calculate the new target rotation
            Quaternion newRotation = Quaternion.LookRotation(position - transform.position);
            newRotation.x = 0f; //Don't rotate in x-axis 
            newRotation.z = 0f; //Don't rotate in z-axis 
            //Rotate the player smoothly by using slerp function towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            //Move the player towards the target
            playerController.SimpleMove(transform.forward * speed);
            //GetComponent<Animation>().Play(run.name);     //Play the run animation
            GetComponent<Animation>().CrossFade(runClip.name);  //Smooth the animation transition
        }
        else
        {
            //GetComponent<Animation>().Play(idle.name); //Play the idle animation
            GetComponent<Animation>().CrossFade(idleClip.name); //smooth animation transition
        }
    }
}
