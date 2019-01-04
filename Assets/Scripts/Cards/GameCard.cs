using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CardData))]

public class GameCard : MonoBehaviour
{
    [SerializeField]
    
    public CardData CardData = null;
    public GameObject TitleText = null;
    public GameObject DescriptionText = null;
    public GameObject ImageContainer = null;
    public GameObject CoolDownText = null;
    public GameObject CardFace = null;
    public GameObject TargetMarker = null;
    public Color DefaultSpellColor;
    private Transform Target;

    private Vector2 TargetLocation;
    private Color CardColor;

    LineRenderer line;
    Vector2 finalPosition;

    private All_Seeing_Eye allSeingEye;

    bool isPressed;

    bool isDisable;

    public GameCard(CardData cardData)
    {
        CardData = cardData;
        PopulateCard(CardData);
    }

    void Start() {
        PopulateCard(CardData);
        RegisterStateMachine();
    }

    private void RegisterStateMachine() {
        allSeingEye = GameObject.Find("Illuminatti").gameObject.GetComponent<All_Seeing_Eye>();
        allSeingEye.RegisterTurnEndCallback(HandleOnTurnEndCallbackDelegate);
        allSeingEye.RegisterMovementEndCallback(HandleActionStartCallbackDelegate);
    }

    private void HandleOnTurnEndCallbackDelegate(int turnNumber) {

    }

    private void HandleActionStartCallbackDelegate() {
    }

    private void OnMouseDown() 
    {
        if (!isDisable){
            Target = null;
            isPressed = true;
        }
    }

    private void OnMouseUp() 
    {
        if (isPressed)
        {
            isPressed = false;
        }
    }

    private void Execute(Transform target, CardAbility ability) {
        Debug.Log($"Target {target.name} got hit by {ability.name} for {ability.HealthChange} healing points.");
    }

    void Update()
    {
        if (line) line.enabled = isPressed;

        if (isPressed)
        {
            Debug.Log(CardData.Target);
            switch (CardData.Target)
            {
                case CardTarget.Allies:
                case CardTarget.Enemies:
                    SetCharacterMouseTarget();
                    break;
                case CardTarget.World:
                    SetWorldTarget();
                    break;
                case CardTarget.Self:
                default:
                    TargetLocation = CardData.Hero.transform.position;
                    break;
            }
            if (Target != null){
                Target.gameObject.GetComponent<Hero>().Highlight(CardColor);
            }
        }
    }

    private void SetWorldTarget() {
        TargetLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        TargetMarker.transform.position = TargetLocation;
    }

    private void SetCharacterMouseTarget() {
        Debug.Log("SetCharacterMouseTarget");
        Vector2 currentMousePos = GetMousePosition();
        Vector2 transformPositionV2 = transform.position;
        Vector2 direction = currentMousePos - transformPositionV2;
        if (line) line.SetPosition(1, currentMousePos);
        finalPosition = currentMousePos;

        Vector2 mouseWorldCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldCoordinates, Vector2.zero);
        
        if (hit.transform != null) 
        {
            if (hit.transform.gameObject.tag == CardData.Target.ToString()){
                Target = hit.transform;
            }
        } else if (Target != null){
            Target.gameObject.GetComponent<Hero>().ClearHighlight();
        }
        Target = null;
    }

    private void PopulateCard(CardData cardData){

        TitleText.GetComponent<Text>().text = cardData.Title;
        DescriptionText.GetComponent<Text>().text = cardData.Description;
        CoolDownText.GetComponent<Text>().text = cardData.Cooldown.ToString();
        ImageContainer.GetComponent<Image>().sprite = cardData.Image;

        if (cardData.Hero != null && cardData.Hero.Color != null) {
            CardFace.GetComponent<Image>().color = cardData.Hero.Color;
        }

        if (cardData.Hero != null && cardData.Hero.Color != null) {
            CardColor = cardData.Hero.Color;
        } else {
            CardColor = DefaultSpellColor;
        }

        if (cardData.Target == CardTarget.Allies || cardData.Target == CardTarget.Enemies){
            line = transform.GetComponent<LineRenderer>();
            line.SetPosition(0, transform.position);
            line.startColor = CardColor;
            line.endColor = new Color(0,0,0,0.5f);
        }

        
        TargetMarker.transform.GetComponent<SpriteRenderer>().color = CardColor;
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }
}
