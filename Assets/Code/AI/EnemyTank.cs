using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    public GameObject bulletRef;
    public float initDirAngle = 180.0f;
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


                    newBullet.InitValue(DAMAGE_GROUP.ENEMY, Attack, td);
                }
            }
        }
    }

    //private void OnGUI()
    //{
    //    Vector2 thePoint = Camera.main.WorldToScreenPoint(transform.position + Vector3.forward);
    //    thePoint.y = Camera.main.pixelHeight - thePoint.y;
    //    GUI.TextArea(new Rect(thePoint, new Vector2(100.0f, 40.0f)), currState.ToString());

    //}
}
