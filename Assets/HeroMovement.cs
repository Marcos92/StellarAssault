using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    Vector2 destination;
    float range;
    bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            //draw circle and arrow
        }
    }

    private void OnMouseDown() 
    {
        isMoving = true;
    }

    private void OnMouseUp() 
    {
        if(isMoving)
        {
            isMoving = false;
            destination = 
        }
    }
}
