using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankOne : DollAuto
{
    public GameObject hull;
    public GameObject turret;
    protected float AttackRandomRatio = 0.2f;

    protected TankController myTankController;

    protected void Awake()
    {
        myTankController = GetComponent<TankController>();
        if (myTankController == null)
        {
            print("ERROR !!!! Must have a TankController !!!!");
        }
    }

    protected override bool SearchTarget()
    {
        GameObject foundEnemy = null;
        float minDistance = Mathf.Infinity;

        Collider[] cols = Physics.OverlapSphere(transform.position, SearchRange, LayerMask.GetMask("Character"));
        foreach (Collider col in cols)
        {
            //print("I Found: "+ col.gameObject.name);
            if (col.gameObject.CompareTag("Enemy"))
            {
                //float dis = (col.gameObject.transform.position - gameObject.transform.position).magnitude;
                float dis = Vector3.Distance(col.gameObject.transform.position, transform.position);

                if (dis < minDistance)
                {
                    minDistance = dis;
                    foundEnemy = col.gameObject;
                }
            }
        }

        myTarget = foundEnemy;

        return (foundEnemy != null);
    }

    protected override void UpdateFollow()
    {
        //UpdateHullToFront();
        myTankController.SetHullToDir(myMaster.transform.forward);

        if (autoStateTime > 0.1f)
        {

            autoStateTime = 0;

            if (SearchTarget())
            {
                //Tank 直接進攻擊
                nextAutoState = AutoState.ATTACK;
            }
        }

        CheckIfRunBack();   //隨時檢查，以立刻移動
    }

    protected override void UpdateAttack()
    {
        //bool waitRotate = false;
        if (myTarget)
        {
            Vector3 toDir = myTarget.transform.position - transform.position;
            toDir.y = 0;
            toDir.Normalize();

            myTankController.SetTurretToDir(toDir);
        }

        if (myTankController.GetIsTurretReady())
            base.UpdateAttack();

        //UpdateHullToFront();
        myTankController.SetHullToDir(myMaster.transform.forward);
    }

    override protected void UpdateGoBack()
    {
        //myFace = (mySlot.position - transform.position).normalized;
        //UpdateTankMove();
        myTankController.SetMoveTarget(mySlot.position);
        if (myTarget)
        {
            Vector3 toDir = myTarget.transform.position - transform.position;
            toDir.y = 0;
            toDir.Normalize();
            myTankController.SetTurretToDir(toDir);
        }

        float dis = (mySlot.position - transform.position).magnitude;

        if (dis < PositionRangeIn)
        {
            nextAutoState = AutoState.FOLLOW;
        }
    }

    protected override void DoOneAttack()
    {
        Vector3 td = myTankController.GetTurretDirt();

        GameObject bulletObj = BattleSystem.SpawnGameObj(bulletRef, myTankController.GetMuzzlePos());

        bullet_base b = bulletObj.GetComponent<bullet_base>();
        if (b)
        {
            float damage = AttackInit * Random.Range(1.0f - AttackRandomRatio, 1.0f + AttackRandomRatio);
            b.InitValue(DAMAGE_GROUP.PLAYER, damage, td, myTarget);
        }
    }

    //private void OnGUI()
    //{
    //    Vector2 thePoint = Camera.main.WorldToScreenPoint(transform.position + Vector3.forward);
    //    thePoint.y = Camera.main.pixelHeight - thePoint.y;
    //    GUI.TextArea(new Rect(thePoint, new Vector2(100.0f, 40.0f)), currAutoState.ToString() + " " + hullAngle);
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position);
    //}
}
