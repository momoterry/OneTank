using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class BattleSystem : MonoBehaviour
{
    public Battle_HUD theBattleHUD;
    public string backScene = "StartMenu";
    //public VPad theVPad;

    public MapGeneratorBase theMG;
    public Transform initPlayerPos;
    public float initPlayerDirAngle = 0;
    public GameObject playerRef;
    public bool ForceUsePlayerRef = false;

    protected GameObject thePlayer;   //TODO Player Character Spawn 較晚，但 PC 應常駐
    protected List<GameObject> enemyList = new List<GameObject>();
    protected List<GameObject> objList = new List<GameObject>();

    protected PC_One thePC;

    public int MaxLevel = 5;
    protected int currLevel = 1;

    //Input 相關
    protected int touchLayer;
    protected List<GameObject> touchDownTracing = new List<GameObject>();

    //血瓶
    public int initPotion = 3;
    public int maxPotion = 5;
    public float potionHealRatio = 0.5f;
    protected int currPotion = 3;

    protected enum BATTLE_GAME_STATE
    {
        NONE,
        INIT,
        BATTLE,
        LOCAL_WIN,
        FAIL,
    }
    protected BATTLE_GAME_STATE currState = BATTLE_GAME_STATE.NONE;
    protected BATTLE_GAME_STATE nextState = BATTLE_GAME_STATE.NONE;
    protected float stateTime = 0;

    //public int GetEnemyCount() { return enemyList.Count; }

    protected static BattleSystem instance = null;
    public static BattleSystem GetInstance() { return instance; }

    public BattleSystem() : base()
    {
    }

    protected void Awake()
    {
        GameSystem.Ensure();    //為了讓任何 Scene 都可以直接 Play !!

        if (instance != null)
            print("ERROR !! 超過一份 BattleSystem 存在: ");
        instance = this;

        touchLayer = LayerMask.GetMask("TouchPlane", "UI");
    }

    public GameObject GetPlayer() { return thePlayer; }

    public PC_One GetPlayerController() { return thePC; }
    public static PC_One GetPC() { return instance.thePC; }
    public static Dialogue GetDialogue() { return instance.theBattleHUD.theDialogue; }
    public VPad GetVPad() {return theBattleHUD.theVPad; }

    public MapGeneratorBase GetMapGenerator() { return theMG; }

    public int GetCurrLevel() { return currLevel; }

    public bool IsDuringBattle() { return currState == BATTLE_GAME_STATE.BATTLE; }

    //public void AddEnemy( GameObject enemyObj)
    //{
    //    enemyList.Add(enemyObj);
    //}

    //public void OnEnemyKilled( GameObject enemyObj)
    //{
    //    //TODO: 是否把 enemyList 改成 Enemy 而不是 GameObject ?

    //   if ( enemyList.Remove(enemyObj))
    //   {
    //        Enemy e = enemyObj.GetComponent<Enemy>();
    //        if (e)
    //        {
    //            thePC.OnKillEnemy(e);
    //        }

    //        if (GetEnemyCount()==0)
    //        {
    //            OnEnemyClear();
    //        }      
    //    }
    //    else
    //    {
    //        print("ERROR !! 不應該的重復擊殺!!: " + enemyObj);
    //    }
    //}



    public GameObject SpawnGameplayObject(GameObject objRef, Vector3 pos, bool clearByBS = true)
    {
#if XZ_PLAN
        Quaternion qm = Quaternion.Euler(90, 0, 0);
#else
        Quaternion qm = Quaternion.identity;
#endif
        if (!objRef)
            return null;

        GameObject o = Instantiate(objRef, pos, qm, null);
        if (o && clearByBS)
        {
            objList.Add(o);
        }
        return o;
    }

    public static GameObject SpawnGameObj(GameObject objRef, Vector3 pos, bool clearByBS = false)
    {
        return instance.SpawnGameplayObject(objRef, pos, clearByBS);
    }



    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (!ForceUsePlayerRef)
        {
            GameObject playerInfoToSet = GameSystem.GetInstance().GetPlayerCharacterRef();
            if (playerInfoToSet != null)
            {
                playerRef = playerInfoToSet;
            }
            else
            {
                GameSystem.GetInstance().SetPlayerCharacterRef(playerRef);
            }
        }

        GameSystem.GetInstance().SaveData();

        nextState = BATTLE_GAME_STATE.INIT;
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (nextState != currState)
        {
            OnStateExit();
            OnStateEnter();
            currState = nextState;
            stateTime = 0;
            return;
        }

        stateTime += Time.deltaTime;
        switch (currState)
        {
            case BATTLE_GAME_STATE.INIT:
                UpdateInit();
                break;
            case BATTLE_GAME_STATE.BATTLE:
                UpdateInput();
                break;
            case BATTLE_GAME_STATE.FAIL:
                UpdateFail();
                break;
        }

        UpdateBattleHUD();
    }

    //Loading 後第一次初始化關卡 (非 Fail Reset)
    protected virtual void InitBattle()
    {
        //print("InitBattle");
        SetUpLevel(1);
        //InitPlayerData();           //TODO: 這裡應該改交給 SetUpLevel 去指定
    }

    protected virtual void InitBattleStatus()
    {
        //print("InitBattleData");
        //thePC.InitStatus();
        currPotion = initPotion;
    }

    protected void UpdateInit()
    {
        //TODO: Loading 結束後等待幾個 Frame ?
        InitBattle();
        nextState = BATTLE_GAME_STATE.BATTLE;
    }


    protected virtual void UpdateBattleHUD()
    {

    }

    protected virtual void UpdateInput()
    {
        //TODO : 避免 UI 衝突的方法有點暴力, 要改
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.currentSelectedGameObject)
        {
            //print("Mouse Down On : "+ EventSystem.current.currentSelectedGameObject);

            RaycastHit hitInfo = new RaycastHit();
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, touchLayer))
            {
                //Debug.Log("Object Hit is " + hitInfo.collider.gameObject.name);
                hitInfo.collider.gameObject.SendMessage("OnBattleTouchDown", hitInfo.point);
                touchDownTracing.Add(hitInfo.collider.gameObject);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            foreach (GameObject o in touchDownTracing)
            {
                if (o)
                {
                    o.SendMessage("OnBattleTouchUp", SendMessageOptions.DontRequireReceiver);
                }
            }
            touchDownTracing.Clear();
        }
        
    }

    protected void UpdateFail()
    {

    }

    protected virtual void OnStateEnter()
    {
        switch (nextState)
        {
            case BATTLE_GAME_STATE.BATTLE:
                break;
            case BATTLE_GAME_STATE.FAIL:
                stateTime = 3.0f;   //三秒後重開局
                StopGameplayByFail();
                theBattleHUD.OnStartFailUI();
                break;
        }
    }

    protected virtual void OnStateExit()
    {

    }

    protected virtual void ClearLevel()
    {

        foreach (GameObject o in objList)
        {
            Destroy(o);
        }
        objList.Clear();
    }

    protected virtual void StopGameplayByFail()
    {
        foreach (GameObject enemyObj in enemyList)
        {
            enemyObj.SendMessage("OnGameFail", SendMessageOptions.DontRequireReceiver);
        }
    }

    protected virtual void SetUpHud()
    {
        string levelText = "LEVEL : " + currLevel;
        theBattleHUD.SetLevelText(levelText);
    }

    protected virtual void SetUpLevel( int level = 1)
    {
        //print("SetUpLevel " + level);
        currLevel = level;
        if (level < 1)
            currLevel = 1;
        else if (level > MaxLevel)
            currLevel = MaxLevel;

        //if (clearGate)
        //    clearGate.SetActive(false);
        theMG.BuildAll(currLevel);

        // ================ 戰鬥資料初始化 ====================
        if (level == 1)
        {
            InitBattleStatus();
            if (thePlayer == null)
            {
#if XZ_PLAN
                Quaternion rm = Quaternion.Euler(90, 0, 0);
#else
                Quaternion rm = Quaternion.identity;
#endif

                thePlayer = Instantiate(playerRef, initPlayerPos.position, rm, null);

                thePC = thePlayer.GetComponent<PC_One>();
                thePC.initFaceDirAngle = initPlayerDirAngle;
                thePC.SetupFaceDirByAngle(initPlayerDirAngle);  //確保 First Frame 轉向正確
                //thePC.InitStatus(); 會在 PC 的 Start 被呼叫
            }    
            else
            {
                thePC.DoTeleport(initPlayerPos.position, initPlayerDirAngle);
                thePC.InitStatus();
            }
        }
        else
        {
            // 關卡升級
            thePC.DoTeleport(initPlayerPos.position, initPlayerDirAngle);
        }

        SetUpHud();
    }

    protected void ResetLevel( int level = 1 )
    {
        print("Reset Level!!");

        ClearLevel();


        SetUpLevel(level);
    }

    public void OnPlayerKilled()
    {
        print("玩家死亡........");
        nextState = BATTLE_GAME_STATE.FAIL;
    }



    public void OnAddLevelDifficulty(int addLevel = 1)
    {
        currLevel += 1;
        SetUpHud();
    }

    public void OnClearGateEnter()
    {
        //增加一個難度
        ResetLevel(currLevel + 1);
    }


    //整個重開，角色也回到等級一狀態，從 Fail UI 呼叫
    public void OnLevelRestart()
    {
        if (currState != BATTLE_GAME_STATE.FAIL)
        {
            print("ERROR !!!!! OnLevelRestart() called but not in fail state !!");
        }


        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);

        //ResetLevel();
        //nextState = BATTLE_GAME_STATE.BATTLE;
    }

    public void OnBackToStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void OnGotoScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnBackPrevScene()
    {
        SceneManager.LoadScene(backScene);
    }

    //private void OnGUI()
    //{
    //    GUI.TextArea(new Rect(new Vector2(10.0f, 10.0f), new Vector2(100.0f, 40.0f)), currState.ToString());
    //}
}
