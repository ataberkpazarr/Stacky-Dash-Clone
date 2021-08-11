using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followingStacks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dash")
        {
            other.gameObject.tag = "notDash"; //defensive programming 
            playerController.instance.TakeDashes(other.gameObject);
            other.gameObject.AddComponent<Rigidbody>(); // this dash will be thet next dash, thats why we are needed to add rigidbody, for catching the collisions
            other.gameObject.GetComponent<Rigidbody>().useGravity = false; // collisions will not physically affect our character
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true; // collisions will not physically affect our character
            other.gameObject.AddComponent<followingStacks>(); // adding this script to new dash which will be the new bottom dash
            Destroy(this); //destroying this script because this dash is not the most bottom dash anymore and thats why it does not need this script




        }

        else if (other.tag =="Border")

        {

            playerController.instance.setMovingInfo(true); //sending true means movement stopped and make isMoving false 
        }
    }
}
