using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MyDictionary<TKey, TValue>
{
    [SerializeField] private TKey _key;
    [SerializeField] private TValue _value;

    public TKey Key => _key;
    public TValue Value => _value;
}
public class NPCCard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject talkObj;//TODO?シーン上で参照する必要がある。プレハブ上で設定できないので、忘れそう
    float nowChildCount;
    [Serializable]
    public struct talkElement
    {
        public string talkName;
        public bool isEndless;
    }
    [SerializeField]
    List<talkElement> talkList;
    [SerializeField]
    private List<MyDictionary<string, int>> _myDictionaries = new List<MyDictionary<string, int>>();
    public Dictionary<string,int> talkThemaNameToIndex = new Dictionary<string, int>();
    private int talkIndex = 0;
    void Start()
    {
        nowChildCount = transform.childCount;
        GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
        //talkObj = GameObject.Find("TalkMngObject");
        //Debug.Log(talkObj);
    }
    void Awake()
    {
        foreach (var myDictionary in _myDictionaries)
        {
            talkThemaNameToIndex.Add(myDictionary.Key, myDictionary.Value);
        }
        // foreach (var talkThemaName in talkThemaNameToIndex){
        //     Debug.Log($"{talkThemaName.Key}：{talkThemaName.Value}");
        // }
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log($"index = {talkIndex}");
    }
    void OnTransformChildrenChanged()
    {
        //Debug.Log(name);
        if (transform.childCount > nowChildCount)
        {
            GetComponent<GuidEvent>().PlayerTalkedToGuid();
            //Debug.Log(name + "childrenの増加を検知");
            //generateCorotine = GenarateTarget();
            if (talkList.Count > talkIndex)
            {
                Debug.Log($"{talkIndex}:talkstart");
                talkObj.GetComponent<TalkMng>().StartTalk(talkList[talkIndex].talkName, this);
                if (talkList[talkIndex].isEndless == false)
                    talkIndex++;
            }
            //StartCoroutine(generateCorotine);
        }
        else if (transform.childCount < nowChildCount)
        {
            //Debug.Log(name + "childrenの減少を検知");
            //StopCoroutine(generateCorotine);
        }
        nowChildCount = transform.childCount;//nowChildCountは一連の判定・処理が終わってから更新
    }

    public void SetTalkIndex(string talkThemaName){
        talkIndex = talkThemaNameToIndex[talkThemaName];
    }
}
