using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovement : MonoBehaviour
{
    public Transform target; //position of the character
    public Vector3 offset;

    public float lerpVal;
    
    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 cameraPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position,cameraPos,lerpVal*Time.deltaTime);
    }
}
