using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDir : bullet
{
    public GameObject dirTarget;

    [SerializeField] protected float DefaultAngle = 0;

    public override void InitValue(DAMAGE_GROUP g, float damage, Vector3 targetVec, GameObject targetObject = null)
    {
        base.InitValue(g, damage, targetVec, targetObject);
        if (!dirTarget)
        {
            dirTarget = gameObject;
        }

        dirTarget.transform.rotation = Quaternion.Euler(90.0f, Vector3.SignedAngle(Vector3.forward, targetVec, Vector3.up) + DefaultAngle, 0);
    }
}
