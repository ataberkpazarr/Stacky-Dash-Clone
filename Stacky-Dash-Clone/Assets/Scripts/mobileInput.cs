using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobileInput : MonoBehaviour
{
    public static mobileInput Instance { set; get; }

    [HideInInspector]
    public bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    [HideInInspector]
    public Vector2 swipeDelta, startTouch;
    private const float deadZone = 100;

    private void Awake() 
    {
        Instance = this;
    }

    private void Update()
    {
        
        tap = swipeLeft = swipeRight = swipeDown = swipeUp = false;

        //Input

        #region Bilgisayar Kontrolleri
        if (Input.GetMouseButtonDown(0))
        {
            tap = true; // tapped 
            startTouch = Input.mousePosition; // mouse position 
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero; // final position 
        }

        #endregion
        
      
        swipeDelta = Vector2.zero;
        if (startTouch != Vector2.zero) // starttouch is equal to vector2 zero if there is no initiative to move the character
        {
           
             if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;

            }
        }

       
        if (swipeDelta.magnitude > deadZone)
        {
           
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                
                if (x < 0)
                {
                    
                    swipeLeft = true;
                }
                else
                {
                    
                    swipeRight = true;
                }
            }
            else
            {
                
                if (y < 0)
                {
                    
                    swipeDown = true;
                }
                else
                {
                   
                    swipeUp = true;
                }
            }

            startTouch = swipeDelta = Vector2.zero;
        }
    }
}
