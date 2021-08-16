using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //free unity package    
using System;

public class playerController : MonoBehaviour
{
    private bool pathEntered;
    public PathCreator pathCreator_; // for the non-straight line creation 
    public float speedAtLine = 5f;  //speed in the non-straight line
    float distanceTravelled;


    private float pathEnterPositionY;
    public Transform posA;
    public Transform posB;
    public Transform posC;
    public float interpolateAmount;

    public GameObject dashpref;
    public GameObject parentOfDashes; 
    public static playerController instance;
    public float speed;
    public Rigidbody rb;
    public GameObject previousDash;
    private bool isMoving = false;  
    
    int lollazo = 0;

    public GameObject first_stack;
    Vector3 lineEnterPosition;

    Vector3 last_step_putted_position;

    bool line_entered = false;

    public List<GameObject> listOfCollectedStacks;
   
    

    directionBeforeCollision direction;
    enum directionBeforeCollision
    {
        I, //initialization
        F, //forward
        B, //backward
        L, //left
        R  //right
    }
    private void Awake()
    {
        if (instance ==null)
        {
            instance = this;
            listOfCollectedStacks = new List<GameObject>();
            listOfCollectedStacks.Add(previousDash);


        }
    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a,b,t);
        Vector3 bc = Vector3.Lerp(b,c,t);

