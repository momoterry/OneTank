using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DAMAGE_NUM_TYPE
{
    NORMAL,
    HEAL,
    BLOCK,
}

public class DmgNumManager : MonoBehaviour
{
    [SerializeField]protected GameObject DamageNumberRef;
    [SerializeField] protected int PoolNum = 20;
    protected static DmgNumManager instance = null;

    
    protected GameObject[] DmgNumPool;
    protected int currIndex = 0;
    //protected List<DamageNumber> numPool = new List<DamageNumber>();

    public static DmgNumManager GetInstance() { return instance; }
    public static void PlayDamageNumber(int num, Vector3 position, DAMAGE_NUM_TYPE type = DAMAGE_NUM_TYPE.NORMAL)
    {
        instance.playDamageNumber(num, position, type);
    }


    private void Awake()
    {
        if (instance != null)
            print("ERROR !! 超過一份 DmgNumManager 存在 ");
        instance = this;

        DmgNumPool = new GameObject[PoolNum];
        for (int i=0; i<PoolNum; i++)
        {
            DmgNumPool[i] = Instantiate(DamageNumberRef, transform.position, Quaternion.Euler(90, 0, 0), transform);
            DmgNumPool[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected GameObject GetDamageNumber()
    {
        GameObject o = DmgNumPool[currIndex];
        currIndex++;
        if (currIndex >= PoolNum)
        {
            currIndex = 0;
        }
        if (o.activeInHierarchy)
        {
            print("ERROR!!!! DmgNumManager Pool not BIG ENOUGTH !!!!");
        }
        o.SetActive(true);
        return o;
    }


    void playDamageNumber(int num, Vector3 position, DAMAGE_NUM_TYPE type = DAMAGE_NUM_TYPE.NORMAL)
    {
        GameObject o = GetDamageNumber();
        DamageNumber dn = o.GetComponent<DamageNumber>();
        dn.Play(num, position, type);
    }

}
