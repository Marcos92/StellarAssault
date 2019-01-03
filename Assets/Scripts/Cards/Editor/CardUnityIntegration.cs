using UnityEditor;
using UnityEngine;

public class CardUnityIntegration
{
    [MenuItem("Assets/Create/Card/Data")]
    public static void CreateCardData() => ScriptableObjectUtility2.CreateAsset<CardData>();
}
