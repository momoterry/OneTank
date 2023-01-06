using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetOffset;

    protected float focusForward = 3.0f;    //往 PC 前方多遠聚焦
    protected float xEdge = 4.0f;
    protected float yEdge = 2.0f;
    protected bool isMoving = false;
    protected float moveCloseRange = 0.1f;
    protected float moveSpeed = 4.0f;

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
        if (thePC)
        {
            targetPos = thePC.transform.position + targetOffset;
            targetPos.y = transform.position.y;
            Vector3 targetFace = thePC.GetFaceDir();
            targetFocus = targetPos + targetFace * focusForward;
            targetPos.y = transform.position.y;

            //TODO Smooth move
            //transform.position = targetPos;
        }


        if (isMoving)
        {
            Vector3 diff = targetFocus - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetFocus, Time.deltaTime * moveSpeed);
            if (diff.sqrMagnitude <= 4.0f)
            {
                isMoving = false;
            }
        }
        else
        {
            Vector3 diff = targetPos - transform.position;
            if ( Mathf.Abs(diff.z) > yEdge || Mathf.Abs(diff.x) > xEdge)
            {
                isMoving = true;
            }   
        }
    }

    //private void OnGUI()
    //{
    //    Vector2 thePoint = Camera.main.WorldToScreenPoint(transform.position);
    //    thePoint.y = Camera.main.pixelHeight - thePoint.y;
    //    GUI.TextArea(new Rect(thePoint, new Vector2(100.0f, 40.0f)), (targetPos - transform.position).ToString());
    //}
}
