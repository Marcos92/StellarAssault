using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTime : MonoBehaviour
{
    private All_Seeing_Eye allSeeingEye;
    private float frameScaleX;

    // Start is called before the first frame update
    void Start()
    {
        allSeeingEye = GameObject.Find("Illuminatti").GetComponent<All_Seeing_Eye>();
        allSeeingEye.RegisterTurnTickCallback(HandleTurnTickCallbackDelegate);

        frameScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void HandleTurnTickCallbackDelegate(float turnPercentage)
    {
        transform.localScale = new Vector3(frameScaleX * (1 - turnPercentage), transform.localScale.y, transform.localScale.z);
    }

}
