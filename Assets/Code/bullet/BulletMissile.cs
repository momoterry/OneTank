using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMissile : BulletDir
{
    public float Acc = 10.0f;
    public float speedMax = 30.0f;

    public float AccDelay = 0.2f;

    protected float initSpeed;
    protected float accTimeMax;

    override protected void Start()
    {
        base.Start();
        initSpeed = speed;
        accTimeMax = (speedMax - initSpeed) / Acc;
    }

    protected override void Update()
    {
        if (lifeTime - myTime >= AccDelay)
        {
            float t = (lifeTime - myTime - AccDelay) / accTimeMax;
            speed = Mathf.SmoothStep(initSpeed, speedMax, t);

            //speed = Mathf.Min(initSpeed + Acc * t, speedMax);


            //if (speed < speedMax)
            //    speed += Acc * Time.deltaTime;
            //else
            //    speed = speedMax;
        }

        base.Update();
    }
}
