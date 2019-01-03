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
        destination = new Vector2();
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
        Debug.Log("Mouse down!");
    }

    private void OnMouseUp() 
    {
        if(isMoving)
        {
            isMoving = false;
            destination = GetMousePosition();
            Debug.Log("Mouse up!");
        }
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(destination.x, destination.y, 1), 0.1f);
    }
}
