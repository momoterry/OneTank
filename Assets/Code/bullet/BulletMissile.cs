using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMissile : BulletDir
{
    public float Acc = 10.0f;

    protected override void Update()
    {
        speed += Acc * Time.deltaTime;
        base.Update();
    }
}
