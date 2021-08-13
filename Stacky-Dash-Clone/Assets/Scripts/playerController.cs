using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public GameObject dashpref;
    public GameObject parentOfDashes; 
    public static playerController instance;
    public float speed;
    public Rigidbody rb;
    public GameObject previousDash;
    private bool isMoving = false;  
    private bool lineNotExitted = false;
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



    // Update is called once per frame
    void Update() //freeze position & rotation may be required 
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || mobileInput.Instance.swipeLeft) && !isMoving && direction !=directionBeforeCollision.L)
        {
            rb.velocity = Vector3.left * speed * Time.deltaTime;
            isMoving = true;
            direction = directionBeforeCollision.L;
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || mobileInput.Instance.swipeRight)&& !isMoving && direction != directionBeforeCollision.R)
        {
            rb.velocity = Vector3.right * speed * Time.deltaTime;
            isMoving = true;
            direction = directionBeforeCollision.R;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || mobileInput.Instance.swipeUp)&& !isMoving && direction != directionBeforeCollision.F)
        {
            direction = directionBeforeCollision.F;
            rb.velocity = Vector3.forward * speed * Time.deltaTime;
            isMoving = true;
        }
        else if ((Input.GetKeyDown(KeyCode.DownArrow) || mobileInput.Instance.swipeDown)&& !isMoving && direction != directionBeforeCollision.B)
        {
            direction = directionBeforeCollision.B;
            rb.velocity = -Vector3.forward * speed * Time.deltaTime;
            isMoving = true;
        }
        //Debug.Log(rb.velocity);
        
        if (rb.velocity.magnitude >5.4f)
        {
            rb.velocity = rb.velocity.normalized * 5.4f;
        }

        if (line_entered)
        {
            
            Debug.Log(Vector3.Distance(transform.position, last_step_putted_position));
            // && 1 > Vector3.Distance(transform.position, last_step_putted_position)
            if (lollazo % 2 != 0 && 1f <Vector3.Distance(transform.position,last_step_putted_position)  )
            {
                last_step_putted_position = transform.position;
                Destroy(listOfCollectedStacks[0]);
                listOfCollectedStacks.RemoveAt(0);


                /// lowering the character 
               
                Vector3 characterPos = transform.localPosition;
                characterPos.y -= 0.08f;
                transform.localPosition = characterPos;


                ///
                //lineEnterPosition.z += 1.4f;
                GameObject sprite = (GameObject)Resources.Load("stackPrefab_", typeof(GameObject));
                //GameObject g = Instantiate(sprite, lineEnterPosition + new Vector3(0, 0.2f, 0), Quaternion.identity) as GameObject;
                GameObject g = Instantiate(sprite,  new Vector3(transform.position.x, lineEnterPosition.y + 0.2f, transform.position.z), Quaternion.identity) as GameObject;
                Debug.Log("aa");
            }
            lollazo += 1;
        }


        
        
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
        lineNotExitted = true;
    }

    public void spendDashes(Vector3 aaa)
    {
        lineEnterPosition = aaa;
        
        float startIn = 0;
        float every = 0.125f;
        //InvokeRepeating("instantiateDashSteps", startIn, every); //velocity 5.4 iyi
        //
        last_step_putted_position = aaa;
       instantiateDashSteps();
        //sInstantiate(dashpref, aaa, Quaternion.identity);
        /*
        while (lineNotExitted)
        {
            //Destroy(listOfCollectedStacks[0]);
            Instantiate(GameObject.FindWithTag("dashPrefab"), this.transform.position + new Vector3(0, 0, -1), Quaternion.identity);
        }*/

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
