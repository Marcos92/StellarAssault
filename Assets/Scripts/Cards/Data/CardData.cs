using UnityEngine;

public class CardData : ScriptableObject
{
    public int Cooldown = 0;

    public Sprite Image = null;
    
    public string Title = "Game Card Title";

    public string Description = "This is the card Description";

    public Hero Hero = null;

    public CardTarget Target = CardTarget.World;
    public CardAbility Ability;
}
