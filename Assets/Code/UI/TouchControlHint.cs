using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlHint : MonoBehaviour
{
    [SerializeField] protected GameObject formationRoot;
    public float TimeHintStay = 1.0f;

    protected enum Phase
    {
        OFF,
        ON,
        FADE_OFF,
    }
    protected Phase currPhase = Phase.OFF;
    protected Phase nextPhase = Phase.OFF;
    protected float phaseTime = 0;

    private void Update()
    {
        if (currPhase != nextPhase)
        {
            if (nextPhase == Phase.OFF)
            {
                gameObject.SetActive(false);
            }
            currPhase = nextPhase;
            phaseTime = 0;
        }

        phaseTime += Time.deltaTime;

        if (currPhase == Phase.FADE_OFF)
        {
            if (phaseTime >= TimeHintStay)
            {
                nextPhase = Phase.OFF;
            }
        }
    }

    public void SetFormationVec(Vector3 vec)
    {
        formationRoot.transform.rotation = Quaternion.LookRotation(vec.normalized, Vector3.up);
    }

    public void SetOnOff(bool isOn)
    {
        //gameObject.SetActive(isOn);
        if (isOn)
        {
            gameObject.SetActive(isOn);
            nextPhase = Phase.ON;
        }
        else
        {
            if (currPhase == Phase.ON)
            {
                nextPhase = Phase.FADE_OFF;
            }
        }
    }
    public void SetFormationOnOff(bool isOn)
    {
        formationRoot.SetActive(isOn);
    }
}
