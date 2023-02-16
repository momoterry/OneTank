using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMissile : BulletDir
{
    public float Acc = 10.0f;
    public float speedMax = 30.0f;

    public float AccDelay = 0.2f;

    protected override void Update()
    {
        if (lifeTime - myTime >= AccDelay)
        {
            if (speed < speedMax)
                speed += Acc * Time.deltaTime;
            else
                speed = speedMax;
        }
        base.Update();
    }
}
