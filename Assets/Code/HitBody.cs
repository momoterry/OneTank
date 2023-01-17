using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HealData
{
    public float healAbsoluteValue;
    public float healRatio;
    public float healResult;
}
public class HitBody : MonoBehaviour
{
    // Start is called before the first frame update
    public float HP_Max = 100.0f;
    public float DamageRatio = 1.0f;
    public float blockAngle = -1.0f;
    public float blockRatio = 0.5f;
    // 
    protected float hp;

    Hp_BarHandler myHPHandler;
    TankController myTankController;

    // Public Get Function
    public float GetHPMax() { return HP_Max; }
    public float GetHP() { return hp; }

    void Start()
    {
        hp = HP_Max;
        myHPHandler = GetComponent<Hp_BarHandler>();
        myTankController = GetComponent<TankController>();
    }

    private void Update()
    {
        if (myHPHandler )
        {
            myHPHandler.SetHP(hp, HP_Max);
        }
    }

    void OnDamage(Damage theDamage)
    {
        float realDamage = theDamage.damage * DamageRatio;
        bool isBlock = false;
        if (blockAngle > 0 && myTankController)
        {
            Vector3 hitDir = theDamage.hitPos - transform.position;
            hitDir.y = 0;
            float angle = Vector3.Angle(myTankController.GetHullDir(), hitDir);
            //print("Angle: " + angle);
            if (angle < blockAngle)
            {
                isBlock = true;
                realDamage = realDamage * blockRatio;
            }
        }

        hp -= realDamage;
        if (hp <= 0)
        {
            gameObject.SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
        }
        DmgNumManager.PlayDamageNumber((int)realDamage, theDamage.hitPos, isBlock ? DAMAGE_NUM_TYPE.BLOCK_ENEMY:DAMAGE_NUM_TYPE.BY_ENEMY);
    }

    public void DoHeal(float healNum)
    {
        hp += healNum;
        if (hp > HP_Max)
            hp = HP_Max;
    }

    public float DoHeal(float healAbsoluteNum, float healRatio)
    {
        float newHp = hp + healAbsoluteNum + HP_Max * healRatio;
        if (newHp >= HP_Max)
            newHp = HP_Max;
        float healResult = newHp - hp;
        hp = newHp;
        return healResult;
    }
}
