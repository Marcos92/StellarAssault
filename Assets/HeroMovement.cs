using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    Vector2 destination;
    float range;
    public float maxRange;
    bool isPressed;
    LineRenderer line;
    GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2();

        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, Vector2.zero);

        arrow = transform.Find("Arrow").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = arrow.active = isPressed;

        if (isPressed)
        {
            Vector2 currentMousePos = GetMousePosition();

            Vector2 direction = currentMousePos;
            direction.Normalize();

            if(currentMousePos.magnitude > maxRange)
            {
                currentMousePos = direction * maxRange;
            }

            line.SetPosition(1, currentMousePos);
            arrow.transform.position = currentMousePos;
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }

        
    }

    private void OnMouseDown() 
    {
        isPressed = true;
    }

    private void OnMouseUp() 
    {
        if (isPressed)
        {
            isPressed = false;
            destination = GetMousePosition();
        }
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }
}