        return Vector3.Lerp(ab,bc,interpolateAmount);
        

    }


    // Update is called once per frame
    void Update() //freeze position & rotation may be required 
    {
        if (!pathEntered)
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || mobileInput.Instance.swipeLeft) && !isMoving && direction != directionBeforeCollision.L)
            {
                //rearrangeTheCharacterPos();
                rb.velocity = Vector3.left * speed * Time.deltaTime;
                isMoving = true;
                direction = directionBeforeCollision.L;
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow) || mobileInput.Instance.swipeRight) && !isMoving && direction != directionBeforeCollision.R)
            {
                //rearrangeTheCharacterPos();
                rb.velocity = Vector3.right * speed * Time.deltaTime;
                isMoving = true;
                direction = directionBeforeCollision.R;
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || mobileInput.Instance.swipeUp) && !isMoving && direction != directionBeforeCollision.F)
            {
                // rearrangeTheCharacterPos();
                direction = directionBeforeCollision.F;
                rb.velocity = Vector3.forward * speed * Time.deltaTime;
                isMoving = true;
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || mobileInput.Instance.swipeDown) && !isMoving && direction != directionBeforeCollision.B)
            {
                //rearrangeTheCharacterPos();
                direction = directionBeforeCollision.B;
                rb.velocity = -Vector3.forward * speed * Time.deltaTime;
                isMoving = true;
            }
        }
        /*
        else if (pathEntered)
        {
            distanceTravelled += speedAtLine * Time.deltaTime;
            transform.localPosition = pathCreator_.path.GetPointAtDistance(distanceTravelled);


            //transform.rotation = pathCreator_.path.GetRotationAtDistance(distanceTravelled);
            


        }*/
        
        
        //Debug.Log(rb.velocity);
        //this.transform.position =QuadraticLerp(posA.position,posB.position,posC.position, 20f);
        if (rb.velocity.magnitude >5.4f)
        {
            rb.velocity = rb.velocity.normalized * 5.4f;
        }

        if (pathEntered)
        {
            //sadece bu iki satır vardı //-----------***********
            //distanceTravelled += speedAtLine * Time.deltaTime;
            //transform.localPosition = new Vector3(0, 1.8f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);

            //distanceTravelled += speedAtLine * Time.deltaTime;
            if (!(pathEnterPositionY - 0.09f < -2.46f))
            {
                pathEnterPositionY -= 0.09f * speedAtLine * Time.deltaTime;
                distanceTravelled += speedAtLine * Time.deltaTime;
                transform.localPosition = new Vector3(0, pathEnterPositionY, 0) + new Vector3(0, 2.3f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);
            }

            else 
            {

                //pathEnterPositionY -= 0.09f * speedAtLine * Time.deltaTime;
                distanceTravelled += speedAtLine * Time.deltaTime;
                transform.localPosition = new Vector3(0, pathEnterPositionY, 0) + new Vector3(0, 2.6f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);

            }
                //Vector3 desiredPoint = new Vector3(0, pathEnterPositionY, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);
            //float yOffset = -0.09f*Time.deltaTime*speedAtLine;

            //transform.localPosition = desiredPoint + new Vector3(0, yOffset, 0);




            //transform.TransformPoint(0, 1, 0);
            //transform.localPosition = transform.localPosition + new Vector3(0,Math.Abs(transform.localPosition.y-0.2f),0);

            //pathCreator_.transform.localPosition = pathCreator_.transform.position + new Vector3(0, pathCreator_.transform.position.y, 0);
            //transform.rotation = pathCreator_.path.GetRotationAtDistance(distanceTravelled);

            //////
            ///
            if (lollazo % 2 != 0 && 1f < Vector3.Distance(transform.position, last_step_putted_position) && listOfCollectedStacks.Count > 1)
            {
                last_step_putted_position = transform.position;

                Destroy(listOfCollectedStacks[0]);
                listOfCollectedStacks.RemoveAt(0);


                /// lowering the character 

                Vector3 characterPos = transform.localPosition;
                characterPos.y -= 0.09f; //lowering y position of character
                if (!(characterPos.y < 0.2f)) // character should not go to below of the main ground
                {
                    transform.localPosition = characterPos;
                }
                Vector3 stacksPos = parentOfDashes.transform.localPosition;
                stacksPos.y += 0.06f; //raising y position of stacks because above lowering operation affects all the character&stacks components but actually only character should be affected 
                parentOfDashes.transform.localPosition = stacksPos;




                GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));

                GameObject g = Instantiate(sprite, new Vector3(transform.position.x, lineEnterPosition.y + 0.2f, transform.position.z), Quaternion.identity) as GameObject;
                Debug.Log("aa");

                if (listOfCollectedStacks.Count == 1)
                {
                    // Vector3 currentPosOfLastStack = listOfCollectedStacks[0].gameObject.transform.localPosition;
                    Vector3 currentPosOfLastStack = parentOfDashes.transform.localPosition;
                    currentPosOfLastStack.y -= 0.2f;
                    parentOfDashes.transform.localPosition = currentPosOfLastStack;
                    //listOfCollectedStacks[0].gameObject.transform.localPosition = currentPosOfLastStack;
                }


            }


            lollazo += 1;



        }

        else if (line_entered)
        {
            
            Debug.Log(Vector3.Distance(transform.position, last_step_putted_position));
            // && 1 > Vector3.Distance(transform.position, last_step_putted_position)
            if (lollazo % 2 != 0 && 1f < Vector3.Distance(transform.position, last_step_putted_position) && listOfCollectedStacks.Count > 1)
            {
                last_step_putted_position = transform.position;

                Destroy(listOfCollectedStacks[0]);
                listOfCollectedStacks.RemoveAt(0);


                /// lowering the character 

                Vector3 characterPos = transform.localPosition;
                characterPos.y -= 0.09f; //lowering y position of character
                if (!(characterPos.y < 0.2f)) // character should not go to below of the main ground
                {
                    transform.localPosition = characterPos;
                }
                Vector3 stacksPos = parentOfDashes.transform.localPosition;
                stacksPos.y += 0.06f; //raising y position of stacks because above lowering operation affects all the character&stacks components but actually only character should be affected 
                parentOfDashes.transform.localPosition = stacksPos;
               



                GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));

                GameObject g = Instantiate(sprite, new Vector3(transform.position.x, lineEnterPosition.y + 0.2f, transform.position.z), Quaternion.identity) as GameObject;
                Debug.Log("aa");

                if (listOfCollectedStacks.Count == 1)
                {
                    // Vector3 currentPosOfLastStack = listOfCollectedStacks[0].gameObject.transform.localPosition;
                    Vector3 currentPosOfLastStack = parentOfDashes.transform.localPosition;
                    currentPosOfLastStack.y -= 0.2f;
                    parentOfDashes.transform.localPosition = currentPosOfLastStack;
                    //listOfCollectedStacks[0].gameObject.transform.localPosition = currentPosOfLastStack;
                }

                
            }

          
            lollazo += 1;
        }

       


        
        
    }

    public void setPathEnteredInfo(bool b)
    {
        pathEntered = b;
        if (!b) //if path is exitted
        {/*
            Vector3 pos = previousDash.transform.localPosition;
            pos.y -= 0.080f;
            //Vector3.Lerp(gam.transform.localPosition, pos, Time.deltaTime * 1000);
            
            Vector3 characterPos = transform.localPosition;
            characterPos.y = lineEnterPosition.y;
            //Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 1000);
            transform.localPosition = characterPos;
            

            previousDash.GetComponent<BoxCollider>().isTrigger = false;
            */
            /*
            Vector3 stacksPos = parentOfDashes.transform.localPosition;
            //stacksPos.y += 0.003f; //raising y position of stacks because above lowering operation affects all the character&stacks components but actually only character should be affected 
            stacksPos.y += 0.22f;
            parentOfDashes.transform.localPosition = stacksPos;
            */
            Vector3 stacksPos = rb.transform.position;
            //stacksPos.y += 0.003f; //raising y position of stacks because above lowering operation affects all the character&stacks components but actually only character should be affected 
            //stacksPos.y += 0.29f;
            stacksPos.y = 0.9f; ///// degisebilir
            rb.transform.position = stacksPos;

            

            /*
            Vector3 characterPos = transform.localPosition;
            characterPos.y = lineEnterPosition.y *speedAtLine*Time.deltaTime;
            //Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 1000);
            transform.position = characterPos;
            */
        }

    }
    public void rearrangeTheCharacterPos()
    {
        if (direction == directionBeforeCollision.L)
        {




        Vector3 characterPos = transform.localPosition;
            characterPos.x += 0.3f *45 *Time.deltaTime;
            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);
            //transform.localPosition = characterPos;

        }
        else if (direction == directionBeforeCollision.R)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.x -= 0.3f * 45 * Time.deltaTime; ;

            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            //transform.localPosition = characterPos;

        }
        else if (direction == directionBeforeCollision.B)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.z += 0.3f * 45 * Time.deltaTime; ;
            transform.position =Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            // transform.localPosition = characterPos;


        }
        else if (direction == directionBeforeCollision.F)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.z -= 0.3f * 45 * Time.deltaTime; ;
            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            // transform.localPosition = characterPos;

        }



    }
    public void TakeDashes(GameObject gam)
    {
        
        gam.transform.SetParent(parentOfDashes.transform);
        listOfCollectedStacks.Add(gam); // put collected stack to list, for reaching it later
        
        Vector3 pos = previousDash.transform.localPosition;
        pos.y -= 0.080f;
        //Vector3.Lerp(gam.transform.localPosition, pos, Time.deltaTime * 1000);
        gam.transform.localPosition  = pos;
        Vector3 characterPos = transform.localPosition;
        characterPos.y += 0.080f;
        //Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 1000);
        transform.localPosition = characterPos;
        previousDash = gam;

        previousDash.GetComponent<BoxCollider>().isTrigger = false;

    
    }
    
    public void setMovingInfo(bool moveStopped)
    {
        if (moveStopped)
        {
            isMoving = false;
            rb.velocity = Vector3.zero;
        }

        else
        {
            isMoving = true;
        }
    
    }

    public bool getMovingInfo()
    {
        return isMoving;
    }

    public void setLineExitCheckBool()
    {
        line_entered = false;
    }

    public void spendDashes(Vector3 aaa)
    {
        lineEnterPosition = aaa;
        if (pathEntered)
        {
            pathEnterPositionY = 0;

        }
        //float startIn = 0;
        //float every = 0.125f;
        //InvokeRepeating("instantiateDashSteps", startIn, every); //velocity 5.4 iyi
       
        last_step_putted_position = aaa;

        
       instantiateDashSteps();
       

    }
    private bool isInt(float N)
    {

        // Convert float value
        // of N to integer
        int X = (int)N;

        float temp2 = N - X;

        // If N is not equivalent
        // to any integer
        if (temp2 > 0)
        {
            return false;
        }
        return true;
    }
    void instantiateDashSteps() //destroying gameobject slowly
    {




        /*
        if (lollazo %2!=0)
        {
            Destroy(listOfCollectedStacks[0]);
            listOfCollectedStacks.RemoveAt(0);
            lineEnterPosition.z += 1.4f;
            GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));
            GameObject g = Instantiate(sprite, lineEnterPosition+new Vector3(0,0.2f,0) , Quaternion.identity) as GameObject;
        }
        
       
        lollazo += 1;
        */
        line_entered = true;
        
        Debug.Log(lollazo);

    }
}
