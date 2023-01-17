using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DamageNumber : MonoBehaviour
{
    public TextMesh theTextMesh;
    public TextMesh[] theTextMeshOutlines;

    public float duration = 1.0f;
    protected float timeLeft = 0;
    protected Animator theAnimator;
    protected int playID;

    public static Color COLOR_NORMAL = Color.yellow;
    public static Color COLOR_BLOCK = Color.HSVToRGB(0, 0, 1.0f);
    public static Color COLOR_HEAL = Color.green;
    public static Color COLOR_BY_ENEMY = Color.red;
    public static Color COLOR_BLOCK_ENEMY = Color.HSVToRGB(0.0833f, 1.0f, 1.0f);

    private void Awake()
    {
        theAnimator = GetComponent<Animator>();
        playID = Animator.StringToHash("Play");
    }

    public void Play(int num, Vector3 pos, DAMAGE_NUM_TYPE type)
    {
        Color color = Color.white;
        float textScale = 1.0f;
        switch (type)
        {
            case DAMAGE_NUM_TYPE.NORMAL:
                color = COLOR_NORMAL;
                break;
            case DAMAGE_NUM_TYPE.HEAL:
                color = COLOR_HEAL;
                break;
            case DAMAGE_NUM_TYPE.BLOCK:
                color = COLOR_BLOCK;
                textScale = 0.8f;
                break;
            case DAMAGE_NUM_TYPE.BY_ENEMY:
                color = COLOR_BY_ENEMY;
                break;
            case DAMAGE_NUM_TYPE.BLOCK_ENEMY:
                color = COLOR_BLOCK_ENEMY;
                textScale = 0.8f;
                break;
        }
        string numText = num.ToString();
        if (theTextMesh)
        {
            theTextMesh.text = numText;
            theTextMesh.color = color;
            //theTextMesh.gameObject.transform.localScale = Vector3.one * textScale;
            //theTextMesh.gameObject.SetActive(true);
        }
        foreach (TextMesh tm in theTextMeshOutlines)
        {
            tm.text = numText;
            //tm.transform.localScale = Vector3.one * textScale;
        }
        transform.position = pos;
        transform.localScale = Vector3.one * textScale;

        timeLeft = duration;
        theAnimator.SetTrigger(playID);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
            return;
        timeLeft -= Time.deltaTime;
        if (timeLeft <=0 )
        {
            DoFinish();
        }
    }

    void DoFinish()
    {
        //theTextMesh.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
