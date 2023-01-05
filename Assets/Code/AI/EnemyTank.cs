using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTank : Enemy
{
    public GameObject bulletRef;

    protected TankController myTankController;

    private void Awake()
    {
        myTankController = GetComponent<TankController>();
        if (myTankController == null)
        {
            print("ERROR !!!! Must have a TankController !!!!");
        }
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
            Vector3 shootPoint = gameObject.transform.position + faceDir * 0.5f;

            GameObject newObj = Instantiate(bulletRef, shootPoint, Quaternion.Euler(90, 0, 0), null);

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
