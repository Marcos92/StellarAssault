using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // Enums
    public enum AttackType
    {
        CircleAttack,
        ConeAttack,
        DonnutAttack,
        LineAttack
    };

    // Characteristics
    public float maxMovementRange;
    public float attackRange;
    public Color Color;
    public AttackType attackType;

    // Movement
    Vector2 destination;
    Vector2 endTurnPosition;
    Vector2 direction;
    LineRenderer line;
    GameObject arrow;
    GameObject finalPosition;
    bool newDestination = false;
    bool isPressed;
    bool isBlocked;

    // Attack
    bool attackOnce = true;
    GameObject attackAnimation;

    // Manager
    All_Seeing_Eye allSeingEye;

    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2();

        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
        line.startColor = Color.black;
        line.endColor = Color;

        arrow = transform.Find("Arrow").gameObject;

        attackAnimation = transform.Find("Attack_Anim").gameObject;

        finalPosition = transform.Find("Final Position").gameObject;
        allSeingEye = GameObject.Find("Illuminatti").gameObject.GetComponent<All_Seeing_Eye>();

        //Callback registration
        allSeingEye.RegisterTurnEndCallback(HandleOnTurnEndCallbackDelegate);
        allSeingEye.RegisterMovementCallback(HandleMovementCallbackDelegate);
        allSeingEye.RegisterActionCallback(HandleActionCallbackDelegate);
        allSeingEye.RegisterActionEndCallback(HandleActionEndCallbackDelegate);
    }

    // Update is called once per frame
    void Update()
    {
        if (allSeingEye.GetState() == All_Seeing_Eye.GameState.Turn)
        {
            CheckNextHeroDestination();
        }
    }

    private void CheckNextHeroDestination()
    {
        line.enabled = isPressed;
        arrow.SetActive(isPressed);

        if (isPressed)
        {
            Vector2 currentMousePos = GetMousePosition();
            Vector2 transformPositionV2 = transform.position;

            direction = currentMousePos - transformPositionV2;

            if (direction.sqrMagnitude > maxMovementRange * maxMovementRange)
            {
                direction.Normalize();
                currentMousePos = transformPositionV2 + (direction * maxMovementRange);
            }

            line.SetPosition(1, currentMousePos);
            arrow.transform.position = currentMousePos;
            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

            //Debug.Log(isBlocked);

            if(!isBlocked)
            {
                destination = currentMousePos;
                //Debug.Log(destination);
            }

            if (!newDestination)
            {
                newDestination = true;
            }

            if (!isBlocked)
            {
                destination = currentMousePos;
            }
        }
    }

    public void Highlight(Color spellColor) {
        transform.GetComponent<SpriteRenderer>().color = spellColor;
    }

    public void ClearHighlight() {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }
    
    private void FixedUpdate() {
        if(!isPressed)
            return;

        LayerMask mask = LayerMask.GetMask("Obstacle");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxMovementRange, mask);
        isBlocked = hit.collider != null && hit.collider.transform != transform;
    }

    private void OnMouseDown() 
    {
        if (allSeingEye.GetState() == All_Seeing_Eye.GameState.Turn)
        {
            isPressed = true;
        }
    }

    private void OnMouseUp() 
    {
        if (allSeingEye.GetState() == All_Seeing_Eye.GameState.Turn && isPressed)
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
        if (newDestination)
        {
            endTurnPosition = transform.position;
            line.SetPosition(0, transform.position);
            finalPosition.SetActive(false);
        }
    }

    void HandleMovementCallbackDelegate(float step)
    {
        if (newDestination)
        {
            Vector2 transformPositionV2 = transform.position;
            Vector2 direction = destination - endTurnPosition;
            direction.Normalize();
            transform.position = endTurnPosition + (direction * step);
        }
    }

    void HandleActionCallbackDelegate()
    {
        // Already moved, so we can reset newDestination
        if (newDestination)
        {
            newDestination = false;
        }

        if (attackOnce)
        {
            attackOnce = false;
            AnimateAttack();
            ApplyAttackOnArea();
        }
    }

    private void ApplyAttackOnArea()
    {
        switch(attackType)
        {
            case AttackType.CircleAttack:
                break;
            default:
                break;
        }
    }

    void HandleActionEndCallbackDelegate()
    {
        attackOnce = true;
        Animator attackAnimationAnimator = GetComponent<Animator>();
        attackAnimationAnimator.StopPlayback();
        attackAnimationAnimator.enabled = false;
    }


    void AnimateAttack()
    {
        
        attackAnimation.transform.position = transform.position;
        attackAnimation.SetActive(true);

        Animator attackAnimationAnimator = attackAnimation.GetComponent<Animator>();
        attackAnimationAnimator.Play("Attac", -1, 0.0f);
    }

}
