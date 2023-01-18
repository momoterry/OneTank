using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlHint : MonoBehaviour
{
    [SerializeField] protected GameObject formationRoot;

    public void SetFormationVec(Vector3 vec)
    {
        formationRoot.transform.rotation = Quaternion.LookRotation(vec.normalized, Vector3.up);
    }

    public void SetFormationOnOff(bool isOn)
    {
        formationRoot.SetActive(isOn);
    }
}
