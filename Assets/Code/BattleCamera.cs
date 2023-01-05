using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 targetOffset;

    protected float xEdge = 4.0f;
    protected float yEdge = 4.0f;
    protected bool isMoving = false;
    protected float moveCloseRange = 0.1f;
    protected float moveSpeed = 4.0f;

    protected Vector3 targetPos;

    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        GameObject thePlayer = BattleSystem.GetInstance().GetPlayer();
        if (thePlayer)
        {
            targetPos = thePlayer.transform.position + targetOffset;
            targetPos.y = transform.position.y;

            //TODO Smooth move
            //transform.position = targetPos;
        }

        Vector3 diff = targetPos - transform.position;
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            if (diff.sqrMagnitude <= 4.0f)
            {
                isMoving = false;
            }
        }
        else
        {
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
