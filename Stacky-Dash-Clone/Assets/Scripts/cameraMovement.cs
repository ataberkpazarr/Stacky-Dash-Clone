using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform target; //position of the character
    public Vector3 offset;
    public static cameraMovement instance;
    public float lerpVal;
    private bool reachedFinishLineBool = false;
    // Update is called once per frame
    void LateUpdate()
    {
        if (!reachedFinishLineBool)
        {
            Vector3 cameraPos = target.position + offset;
            //transform.position = Vector3.Lerp(transform.position,cameraPos,lerpVal*Time.deltaTime);
            transform.position = cameraPos;
        }
        else 
        {
            Vector3 cameraPos = target.position + offset + new Vector3(2, -2, 2);

            transform.position = cameraPos;

        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    public void reachedFinishLine()
    {

        reachedFinishLineBool = true;

    }
}