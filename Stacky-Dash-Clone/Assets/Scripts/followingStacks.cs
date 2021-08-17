using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followingStacks : MonoBehaviour
{
    public GameObject dashPrefab;
    private bool lineExitted =true;
    Vector3 lineEnterPosition;
    public GameObject rightBorder3;
    GameObject sprit;
    
  

    private void Start()
    {
         sprit = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));


    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dash")
        {
            other.gameObject.tag = "notDash"; //defensive programming 
            playerController.instance.TakeDashes(other.gameObject);
            playerController.instance.IncreaseTotalCollected();

            other.gameObject.AddComponent<Rigidbody>(); // this dash will be thet next dash, thats why we are needed to add rigidbody, for catching the collisions
            other.gameObject.GetComponent<Rigidbody>().useGravity = false; // collisions will not physically affect our character
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true; // collisions will not physically affect our character
            other.gameObject.AddComponent<followingStacks>(); // adding this script to new dash which will be the new bottom dash

            Destroy(this); //destroying this script because this dash is not the most bottom dash anymore and thats why it does not need this script




        }

        else if (other.tag == "Border")

        {

            playerController.instance.setMovingInfo(true); //sending true means movement stopped and make isMoving false 
            playerController.instance.rearrangeTheCharacterPos();

        }

        else if (other.tag == "Line")
        {
            lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            playerController.instance.spendDashes(lineEnterPosition);





        }

        else if (other.tag == "lineEnd")
        {
            playerController.instance.setLineExitCheckBool();
            //lineExitted = true;
        }

        else if (other.tag == "Path")
        {

            playerController.instance.setPathEnteredInfo(true);
            lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            playerController.instance.spendDashes(lineEnterPosition);

            //lineExitted = true;
        }

        else if (other.tag == "endOfPath")
        {
            playerController.instance.setPathEnteredInfo(false);
            playerController.instance.setLineExitCheckBool();

            //lineEnterPosition = this.transform.position;

            //other.gameObject.GetComponent<Collider>().isTrigger = false;

            //playerController.instance.spendDashes(lineEnterPosition);
        }

        else if (other.tag == "levelEnd")
        {/*
            lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            playerController.instance.spendDashes(lineEnterPosition);
            playerController.instance.setEndPlatformReachedInfo();
            */
            playerController.instance.setEndPlatformReachedInfo();
            //playerController.instance.setMovingInfo(true);
            playerController.instance.setReachedMultiplierInfo(0f); //we already finished and at the x1 

        }

        else if (other.tag == "reachedMultiplier")
        {
            Debug.Log(other.name);
            float reached_multiplier_number = float.Parse(other.name.ToString());
            playerController.instance.setReachedMultiplierInfo(reached_multiplier_number);
        }

        else if (other.tag == "blockBackwardMovement")
        {
            playerController.instance.SetBackwardMovementAllowedInfo();
        
        }
        else if (other.tag == "finishLine")
        {
            cameraMovement.instance.reachedFinishLine();

        }





    }
    /*
    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag=="Line")
        {
            lineExitted = true;
        }
    }*/
}
