using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DM_Tank : DollManager
{
    public int MaxSlot = 40;

    public int FrontWidth = 4;
    public float slotWidth = 1.5f;
    public float slotDepth = 1.5f;

    public GameObject hintRef;

    protected float allShift = 0.0f;

    protected List<Doll> frontList = new List<Doll>();
    protected bool needRebuild = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        slotNum = MaxSlot;
        DollSlots = new Transform[slotNum];
        for (int i = 0; i < slotNum; i++)
        {
            GameObject o = new GameObject("DynaSlot_" + i);
            //GameObject o = Instantiate(hintRef, transform.position, transform.rotation, transform);
            GameObject ho = BattleSystem.SpawnGameObj(hintRef, transform.position);
            ho.transform.parent = o.transform;
            o.transform.position = transform.position;
            o.transform.parent = transform;
            DollSlots[i] = o.transform;
            o.SetActive(false); 
        }
        dolls = new Doll[slotNum];
    }


    // Update is called once per frame
    void Update()
    {
        if (needRebuild)
        {
            RebuildFormation();
            needRebuild = false;
        }
    }

    public override void OnStartHint() 
    { 
        foreach (Doll d in frontList)
        {
            GameObject ho = d.GetSlot().gameObject;
            ho.SetActive(true);
            SPAnimator sa = ho.GetComponentInChildren<SPAnimator>();
            sa.Restart();
        }
    }
    public override void OnFinishHint()
    {
        foreach (Doll d in frontList)
        {
            GameObject ho = d.GetSlot().gameObject;
            ho.SetActive(false);
        }
    }

    public override void OnUpdateHint(Vector3 faceDir) { }

    protected void RebuildFrontSlots()
    {
        int frontNum = frontList.Count;

        if (frontNum <= 0)
            return;

        int nLine = ((frontNum - 1) / FrontWidth) + 1;
        int lastLineCount = (frontNum - 1) % FrontWidth + 1;

        float fPos = ((float)(nLine - 1) * 0.5f) + allShift;  //前方起始
        //float slotDepth = 2.0f;
        fPos += slotDepth * (float)(nLine - 1);

        for (int l = 0; l < nLine; l++)
        {
            int num = FrontWidth;
            if (l == nLine - 1)
                num = lastLineCount;
            //print("Line: " + l + " Count: " + num);

            //float slotWidth = Mathf.Max(1.0f, 1.5f - ((float)(num - 1) * 0.25f));
            float width = (float)(num - 1) * slotWidth;
            float lPos = width * -0.5f;
            for (int i = l * FrontWidth; i < l * FrontWidth + num; i++)
            {
                //print("Prepare ..." + i);
                frontList[i].GetSlot().localPosition = new Vector3(lPos, 0, fPos);
                lPos += slotWidth;
            }
            fPos -= slotDepth;
        }
    }


    protected void RebuildFormation()
    {
        frontList.Clear();

        for (int i = 0; i < slotNum; i++)
        {
            if (dolls[i] && dolls[i].gameObject.activeInHierarchy)
            {
                switch (dolls[i].positionType)
                {
                    case DOLL_POSITION_TYPE.FRONT:
                        frontList.Add(dolls[i]);
                        break;
                    case DOLL_POSITION_TYPE.MIDDLE:
                        frontList.Add(dolls[i]);
                        break;
                    case DOLL_POSITION_TYPE.BACK:
                        frontList.Add(dolls[i]);
                        break;
                }
            }
        }
        if (frontList.Count > 0)
        {
            RebuildFrontSlots();
        }
    }


    public override Transform AddOneDoll(Doll doll, DOLL_POSITION_TYPE positionType = DOLL_POSITION_TYPE.FRONT)
    {
        //Transform result = base.AddOneDoll(doll, positionType);
        Transform result = null;
        for (int i = 0; i < slotNum; i++)
        {
            if (dolls[i] == null && DollSlots[i] != null)
            {
                dolls[i] = doll;
                result = DollSlots[i];
                break;
            }
        }

        if (result)
        {
            //RebuilFormation();
            //result.gameObject.SetActive(true);
            needRebuild = true;
        }

        return result;
    }

    public override void OnDollTempDeath(Doll doll)
    {
        DoRemoveDollFronList(doll);
    }

    public override void OnDollRevive(Doll doll)
    {
        needRebuild = true;
        doll.GetSlot().gameObject.SetActive(true);
    }

    public override void OnDollDestroy(Doll doll)
    {
        DoRemoveDollFronList(doll);
    }

    protected void DoRemoveDollFronList(Doll doll)
    {
        switch (doll.positionType)
        {
            case DOLL_POSITION_TYPE.FRONT:
            case DOLL_POSITION_TYPE.MIDDLE:
            case DOLL_POSITION_TYPE.BACK:
                frontList.Remove(doll);
                doll.GetSlot().gameObject.SetActive(false);
                RebuildFrontSlots();
                break;
            //case DOLL_POSITION_TYPE.MIDDLE:
            //    middleList.Remove(doll);
            //    RebuildMiddleSlots();
            //    break;
            //case DOLL_POSITION_TYPE.BACK:
            //    backList.Remove(doll);
            //    RebuildBackSlots();
            //    break;
        }
    }
}
