using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //free unity package    
using System;
using UnityEngine.UI; 

public class playerController : MonoBehaviour
{
    private float reachedMultiplier;

    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private Text levelEndText;
    [SerializeField] private Text scoreBoard;
    [SerializeField] private GameObject dashPosition;


    private bool pathEntered;
    public PathCreator pathCreator_; // for the non-straight line creation 
    public float speedAtLine = 5f;  //speed in the non-straight line
    float distanceTravelled;
    ////0.1699996
    bool endPlatformReached;
    private float pathEnterPositionY;
    public Transform posA;
    public Transform posB;
    public Transform posC;
    public float interpolateAmount;

    private Vector3 parentOfDashesPos;

    public GameObject dashpref;
    public GameObject parentOfDashes; 
    public static playerController instance;
    public float speed;
    public Rigidbody rb;
    public GameObject previousDash;
    private bool isMoving = false;
    private bool backwardMovementIsAllowed;
    int lollazo = 0;

    

    public GameObject first_stack;
    Vector3 lineEnterPosition;

    Vector3 last_step_putted_position;

    bool line_entered = false;

    public List<GameObject> listOfCollectedStacks;

    private float offsetBetweenGroundAndCharacter;
    

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
            
            backwardMovementIsAllowed = true;
            offsetBetweenGroundAndCharacter = this.transform.position.y - dashPosition.transform.position.y;

            parentOfDashesPos = parentOfDashes.transform.position;


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
        scoreBoard.text = listOfCollectedStacks.Count.ToString(); ;
        if (listOfCollectedStacks.Count==1  )
        {
            if (endPlatformReached)
            {

                setMovingInfo(true);
                endPlatformReached = false;
                levelEndText.text = "Congrats for having X" + reachedMultiplier.ToString();
                levelEndPanel.SetActive(true);
            }
            setOffset();
            /*
            float offsetDifference = listOfCollectedStacks[0].gameObject.transform.position.y - dashPosition.transform.position.y;
            if (offsetDifference >0)
            {
                Vector3 pos = transform.position;
                pos.y -= offsetDifference;
                transform.position = pos;
            }

            else if (offsetDifference<0)
            {
                Vector3 pos = transform.position;
                pos.y += Math.Abs(offsetDifference);
                transform.position = pos;
            }*/
            /*
            if (this.transform.position.y < 0.4f)
            {

                Vector3 pos = transform.position;
                pos.y = 0.4f;
                transform.position = pos;
            }*/

           // rb.velocity = rb.velocity.normalized * 0f;
           // rb.velocity = rb.velocity * 0*Time.deltaTime; //-0.08
          
        }

