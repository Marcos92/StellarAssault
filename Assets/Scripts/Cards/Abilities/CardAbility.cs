using UnityEngine;

public class CardAbility : ScriptableObject{

    public string Name = "Ability Name";
    [TextArea]
    public string Description = "Flavour Text";
    public int HealthChange = 0;
    public int SpeedChange = 0;
    public int Radius = 0;
    public bool StunTarget = false;
}