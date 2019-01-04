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

    public GameCard(CardData cardData)
    {
        CardData = cardData;
        PopulateCard(CardData);
    }

    void Start() => PopulateCard(CardData);

    private void PopulateCard(CardData cardData){
        TitleText.GetComponent<Text>().text = cardData.Title;
        DescriptionText.GetComponent<Text>().text = cardData.Description;
        CoolDownText.GetComponent<Text>().text = cardData.Cooldown.ToString();
        ImageContainer.GetComponent<Image>().sprite = cardData.Image;
        if (cardData.Hero != null && cardData.Hero.Color != null) {
            CardFace.GetComponent<Image>().color = cardData.Hero.Color;
        }
    }

    private void Cast() {
        if (CardData != null && CardData.Ability != null){
            CardData.Ability.Execute();
        }
    }
}
