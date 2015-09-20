using UnityEngine;
using System.Collections;

public class CameraSmooth : MonoBehaviour {

    public Transform target; //Target object that we are following
    public float distance = 10.0f;  //distance in the x-z plane from target
    public float height = 5.0f; //height of the camera above the target
    public float heightDamping = 20.0f;
    float wantedHeight;
    float currentHeight;

    void LateUpdate()
    {
        //Return if we don't have the target
        if (!target)
        {
            return;
        }
        
        wantedHeight = target.position.y + height;
        
        //angle and height of the object that is attached with the script
        //float currentRotationAngle = transform.eulerAngles.y;
        currentHeight = transform.position.y;

        //Deep the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        //Set the position of the camera along x-z plane
        transform.position = target.position;
        //Distance meters behing the target
        transform.position -=  Vector3.forward * distance;

        //Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        //Always look at the target
        transform.LookAt(target);


    }
}
