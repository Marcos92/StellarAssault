using UnityEditor;
using UnityEngine;

public class CardUnityIntegration
{
    [MenuItem("Assets/Create/Card/Data")]
    public static void CreateCardData() => ScriptableObjectUtility2.CreateAsset<CardData>();

    [MenuItem("Assets/Create/Card/Ability")]
    public static void CreateAbilityData() => ScriptableObjectUtility2.CreateAsset<CardAbility>();
}
