using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControl : MonoBehaviour
{

    protected PC_One thePC = null;

    protected bool isTouching = false;
    protected Vector3 touchPos;

    // Start is called before the first frame update
    void Start()
    {
//#if !TOUCH_MOVE
        if (GameSystem.IsUseVpad())
        {
            this.enabled = false;
        }
//#endif
    }

    // Update is called once per frame
    void Update()
    {


        if (isTouching)
        {
            //print("isTouching !! " + thePC);
            Vector3 mPos = Input.mousePosition;
            Vector3 mWorldMousePos = Camera.main.ScreenToWorldPoint(mPos);
            mWorldMousePos.y = 0.0f;
            if (thePC)
            {
                Vector3 dir = mWorldMousePos - touchPos;
                if (dir.magnitude > 0.1f)
                {
                    thePC.OnSetupFace(dir.normalized);
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
            touchPos.y = 0.0f;
            thePC.OnMoveToPosition(touchPos);
        }
    }

    void OnBattleTouchUp()
    {
        isTouching = false;
        thePC = BattleSystem.GetPC();
        if (thePC)
        {
            thePC.OnMoveToPositionEnd();
        }
    }

    //private void OnMouseUp()
    //{
    //    print("Mouse Up !!");
    //    isTouching = false;
    //}

    //
}
