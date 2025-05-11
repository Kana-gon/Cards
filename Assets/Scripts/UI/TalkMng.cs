using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TalkMng : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private SpriteRenderer talkingNPCGraphic;
    [SerializeField]
    private ChoiceMng choiceMng;

    [SerializeField]
    private TalkDataBase talkDataBase;
    [SerializeField]
    private ChoiceDataBase choiceDataBase;
    [SerializeField]
    private TextMeshPro talkText;
    private IEnumerator talking = null;
    private bool isClicked = false;
    private TalkData talkdata;//StarTtalkでリストから検索して取ってきたデータを入れる。
    void Start()
    {
        gameObject.SetActive(false);
        //デバッグ用
        //StartTalk("Guid_1");
    }

    private NPCCard talkingNPC;
    // Update is called once per frame
    void Update()
    {

    }
    void OnMouseDown()
    {
        isClicked = true;
    }
    public void StartTalk(string talkName, NPCCard _talkingNPC)
    {
        talkingNPC = _talkingNPC;
        GameObject.Find("Cards").GetComponent<CardMng>().cardsClickable = false;
        GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
        //Debug.Log(talkDataBase);
        talkdata = talkDataBase.GetTalkDataByName(talkName);
        //Debug.Log(talkdata);//犯人はお前だ！！！！！！！
        talking = Talking();
        //Debug.Log(talking);
        StartCoroutine(talking);
        //会話の開始
    }
    IEnumerator Talking()
    {
        foreach (var talk in talkdata.GetLineList())
        {
            isClicked = false;
            talkText.text = talk.lineText;
            if (talk.faseSprite)
                talkingNPCGraphic.sprite = talk.faseSprite;
            if (talk.choiceID != "")
            {
                //Debug.Log($"{talk.choiceID}の選択肢を表示します");
                GetComponent<BoxCollider2D>().enabled = false;
                choiceMng.SetChoice(talk.choiceID);//選択の諸々はChoiceMng側で処理
                yield return new WaitUntil(() => choiceMng.isClicked);
                if (choiceMng.isAllChoiced)
                {
                    Debug.Log("all choiced");
                    talkingNPC.SetTalkIndex(choiceDataBase.GetChoice(talk.choiceID).GetChoiceEndTalkThema());
                }
                StopCoroutine(talking);
                StartTalk(choiceMng.NextTalkName, talkingNPC);
            }
            //Debug.Log("テキスト変更");
            //yield return new WaitForSeconds(1);
            yield return new WaitUntil(() => isClicked);
            //TODO 一部のカードのクリック判定がTalkObjよりも優先されている。（動かすとこれよりも下になる）何故？
        }
        gameObject.SetActive(false);
        GameObject.Find("Cards").GetComponent<CardMng>().cardsClickable = true;
    }
}
