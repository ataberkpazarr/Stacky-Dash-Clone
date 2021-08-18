using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; //free unity package    
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    private float reachedMultiplier;

    [SerializeField] private GameObject levelEndPanel;
    [SerializeField] private Text levelEndText;
    [SerializeField] private Text scoreBoard;
    [SerializeField] private GameObject dashPosition;
    [SerializeField] private GameObject previousDash;
    [SerializeField] public float speedAtLine = 5f; //speed in the non-straight line
    [SerializeField] private PathCreator pathCreator_; // for the non-straight line creation, an Unity plugin is used by me
    [SerializeField] private float interpolateAmount;
    [SerializeField] GameObject dashpref; // for instantiating dashes when its required 
    [SerializeField] GameObject parentOfDashes; // for reaching dashes, which follows character

    private int totalCollected=0; // total collected stack info
    private bool pathEntered; //check if path is entered, set by following stacks, after trigger by collision
    float distanceTravelled; // for calculation of the movement in the path

    bool endPlatformReached; //check if end-platform is entered, set by following stacks, after trigger by collision
    private float pathEnterPositionY; // y dimension of character, when it entered to path

    


    public static playerController instance; //this
    public float speed; 
    public Rigidbody rb; //character
   
    private bool isMoving = false; //
    private bool backwardMovementIsAllowed; //check if it is in backward movement non-allowed area, set by following stacks, after trigger by collision
    int lollazo = 0;
    Vector3 lineEnterPosition; //line enter position for dropping stacks correctly
    Vector3 last_step_putted_position; //where last stack dropped
    bool line_entered = false; // set by followingstacks.cs

    public List<GameObject> listOfCollectedStacks; 


    float fark;
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
          

            
            fark = previousDash.transform.position.y - transform.position.y;

            

        }
    }


    void Update() 
    {
        
       
        normalizeVelocity(); 
        updateScore(); 
        checkIfCollectedStacksRunOut();
        if (!pathEntered) // if path not entered
        {
            if (line_entered) //but if line entered
            {
                dropTiles(); 
            }
            Move();

        }

        else if (pathEntered)
        {

            handlePathMovement(); //path movement is different from straight line movement
        }
        

    }


   

    private void dropTiles()
    {
        if (lollazo % 2 != 0 && 1f < Vector3.Distance(transform.position, last_step_putted_position) && listOfCollectedStacks.Count > 1) //check if the distance, between last stack putted position and current position of character, higher than 1 unit. Stacks are scaled 0.8x0.8 in x&z dimensions for making visibility better, rather than 1x1
        {
            last_step_putted_position = transform.position; // saving the location where the last stack dropped so I will be able to drop a new one in every 1 unit by the help of above if condition checks
            Destroy(listOfCollectedStacks[0]); // destroy the last collected stack
            listOfCollectedStacks.RemoveAt(0); //remove the last collected stack from the list //bak buraya

            /// lowering the character 
            Vector3 characterPos = transform.localPosition;
            characterPos.y -= 0.09f; //lowering y position of character
            if (!(characterPos.y < 0.2f)) // character should not go to below of the main ground
            {
                
                transform.position = characterPos;
            }
            Vector3 stacksPos = parentOfDashes.transform.localPosition;
            stacksPos.y += 0.06f; //raising y position of stacks because above lowering operation affects all the character&stacks components whereas only character should be affected 
            parentOfDashes.transform.localPosition = stacksPos;
            GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject)); //take the required stack prefab that will be newly instantiated, from assets/resources/stackprefab_
            GameObject g = Instantiate(sprite, new Vector3(transform.position.x, lineEnterPosition.y + 0.2f, transform.position.z), Quaternion.identity) as GameObject; // instantiate new dropped stack 


        }

        lollazo += 1;
    }
    private void normalizeVelocity() //an upper limit for velocity o character
    {

        if (rb.velocity.magnitude > 5.4f)
        {
            rb.velocity = rb.velocity.normalized * 5.4f;
        }
    }


    private void Move() //all move operations, including in line are handled by this function
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || mobileInput.Instance.swipeLeft) && !isMoving && direction != directionBeforeCollision.L)
        {
            
            rb.velocity = Vector3.left * speed * Time.deltaTime;
            isMoving = true;
            direction = directionBeforeCollision.L;


        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || mobileInput.Instance.swipeRight) && !isMoving && direction != directionBeforeCollision.R)
        {

            rb.velocity = Vector3.right * speed * Time.deltaTime;
            isMoving = true;
            direction = directionBeforeCollision.R;
            if (!backwardMovementIsAllowed)
            {
                backwardMovementIsAllowed = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || mobileInput.Instance.swipeUp) && !isMoving && direction != directionBeforeCollision.F)
        {

            direction = directionBeforeCollision.F;
            rb.velocity = Vector3.forward * speed * Time.deltaTime;
            isMoving = true;
            if (!backwardMovementIsAllowed)
            {
                backwardMovementIsAllowed = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || mobileInput.Instance.swipeDown) && !isMoving && direction != directionBeforeCollision.B && backwardMovementIsAllowed)
        {

            direction = directionBeforeCollision.B;
            rb.velocity = -Vector3.forward * speed * Time.deltaTime;
            isMoving = true;
        }
    }

    private void handlePathMovement()
    {
        dropTiles();
        if (!(pathEnterPositionY - 0.09f <0)) // character should not go to below level of main ground
        {
            pathEnterPositionY -= 0.09f * speedAtLine * Time.deltaTime;
        }
        distanceTravelled += speedAtLine * Time.deltaTime;
        transform.position = new Vector3(0, pathEnterPositionY, 0)  + pathCreator_.path.GetPointAtDistance(distanceTravelled);


    }
    private void checkIfCollectedStacksRunOut() //if collectedstacks run out or not
    {
        if (listOfCollectedStacks.Count == 1) // most bottom is not loseable, thats why if we have 1 rather than 0, It means we already spent all
        {
            if (endPlatformReached) //if endplatform is reached
            {
                float coefficient = 1; // multiplier coefficient
                setMovingInfo(true); //for ending movement



                if (reachedMultiplier == 1)
                {
                    coefficient = 1.1f;
                }

                else if (reachedMultiplier == 2)
                {
                    coefficient = 1.2f;
                }

                else if (reachedMultiplier == 3)
                {
                    coefficient = 1.3f;
                }

                else if (reachedMultiplier == 4)
                {
                    coefficient = 1.4f;
                }

                else if (reachedMultiplier == 5)
                {
                    coefficient = 1.5f;
                }

                levelEndText.text = "Congrats for having X" + coefficient.ToString();


                scoreBoard.text = (totalCollected * coefficient).ToString();


                levelEndPanel.SetActive(true);

                Invoke("load_next_scene", 1f);
            }

            //setOffset(); // commentteed tı bak buraya 


        }


    }
    private void updateScore()
    {
        if (!endPlatformReached)
        {
            scoreBoard.text = totalCollected.ToString();
        }
    }
    private void load_next_scene() //calling with invoke. That's why It says 0 
    {
        Destroy(this);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


    }
    private void setOffset() //the y position get harmed after it enters to "path", here, y position issues are being handled
    {
        
        float offsetDifference = listOfCollectedStacks[listOfCollectedStacks.Count-1].gameObject.transform.position.y - dashPosition.transform.position.y;
        
       

        if (offsetDifference > 0)
        {
            Vector3 pos = transform.position;
            
            pos.y -= offsetDifference;
            transform.position = pos;
            

      
        }

        else if (offsetDifference < 0)
        {
            
            Vector3 pos = transform.position;
            pos.y += Math.Abs(offsetDifference);
           

            transform.position = pos;
            
        }
        
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
            
            setOffset();

         
        }

    }
    public void rearrangeTheCharacterPos() // after collision to borders, character position should be rearrenged 
    {
        if (direction == directionBeforeCollision.L)
        {




        Vector3 characterPos = transform.localPosition;
            characterPos.x += 0.3f *45 *Time.deltaTime;
            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);
           

        }
        else if (direction == directionBeforeCollision.R)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.x -= 0.3f * 45 * Time.deltaTime; ;

            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            

        }
        else if (direction == directionBeforeCollision.B)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.z += 0.3f * 45 * Time.deltaTime; ;
            transform.position =Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            


        }
        else if (direction == directionBeforeCollision.F)
        {
            Vector3 characterPos = transform.localPosition;
            characterPos.z -= 0.3f * 45 * Time.deltaTime; ;
            transform.position = Vector3.Lerp(transform.localPosition, characterPos, Time.deltaTime * 50);

            

        }



    }

    public void SetBackwardMovementAllowedInfo()
    {
        backwardMovementIsAllowed = false;
    }

    public void IncreaseTotalCollected()
    {

        totalCollected = totalCollected + 1;
    }
    public void TakeDashes(GameObject gam)
    {
        
        gam.transform.SetParent(parentOfDashes.transform);
        listOfCollectedStacks.Add(gam); // put collected stack to list, for reaching it later
        
        Vector3 pos = previousDash.transform.localPosition;
        pos.y -= 0.080f;
        
        gam.transform.localPosition  = pos;
        Vector3 characterPos = transform.localPosition;
        characterPos.y += 0.080f;
        
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
        if (pathEntered )
        {
            
            if (SceneManager.GetActiveScene().buildIndex == 0)  // if in level 1
            {
                pathEnterPositionY = 2.2f;

            }
            else if (SceneManager.GetActiveScene().buildIndex == 1) //level 2 
            {
                pathEnterPositionY = 2.6f+(1.2f * (listOfCollectedStacks.Count/40)); // checks what percentage of total collectable stacks are being collected. The amount which character will be raied in dimension Y, is being decided respected to this number
            }
            else if (SceneManager.GetActiveScene().buildIndex == 2) //Level 3
                {
                pathEnterPositionY = 2.6f+(2.2f * (listOfCollectedStacks.Count/47));
            }
            
        }
       

        line_entered = true;
        last_step_putted_position = aaa;

        
        
       

    }
   
    
}
