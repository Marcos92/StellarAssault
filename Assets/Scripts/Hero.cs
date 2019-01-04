using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    Vector2 destination;
    Vector2 endTurnPosition;
    public float maxRange;
    bool isPressed;
    LineRenderer line;
    GameObject arrow;
    GameObject finalPosition;
    All_Seeing_Eye allSeingEye;

    public Color Color;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2();

        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.startColor = Color.black;
        line.endColor = Color;

        arrow = transform.Find("Arrow").gameObject;
        finalPosition = transform.Find("Final Position").gameObject;
        allSeingEye = GameObject.Find("Illuminatti").gameObject.GetComponent<All_Seeing_Eye>();
        allSeingEye.RegisterTurnEndCallback(HandleOnTurnEndCallbackDelegate);
        allSeingEye.RegisterMovementCallback(HandleMovementCallbackDelegate);
    }

    // Update is called once per frame
    void Update()
    {
        line.enabled = isPressed;
        arrow.SetActive(isPressed);

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

    public void Highlight(Color spellColor) {
        transform.GetComponent<SpriteRenderer>().color = spellColor;
    }

    public void ClearHighlight() {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
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
            finalPosition.SetActive(true);
            finalPosition.transform.position = destination;
        }
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }

    private void HandleOnTurnEndCallbackDelegate(int turnNumber)
    {
        endTurnPosition = transform.position;
        line.SetPosition(0, destination);
        finalPosition.SetActive(false);
    }

    void HandleMovementCallbackDelegate(float step)
    {
        Debug.Log("Updating position to: " + step);
        Vector2 transformPositionV2 = transform.position;
        Vector2 direction = destination - endTurnPosition;
        direction.Normalize();
        transform.position = endTurnPosition + (direction * step);
    }

}
