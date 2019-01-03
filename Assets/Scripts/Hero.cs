using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    Vector2 destination;
    float range;
    public float maxRange;
    bool isPressed;
    LineRenderer line;
    GameObject arrow;
    GameObject finalPosition;

    public Color Color;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2();

        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);

        arrow = transform.Find("Arrow").gameObject;
        finalPosition = transform.Find("Final Position").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = arrow.active = isPressed;

        if (isPressed)
        {
            Vector2 currentMousePos = GetMousePosition();
            Vector2 transformPositionV2 = transform.position;

            Vector2 direction = currentMousePos - transformPositionV2;


            if (direction.sqrMagnitude > maxRange * maxRange)
            {
                direction.Normalize();
                currentMousePos = transformPositionV2 + (direction * maxRange);
            }


            line.SetPosition(1, currentMousePos);
            arrow.transform.position = currentMousePos;
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
            destination = currentMousePos;
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
            finalPosition.active = true;
            finalPosition.transform.position = destination;
        }
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }
}
