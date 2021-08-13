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

        if (!lineExitted)
        {
            /*
            lineEnterPosition.z += 0.00000000000000000000000000000000000000000000000000000000000000000000000000000000001f;
            if (lineEnterPosition.z < this.transform.position.z+0.5f)
            {
                lineEnterPosition.z += 1.2f;
               //GameObject g = Instantiate(GameObject.FindWithTag("dashPrefab"), lineEnterPosition, Quaternion.identity) as GameObject;
            }
            */

            
            // GameObject g = Instantiate(GameObject.FindWithTag("dashPrefab"),lineEnterPosition, Quaternion.identity) as GameObject;
            // StartCoroutine(instantiateDashSteps());

        }

       
    }

    private void Start()
    {
         sprit = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));


    }

    void instantiateDashSteps() //destroying gameobject slowly
    {


        //yield return new WaitForSeconds(0.0000001f);
        //GameObject g= Instantiate(GameObject.FindWithTag("dashPrefab"), this.transform.position + new Vector3(0, 0, -1.3f), Quaternion.identity) as GameObject;
        //lineEnterPosition.z += 5f;
        lineEnterPosition.z += 1.5f;
        GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));
        GameObject g = Instantiate(sprite, lineEnterPosition + new Vector3(0, 5, 0), Quaternion.identity) as GameObject;

        //GameObject g = Instantiate(GameObject.FindWithTag("dashPrefab"), lineEnterPosition+ new Vector3(0,5,0), Quaternion.identity) as GameObject;
        //g.transform.SetParent(rightBorder3.transform);
        

        //yield return new WaitForSeconds(1f);
        
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aa");
        if (collision.collider.tag=="Line")
        {
            lineEnterPosition = this.transform.position;



            lineExitted = false;
            //instantiateDashSteps();
            float startIn = 0;
            float every = 0.285f;
            InvokeRepeating("instantiateDashSteps", startIn, every); //velocity 5.4 iyi
            playerController.instance.spendDashes();
        }
    }*/

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

        else if(other.tag =="Line")
        {
             lineEnterPosition = this.transform.position;

            other.gameObject.GetComponent<Collider>().isTrigger = false;

            lineExitted = false;
            //instantiateDashSteps();
            float startIn = 0;
            float every = 0.285f;
            //instantiateDashSteps();
            rightBorder3 = other.gameObject;
           // InvokeRepeating("instantiateDashSteps", startIn, every); //velocity 5.4 iyi
            playerController.instance.spendDashes(lineEnterPosition); 
                
            



        }

        else if(other.tag=="lineEnd")
            {
            //playerController.instance.setLineExitCheckBool();
            lineExitted = true;
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
