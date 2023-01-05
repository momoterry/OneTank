using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControl : MonoBehaviour
{

    protected PC_One thePC = null;

    protected bool isTouching = false;
    protected Vector3 touchPos;
    protected Vector3 touchMousePos;

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
            //Vector3 mWorldMousePos = Camera.main.ScreenToWorldPoint(mPos);
            //mWorldMousePos.y = 0.0f;
            if (thePC)
            {
                //Vector3 dir = mWorldMousePos - touchPos;
                Vector3 dir = mPos - touchMousePos;
                dir = new Vector3(dir.x, 0, dir.y);
                if (dir.magnitude > 0.5f)
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
            touchMousePos = Input.mousePosition;
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

}
