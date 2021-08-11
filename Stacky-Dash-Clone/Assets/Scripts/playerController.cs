using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{


    public GameObject parentOfDashes; 
    public static playerController instance;
    public float speed;
    public Rigidbody rb;
    public GameObject previousDash;
    private bool isMoving = false; //kullanılmıyo su an 

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

    }

    public void TakeDashes(GameObject gam)
    {
        gam.transform.SetParent(parentOfDashes.transform);
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
}
