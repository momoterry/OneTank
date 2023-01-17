using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControl : MonoBehaviour
{

    protected PC_One thePC = null;

    protected bool isTouching = false;
    protected Vector3 touchPos;
    protected Vector3 touchMousePos;
    protected Vector3 dragDir;

    // Start is called before the first frame update
    void Start()
    {
        if (GameSystem.IsUseVpad())
        {
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouching)
        {
            Vector3 mPos = Input.mousePosition;
            Vector3 mWorldMousePos = Camera.main.ScreenToWorldPoint(mPos);
            mWorldMousePos.y = 0.0f;
            if (thePC)
            {
                //dragDir = mWorldMousePos - touchPos;
                dragDir = mPos - touchMousePos;
                dragDir = new Vector3(dragDir.x, 0, dragDir.y);
                if (dragDir.magnitude > 0.5f)
                {
                    //thePC.OnSetupFace(dir.normalized);
                    thePC.OnTouchDrag(touchPos+ dragDir, dragDir);
                }
            }
        }
    }


    void OnBattleTouchDown(Vector3 point)
    {
        //print("TouchControl::OnBattleTouchDown!! " + point);
        thePC = BattleSystem.GetPC();
        if (thePC)
        {
            isTouching = true;
            touchPos = point;
            touchMousePos = Input.mousePosition;
            touchPos.y = 0.0f;
            dragDir = Vector3.zero;
            //thePC.OnMoveToPosition(touchPos);
            thePC.OnTouchDown(touchPos);
        }
    }

    void OnBattleTouchUp()
    {
        if (isTouching)
        {
            //thePC = BattleSystem.GetPC();
            if (thePC)
            {
                //thePC.OnMoveToPositionEnd();
                thePC.OnTouchFinish(touchPos + dragDir, dragDir);
            }
        }

        isTouching = false;
    }

}
