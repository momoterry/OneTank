using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public GameObject hull;
    public GameObject turret;

    public float RunSpeed = 6.0f;
    public float targetCloseRange = 0.25f;

    protected Vector3 moveTargetPos;
    protected Vector3 hullToDir = Vector3.forward;   //希望 Hull 轉向的前方
    protected Vector3 turretToDir = Vector3.forward;    //希望砲塔轉向的方向

    public float hullRotateSpeed = 180.0f;
    public float turretRotateSpeed = 240.0f;

    protected Vector3 hullBaceDir = Vector3.forward;
    protected Vector3 hullDir = Vector3.forward;
    protected float hullAngle = 0;

    protected Vector3 turretBaseDir = Vector3.forward;
    protected Vector3 turretDir = Vector3.forward;
    protected float turretAngle = 0;

    protected bool isTurrestReady = false;

    public bool GetIsTurretReady() { return isTurrestReady; }

    public void SetHullToDir(Vector3 dir) { hullToDir = dir; }
    public void SetTurretToDir(Vector3 dir) { turretToDir = dir; }
    public void SetMoveTarget(Vector3 pos) { moveTargetPos = pos; }

    private void Awake()
    {
        moveTargetPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if ((moveTargetPos - transform.position).sqrMagnitude > targetCloseRange * targetCloseRange)
        {
            UpdateTankMove();
        }
        else
        {
            UpdateHullToFront();
        }
        UpdateTurret();

        DoUpdateHullObj();
        DoUpdateTurretObj();
    }

    public void SetTankDir(Vector3 _hullDir, Vector3 _turretDir)
    {
        hullDir = hullToDir = _hullDir;
        turretDir = turretToDir = _turretDir;

        DoUpdateHullObj();
        DoUpdateTurretObj();
    }

    protected void DoUpdateHullObj()
    {
        hullAngle = Vector3.SignedAngle(hullBaceDir, hullDir, Vector3.down);
        if (hull)
            hull.transform.localRotation = Quaternion.Euler(0, 0, hullAngle);
    }

    protected void DoUpdateTurretObj()
    {
        turretAngle = Vector3.SignedAngle(turretBaseDir, turretDir, Vector3.down);
        if (turret)
            turret.transform.localRotation = Quaternion.Euler(0, 0, turretAngle);
    }

    protected void UpdateTurret()
    {
        float diffAngle = Vector3.Angle(turretDir, turretToDir);
        isTurrestReady = diffAngle < 3.0f ? true : false;

        turretDir = Vector3.RotateTowards(turretDir, turretToDir, Time.deltaTime * turretRotateSpeed * Mathf.Deg2Rad, 0);
        //turretAngle = Vector3.SignedAngle(turretBaseDir, turretDir, Vector3.down);
        //turret.transform.localRotation = Quaternion.Euler(0, 0, turretAngle);

        //print("Turrest Diff Angle :" + diffAngle);
    }

    protected void UpdateTankMove()
    {
        Vector3 toDir = moveTargetPos - transform.position;
        toDir.y = 0;
        toDir.Normalize();


        float diffAngle = Vector3.Angle(hullDir, toDir);
        if (diffAngle < 3.0f)
        {
            //對準了才移動
            transform.position = Vector3.MoveTowards(transform.position, moveTargetPos, RunSpeed * Time.deltaTime);
        }

        hullDir = Vector3.RotateTowards(hullDir, toDir, Time.deltaTime * hullRotateSpeed * Mathf.Deg2Rad, 0);
        //turretAngle = Vector3.SignedAngle(turretBaseDir, turretDir, Vector3.down);
        //turret.transform.localRotation = Quaternion.Euler(0, 0, turretAngle);
    }

    protected void UpdateHullToFront()
    {
        Vector3 front = hullToDir;
        front.y = 0;

        hullDir = Vector3.RotateTowards(hullDir, front, Time.deltaTime * hullRotateSpeed * Mathf.Deg2Rad, 0);
        //hullAngle = Vector3.SignedAngle(hullBaceDir, hullDir, Vector3.down);
        //hull.transform.localRotation = Quaternion.Euler(0, 0, hullAngle);
    }

}
