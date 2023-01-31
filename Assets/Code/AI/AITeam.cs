using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITeam : MonoBehaviour
{
    public Enemy[] members;
    // Start is called before the first frame update

    protected enum PHASE
    {
        NONE,
        SLEEP,
        AWAKE,
    }

    protected PHASE currPhase = PHASE.NONE;
    protected PHASE nextPhase = PHASE.NONE;

    public void OnTeamAlert( Enemy alertBy, GameObject alertTarget = null)
    {
        if (currPhase == PHASE.SLEEP)
        {
            if (alertTarget == null)
            {
                alertTarget = BattleSystem.GetPC().gameObject;
            }

            foreach (Enemy e in members)
            {
                if (e != alertBy)
                {
                    e.OnAlert(alertTarget);
                }
            }
        }
    }

    void Awake()
    {
        foreach (Enemy enemy in members)
        {
            enemy.SetAITeam(this);
        }
    }

    void Start()
    {
        nextPhase = PHASE.SLEEP;
    }

    // Update is called once per frame
    void Update()
    {
        currPhase = nextPhase;
    }
}
