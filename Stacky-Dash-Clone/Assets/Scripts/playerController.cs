using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    private bool isMoving = false; //kullanılmıyo su an 
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update() //freeze position & rotation may be required 
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || mobileInput.Instance.swipeLeft)
        {
            rb.velocity = Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || mobileInput.Instance.swipeRight)
        {
            rb.velocity = Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || mobileInput.Instance.swipeUp)
        {
            rb.velocity = Vector3.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || mobileInput.Instance.swipeDown)
        {
            rb.velocity = -Vector3.forward * speed * Time.deltaTime;
        }

    }
}
