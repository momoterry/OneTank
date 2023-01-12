using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetOffset;

    [SerializeField] protected float focusForward = 3.0f;    //往 PC 前方多遠聚焦
    //[SerializeField] protected float xEdge = 4.0f;
    //[SerializeField] protected float yEdge = 2.0f;
    protected bool isMoving = false;
    protected float moveCloseRange = 0.1f;
    protected float moveSpeed = 4.0f;
    protected float currSpeed = 0;

    protected Vector3 targetPos;
    protected Vector3 targetFocus;

    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //GameObject thePlayer = BattleSystem.GetInstance().GetPlayer();
        PC_One thePC = BattleSystem.GetPC();
        if (!thePC || !thePC.GetDollManager())
            return;

        targetPos = thePC.GetDollManager().transform.position + targetOffset;
        targetPos.y = transform.position.y;
        Vector3 targetFace = thePC.GetFaceDir();
        targetFocus = targetPos + targetFace * focusForward;

        Vector3 diff = targetFocus - transform.position;
        float dis = diff.magnitude;

        float moveDis = 4.0f;

        if (isMoving)
        {
            //Vector3 diff = targetFocus - transform.position;
            //float dis = diff.magnitude;
            //print("dis : " + dis);

            float minDist = 0.5f;
            float minSpeed = 1.0f;
            float maxSpeed = 8.0f;
            float AccRatio = 4.0f;
            float breakDis = 2.0f;
            float breakAcc = 2.0f;

            float Acc = dis * AccRatio;
            if (dis < breakDis)
            {
                Acc = -breakAcc;
            }
            currSpeed += Time.deltaTime * Acc;
            currSpeed = Mathf.Min(Mathf.Max(currSpeed, minSpeed), maxSpeed);

            //transform.position = Vector3.MoveTowards(transform.position, targetFocus, Time.deltaTime * moveSpeed);
            transform.position = Vector3.MoveTowards(transform.position, targetFocus, Time.deltaTime * currSpeed);
            if (dis <= minDist)
            {
                isMoving = false;
            }
        }
        else
        {
            currSpeed = 0;

            //Vector3 diff = targetFocus - transform.position;
            //if ( Mathf.Abs(diff.z) > yEdge || Mathf.Abs(diff.x) > xEdge)
            if (dis >= moveDis)
            {
                isMoving = true;
            }   
        }
    }

    //private void OnGUI()
    //{
    //    Vector2 thePoint = Camera.main.WorldToScreenPoint(transform.position);
    //    thePoint.y = Camera.main.pixelHeight - thePoint.y;
    //    GUI.TextArea(new Rect(thePoint, new Vector2(100.0f, 40.0f)), isMoving.ToString() + " / " + currSpeed.ToString());
    //}
}
