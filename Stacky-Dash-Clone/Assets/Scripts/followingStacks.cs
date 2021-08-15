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
    
    private void Update()
    {

       
       
    }

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
            other.gameObject.AddComponent<Rigidbody>(); // this dash will be thet next dash, thats why we are needed to add rigidbody, for catching the collisions
            other.gameObject.GetComponent<Rigidbody>().useGravity = false; // collisions will not physically affect our character
            other.gameObject.GetComponent<Rigidbody>().isKinematic = true; // collisions will not physically affect our character
            other.gameObject.AddComponent<followingStacks>(); // adding this script to new dash which will be the new bottom dash
            Destroy(this); //destroying this script because this dash is not the most bottom dash anymore and thats why it does not need this script

            Debug.Log("aa");


        }

        else if (other.tag =="Border")

        {

            playerController.instance.setMovingInfo(true); //sending true means movement stopped and make isMoving false 
            playerController.instance.rearrangeTheCharacterPos();
            Debug.Log("aaaaaaa");
        }

        else if(other.tag =="Line")
        {
             lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            playerController.instance.spendDashes(lineEnterPosition); 
                
            



        }

        else if(other.tag=="lineEnd")
            {
            playerController.instance.setLineExitCheckBool();
            //lineExitted = true;
            }

        else if (other.tag == "Path")
        {
            
            playerController.instance.setPathEnteredInfo ();
            lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            playerController.instance.spendDashes(lineEnterPosition);

            //lineExitted = true;
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
