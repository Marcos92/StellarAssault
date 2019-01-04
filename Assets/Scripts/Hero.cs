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
    public float health;
    float initialHealth;
    public float attackDamage;
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

    Animator animator;
    SpriteRenderer sprite;

    Transform healthBar;
    float initialHealthBarSize;

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

        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        initialHealth = health;
        healthBar = transform.Find("HealthBar").transform;
        initialHealthBarSize = healthBar.localScale.x;

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
        if(health <= 0)
            return;

        if (newDestination)
        {
            endTurnPosition = transform.position;
            finalPosition.SetActive(false);
        }
    }

    void HandleMovementCallbackDelegate(float step)
    {
        if(health <= 0)
            return;
            
        if (newDestination)
        {
            animator.SetInteger("direction", 1);
            sprite.flipX = destination.x < 0;
            Vector2 transformPositionV2 = transform.position;
            Vector2 playerDirection = destination - endTurnPosition;
            playerDirection.Normalize();
            transform.position = endTurnPosition + (playerDirection * step);
        }
    }

    void HandleActionCallbackDelegate()
    {
        if(health <= 0)
            return;
            
        // Already moved, so we can reset newDestination
        if (newDestination)
        {
            line.SetPosition(0, transform.position);
            newDestination = false;
            animator.SetInteger("direction", 0);
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
                String tagToFind = "Enemies";
                if (tag == "Enemies")
                {
                    tagToFind = "Allies";
                }
                GameObject[] heroes = GameObject.FindGameObjectsWithTag(tagToFind);
                foreach(GameObject hero in heroes)
                {
                    Hero heroScript = hero.GetComponent<Hero>();
                    if (heroScript != null)
                    {
                        if (heroScript.NextToPlayer(transform.position, attackRange)) {
                            Debug.Log("[" + name + "] " + " -> (" + heroScript.name + ") is next to me");
                            heroScript.DoDamage(attackDamage);
                        }
                    }
                }


                break;
            default:
                break;
        }
    }

    public bool NextToPlayer(Vector2 otherPosition, float otherAttackRange)
    {
        float distance = Vector2.Distance(otherPosition, transform.position);
        return distance < otherAttackRange;
    }

    public void DoDamage(float otherAttackPower)
    {
        health -= otherAttackPower;
        Debug.Log("[" + name + "] -> Health is now " + health);

        float size;

        if (health <= 0)
        {
            OnPlayerDeath();
            size = 0;
        }
        else
        {
            size = health * initialHealthBarSize / initialHealth;
        }

        healthBar.localScale = new Vector2(size, healthBar.localScale.y);

        SpriteRenderer healthSprite = healthBar.GetComponent<SpriteRenderer>();

        if(health / initialHealth <= 0.2)
        {
            healthSprite.color = Color.red;
        }
        else if(health / initialHealth <= 0.5)
        {
            healthSprite.color = Color.yellow;
        }
    }

    private void OnPlayerDeath()
    {
        animator.SetBool("die", true);
    }

    void HandleActionEndCallbackDelegate()
    {
        if(health <= 0)
            return;
            
        attackOnce = true;
        Animator attackAnimationAnimator = attackAnimation.GetComponent<Animator>();
        attackAnimationAnimator.StopPlayback();
        attackAnimation.SetActive(false);
    }


    void AnimateAttack()
    {
        attackAnimation.transform.position = transform.position;
        attackAnimation.SetActive(true);

        Animator attackAnimationAnimator = attackAnimation.GetComponent<Animator>();
        attackAnimationAnimator.Play("Attac", -1, 0.0f);

        animator.SetTrigger("attack");
    }

}
