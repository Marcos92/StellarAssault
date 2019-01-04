using UnityEngine;

public class CardAbility : ScriptableObject{
    public int HealthChange = 0;
    public int HealthChangePerTurn = 0;
    public int DivideHealthChange = 1;
    public double SpeedChange = 1;
    public double DamageBoost = 1;
    public int EffectDuration = 1;
    public int Radius = 0;
    public bool StunTarget = false;
    public bool FromHero = false;
}