        if (!pathEntered)
        {
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || mobileInput.Instance.swipeLeft) && !isMoving && direction != directionBeforeCollision.L )
            {
                //rearrangeTheCharacterPos();
                rb.velocity = Vector3.left * speed * Time.deltaTime;
                isMoving = true;
                direction = directionBeforeCollision.L;

                
            }
            else if ((Input.GetKeyDown(KeyCode.RightArrow) || mobileInput.Instance.swipeRight) && !isMoving && direction != directionBeforeCollision.R )
            {
                //rearrangeTheCharacterPos();
                rb.velocity = Vector3.right * speed * Time.deltaTime;
                isMoving = true;
                direction = directionBeforeCollision.R;
                if (!backwardMovementIsAllowed)
                {
                    backwardMovementIsAllowed = true;
                }
            }
            else if ((Input.GetKeyDown(KeyCode.UpArrow) || mobileInput.Instance.swipeUp) && !isMoving && direction != directionBeforeCollision.F )
            {
                // rearrangeTheCharacterPos();
                direction = directionBeforeCollision.F;
                rb.velocity = Vector3.forward * speed * Time.deltaTime;
                isMoving = true;
                if (!backwardMovementIsAllowed)
                {
                    backwardMovementIsAllowed = true;
                }
            }
            else if ((Input.GetKeyDown(KeyCode.DownArrow) || mobileInput.Instance.swipeDown) && !isMoving && direction != directionBeforeCollision.B &&backwardMovementIsAllowed )
            {
                //rearrangeTheCharacterPos();
                direction = directionBeforeCollision.B;
                rb.velocity = -Vector3.forward * speed * Time.deltaTime;
                isMoving = true;
            }

           
        }

        if (rb.velocity.magnitude > 5.4f)
        {
            rb.velocity = rb.velocity.normalized * 5.4f;
        }

        if (pathEntered)
        {
            
            if (!(pathEnterPositionY - 0.09f < -2.46f))
            {
                //transform.localPosition = new Vector3(0, pathEnterPositionY, 0) + new Vector3(0, 2.3f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);
                
                pathEnterPositionY -= 0.09f * speedAtLine * Time.deltaTime;
                distanceTravelled += speedAtLine * Time.deltaTime;
                transform.localPosition = new Vector3(0, pathEnterPositionY, 0) + new Vector3(0, 2.7f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);
            }

            else 
            {

               
                distanceTravelled += speedAtLine * Time.deltaTime;
                transform.localPosition = new Vector3(0, pathEnterPositionY, 0) + new Vector3(0, 2.6f, 0) + pathCreator_.path.GetPointAtDistance(distanceTravelled);

            }
                

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
                

                if (listOfCollectedStacks.Count == 1)
                {
                    
                    Vector3 currentPosOfLastStack = parentOfDashes.transform.localPosition;
                    currentPosOfLastStack.y -= 0.2f;
                    parentOfDashes.transform.localPosition = currentPosOfLastStack;

                    


                }


            }


            lollazo += 1;



        }

        else if (line_entered)
        {
            
            //Debug.Log(Vector3.Distance(transform.position, last_step_putted_position));
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

    private void setOffset()
    {
        
        float offsetDifference = listOfCollectedStacks[listOfCollectedStacks.Count-1].gameObject.transform.position.y - dashPosition.transform.position.y;
        
        //float offsetDifference = parentOfDashesPos.y - dashPosition.transform.position.y;

        if (offsetDifference > 0)
        {
            Vector3 pos = transform.position;
            
            pos.y -= offsetDifference;
            transform.localPosition = pos;
            

            /*
            Vector3 pos = parentOfDashes.transform.position;

            pos.y -= offsetDifference;
            parentOfDashes.transform.position = pos;*/
        }

        else if (offsetDifference < 0)
        {
            
            Vector3 pos = transform.position;
            pos.y += Math.Abs(offsetDifference);
            //pos.y = dashPosition.transform.position.y;

            transform.localPosition = pos;
            /*
            Vector3 pos = parentOfDashes.transform.position;
            pos.y += Math.Abs(offsetDifference);
            //pos.y = dashPosition.transform.position.y;

            parentOfDashes.transform.position = pos;
            */
        }
        /*
        float differenceOfOffsettes = offsetBetweenGroundAndCharacter - offsetDifference;
        if (differenceOfOffsettes >0)
        {

        }

        else if (differenceOfOffsettes<0)
        { }*/
    }

    public void setReachedMultiplierInfo(float f)
    {
        reachedMultiplier = f;
    }
    public void setEndPlatformReachedInfo()
    {
        endPlatformReached = true;
        
    }
    public void setPathEnteredInfo(bool b)
    {
        pathEntered = b;
        if (!b) //if path is exitted
        {
            /*
            Vector3 stacksPos = rb.transform.position;
            //stacksPos.y += 0.003f; //raising y position of stacks because above lowering operation affects all the character&stacks components but actually only character should be affected 
            //stacksPos.y += 0.29f;
            stacksPos.y = 0.85f; ///// degisebilir
            rb.transform.position = stacksPos;
            */
            setOffset();

         
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

    public void SetBackwardMovementAllowedInfo()
    {
        backwardMovementIsAllowed = false;
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




        
        line_entered = true;
        
       

    }
}
