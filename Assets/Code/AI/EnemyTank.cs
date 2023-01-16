using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    public GameObject bulletRef;
    public float initDirAngle = 180.0f;
    public float AttackRandomRatio = 0.2f;
    public float blockAngle = 60.0f;
    public float blockRatio = 0.5f;

    protected TankController myTankController;

    public void SetTankDirAngle(float angle)
    {
        Vector3 tankDir = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        myTankController.SetTankDir(tankDir, tankDir);
    }

    private void Awake()
    {
        myTankController = GetComponent<TankController>();
        if (myTankController == null)
        {
            print("ERROR !!!! Must have a TankController !!!!");
        }
        SetTankDirAngle(initDirAngle);
    }

    protected override void UpdateChase()
    {
        base.UpdateChase();
        myTankController.SetMoveTarget(targetPos);
        myTankController.SetHullToDir(targetPos - transform.position);
        if (targetObj)
            myTankController.SetTurretToDir(targetObj.transform.position - transform.position);
    }

    protected override void UpdateAttack()
    {
        myTankController.SetMoveTarget(transform.position); //停止移動

        if (targetObj)
            myTankController.SetTurretToDir(targetObj.transform.position - transform.position);

        if (myTankController.GetIsTurretReady())
            base.UpdateAttack();
    }


    protected override void DoOneAttack()
    {
        // TODO: 檢查是否玩家在視野
        if (bulletRef)
        {
            GameObject newObj = BattleSystem.SpawnGameObj(bulletRef, myTankController.GetMuzzlePos());
            if (newObj)
            {
                bullet_base newBullet = newObj.GetComponent<bullet_base>();
                if (newBullet)
                {
                    Vector3 td = targetObj.transform.position - newObj.transform.position;

                    td.y = 0;

                    float damage = Attack * Random.Range(1.0f - AttackRandomRatio, 1.0f + AttackRandomRatio);
                    newBullet.InitValue(DAMAGE_GROUP.ENEMY, damage, td);
                }
            }
        }
    }

    void OnDamage(Damage theDamage)
    {
        //已經死了，不要再處理以避免重覆擊殺 !!
        if (hp <= 0)
        {
            //print("你已經死了....");
            return;
        }

        //計算正面與否
        //float blockAngle = 60.0f;
        //float blockRatio = 0.5f;
        float realDamage = theDamage.damage;

        bool isBlock = false;
        Vector3 hitDir = theDamage.hitPos - transform.position;
        hitDir.y = 0;
        float angle = Vector3.Angle(myTankController.GetHullDir(), hitDir);
        //print("Angle: " + angle);
        if (angle < blockAngle)
        {
            isBlock = true;
            realDamage = realDamage * blockRatio;
        }

        hp -= realDamage;
        if (hp <= 0)
        {
            hp = 0;
            DoDeath();
        }

        //從 Idle 中醒來
        //if (currState == AI_STATE.IDLE)
        //{
        //    //TODO: 應該透過子彈來回追發射者
        //    GameObject po = BattleSystem.GetInstance().GetPlayer();
        //    SetTarget(po);
        //    nextState = AI_STATE.CHASE;
        //}


        //Damage Number
        DmgNumManager.PlayDamageNumber((int)realDamage, theDamage.hitPos, isBlock? DAMAGE_NUM_TYPE.BLOCK:DAMAGE_NUM_TYPE.NORMAL);
    }

    //private void OnGUI()
    //{
    //    Vector2 thePoint = Camera.main.WorldToScreenPoint(transform.position + Vector3.forward);
    //    thePoint.y = Camera.main.pixelHeight - thePoint.y;
    //    GUI.TextArea(new Rect(thePoint, new Vector2(100.0f, 40.0f)), currState.ToString());

    //}
}
