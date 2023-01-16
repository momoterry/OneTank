using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    public TextMesh theTextMesh;

    public float duration = 1.0f;
    protected float timeLeft = 0;
    protected Animator theAnimator;
    protected int playID;

    private void Awake()
    {
        theAnimator = GetComponent<Animator>();
        playID = Animator.StringToHash("Play");
    }

    public void Play(int num, Vector3 pos, DAMAGE_NUM_TYPE type)
    {
        Color color = Color.white;
        switch (type)
        {
            case DAMAGE_NUM_TYPE.NORMAL:
                color = Color.yellow;
                break;
            case DAMAGE_NUM_TYPE.HEAL:
                color = Color.green;
                break;
            case DAMAGE_NUM_TYPE.BLOCK:
                color = Color.white;
                break;
        }
        if (theTextMesh)
        {
            theTextMesh.text = num.ToString();
            theTextMesh.color = color;
            theTextMesh.gameObject.SetActive(true);
        }
        transform.position = pos;

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
        theTextMesh.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
