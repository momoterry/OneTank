using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchControl : MonoBehaviour
{
    protected bool isOriginalCenter = true;
    protected PC_One thePC = null;

    protected bool isTouching = false;
    protected Vector3 touchPos;
    protected Vector3 touchMousePos;
    protected Vector3 dragVec;

    [SerializeField]protected GameObject touchHintRef;
    //protected GameObject theTouchHintObj;
    protected TouchControlHint theHint;

    private void Awake()
    {
        if (touchHintRef)
        {
            GameObject o = Instantiate(touchHintRef);
            theHint = o.GetComponent<TouchControlHint>();
            o.SetActive(false);
        }
    }
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
            float dragMinL = 1.0f;

            if (isOriginalCenter)
            {
                dragVec = mPos - touchMousePos;
                float vecRatio = (Camera.main.orthographicSize * 2.0f) / (float)Camera.main.scaledPixelHeight;
                dragVec = new Vector3(dragVec.x*vecRatio, 0, dragVec.y * vecRatio);
                //dragMinL *= (float)Camera.main.scaledPixelHeight / (Camera.main.orthographicSize * 2.0f);
            }
            else
            {
                Vector3 mWorldMousePos = Camera.main.ScreenToWorldPoint(mPos);
                mWorldMousePos.y = 0.0f;
                dragVec = mWorldMousePos - touchPos;
            }

            if (dragVec.magnitude > dragMinL)
            {
                if (thePC)
                {
                    //thePC.OnSetupFace(dir.normalized);
                    thePC.OnTouchDrag(touchPos + dragVec, dragVec);
                }

                if (theHint)
                {
                    theHint.SetFormationOnOff(true);
                    theHint.SetFormationVec(dragVec.normalized);
                }
            }
            else
            {
                dragVec = Vector3.zero;
                if (theHint)
                {
                    theHint.SetFormationOnOff(false);
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
            dragVec = Vector3.zero;
            //thePC.OnMoveToPosition(touchPos);
            thePC.OnTouchDown(touchPos);
            if (theHint)
            {
                theHint.gameObject.SetActive(true);
                theHint.transform.position = touchPos;
                theHint.SetFormationOnOff(false);
            }
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
                thePC.OnTouchFinish(touchPos, dragVec);
            }
            if (theHint)
            {
                theHint.gameObject.SetActive(false);
            }
        }

        isTouching = false;
    }

}
