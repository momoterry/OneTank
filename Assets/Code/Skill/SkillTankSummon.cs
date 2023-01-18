using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTankSummon : SkillBase
{
    public GameObject tankRef;
    public GameObject summonFX;
    public float defaultSummonDistance = -2.0f;

    public override bool DoStart(ref SKILL_RESULT result)
    {
        if (!base.DoStart(ref result))
            return false;
        //================================

        DollManager dm = thePC.GetDollManager();
        if (tankRef == null || dm == null)
        {
            result = SKILL_RESULT.ERROR;
            return false;
        }

        Doll refDoll = tankRef.GetComponent<Doll>();
        if (!refDoll)
        {
            result = SKILL_RESULT.ERROR;
            return false;
        }

        //Transform availableSlot = dm.GetEmptySlot(refDoll.positionType);
        //if (availableSlot == null)
        if (!dm.HasEmpltySlot(refDoll.positionType))
        {
            thePC.SaySomthing("沒有空間了....");
            result = SKILL_RESULT.ERROR;
            return false;
        }


        Vector3 pos = transform.position + thePC.GetFaceDir() * defaultSummonDistance;

        if (summonFX)
            BattleSystem.GetInstance().SpawnGameplayObject(summonFX, pos, false);

        GameObject tankObj = BattleSystem.GetInstance().SpawnGameplayObject(tankRef, pos, false);
        TankOne theTank = tankObj.GetComponent<TankOne>();
        if (theTank == null)
        {
            print("Error!! There is no TankOne in tankRef !!");
            Destroy(tankObj);
        }

        if (!theTank.TryJoinThePlayer())
        {
            print("Woooooooooops.......");
        }


        //Vector3 td = (pos - transform.position).normalized;

        //if (theAnimator)
        //{
        //    theAnimator.SetFloat("CastX", td.x);
        //    theAnimator.SetFloat("CastY", td.z);

        //    theAnimator.SetTrigger("Cast");
        //}


        //================================
        result = SKILL_RESULT.SUCCESS;
        base.OnSkillSucess();
        return true; ;
    }
}
