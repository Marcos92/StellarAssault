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
    public Color DefaultSpellColor;
    private Transform Target;
    private Color CardColor;

    LineRenderer line;
    Vector2 finalPosition;

    private All_Seeing_Eye allSeingEye;

    bool isPressed;

    bool isAvailable = true;

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
        allSeingEye.RegisterActionEndCallback(HandleTurnStartCallbackDelegate);
    }

    private void HandleTurnStartCallbackDelegate() {

    }

    private void HandleOnTurnEndCallbackDelegate(int turnNumber) {

    }

    private void OnMouseDown() 
    {
        if (isAvailable){
            Target = null;
            isPressed = true;
        }
    }

    private void OnMouseUp() 
    {
        if (isPressed)
        {
            isPressed = false;
            if (Target && CardData.Ability) {
                Execute(Target, CardData.Ability);
            }
        }
    }

    private void Execute(Transform target, CardAbility ability) {
        Debug.Log($"Target {target.name} got hit by {ability.name} for {ability.HealthChange} healing points.");
    }

    void Update()
    {
        line.enabled = isPressed;

        if (isPressed)
        {
            Vector2 currentMousePos = GetMousePosition();
            Vector2 transformPositionV2 = transform.position;
            Vector2 direction = currentMousePos - transformPositionV2;
            line.SetPosition(1, currentMousePos);
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
                Target = null;
            }

            if (Target != null){
                Target.gameObject.GetComponent<Hero>().Highlight(CardColor);
            }
        }
    }

    private void PopulateCard(CardData cardData){

        TitleText.GetComponent<Text>().text = cardData.Title;
        DescriptionText.GetComponent<Text>().text = cardData.Description;
        CoolDownText.GetComponent<Text>().text = cardData.Cooldown.ToString();
        ImageContainer.GetComponent<Image>().sprite = cardData.Image;

        if (cardData.Hero != null && cardData.Hero.Color != null) {
            CardFace.GetComponent<Image>().color = cardData.Hero.Color;
        }

        line = transform.GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);

        if (cardData.Hero != null && cardData.Hero.Color != null) {
            CardColor = cardData.Hero.Color;
        } else {
            CardColor = DefaultSpellColor;
        }
        line.startColor = CardColor;
        line.endColor = new Color(0,0,0,0);
    }

    private Vector2 GetMousePosition()
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.nearClipPlane));
    }
}
