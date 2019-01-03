using UnityEditor;
using UnityEngine;

public class CardUnityIntegration
{
    [MenuItem("Assets/Create/CardData")]
    public static void CreateYourScriptableObject() => ScriptableObjectUtility2.CreateAsset<CardData>();
}
