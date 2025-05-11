using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Collectpoint : MonoBehaviour
{
    // Start is called before the first frame update
    private int nowChildCount = 0;
    public Vector3 generateRandomness = new Vector3(1.5f, 1.5f, 0);
    public float moveDuration = 0.8f;
    [SerializeField] private GameObject generatingTarget;
    [SerializeField] private ProgressBar progressBar;
    private Transform objBase;
    [SerializeField] private float spawnInterval = 4f;
    //private IEnumerator generateCorotine;
    private IEnumerator generateProgressTimer;
    void Start()
    {
        nowChildCount = transform.childCount;
        objBase = transform.parent;
    }
    [Serializable]
    public struct ColectableElement
    {
        public GameObject colectoObj;
        public int colectChance;
        public bool isOnceOnly;

        public ColectableElement(GameObject colectObj, int colectChance, bool isOnceOnly)
        {
            this.colectoObj = colectObj;//gameObjectでよくね？
            this.colectChance = colectChance;
            this.isOnceOnly = isOnceOnly;
        }
    }
    [SerializeField]
    List<ColectableElement> colectedItems = new List<ColectableElement>();//リスト内のcolectChanceの和が100になるように→別にそうじゃなくてもエラーは出ないようにしたけど、一応ね
    // Update is called once per frame
    void Update()
    {

    }
    void OnTransformChildrenChanged()
    {
        //Debug.Log(name);
        if (transform.childCount > nowChildCount)
        {
            //Debug.Log(name + "childrenの増加を検知");
            //generateCorotine = GenarateTarget();
            generateProgressTimer = GenerateProgressTimer();
            //StartCoroutine(generateCorotine);
            StartCoroutine(generateProgressTimer);
        }
        else if (transform.childCount < nowChildCount)
        {
            //Debug.Log(name + "childrenの減少を検知");
            //StopCoroutine(generateCorotine);
            StopCoroutine(generateProgressTimer);
            progressBar.progress = 0;
        }
        nowChildCount = transform.childCount;//nowChildCountは一連の判定・処理が終わってから更新
    }

    private IEnumerator GenerateProgressTimer()
    {
        float timer = 0.0f;
        while (true)
        {
            //Debug.Log(timer);
            timer++;
            if (timer + 0.01f > spawnInterval * 100)
            {//TODO?この一連の流れは関数化してもいいかも
                generatingTarget = decideGeneratingTarget(colectedItems);
                if(generatingTarget){
                var generatedTarget = Instantiate(generatingTarget, transform.position, Quaternion.identity, objBase);
                var targetPos = transform.TransformPoint(
                    new Vector3(
                        UnityEngine.Random.Range(0, generateRandomness.x) - generateRandomness.x / 2,
                        UnityEngine.Random.Range(0, generateRandomness.y) - generateRandomness.y / 2,
                        0.0f)
                        );

                targetPos.x = Mathf.Clamp(targetPos.x, -8.2f, 8.2f);
                targetPos.y = Mathf.Clamp(targetPos.y, -4.0f, 4.0f);
                //Debug.Log(targetPos);
                generatedTarget.GetComponent<CardBase>().MoveCard(targetPos, moveDuration);
                timer = 0;
                }else{
                    StopCoroutine(generateProgressTimer);
                }
            }
            yield return new WaitForSeconds(0.01f);
            progressBar.progress = timer / spawnInterval;
        }
    }
    private GameObject decideGeneratingTarget(List<ColectableElement> colectedItemsList)
    {
        float collectChanceMax = 0f;
        foreach (ColectableElement colectedItem in colectedItemsList)
        {
            collectChanceMax += colectedItem.colectChance;
        }
        var random = UnityEngine.Random.Range(0, collectChanceMax);
        if (collectChanceMax != 0)
        {
            ColectableElement generateTarget = new ColectableElement();
            foreach (ColectableElement colectedItem in colectedItemsList)
            {
                Debug.Log(collectChanceMax);
                random -= colectedItem.colectChance;
                if (random <= 0)
                {
                    generateTarget = colectedItem;
                    break;
                }
            }
            if (generateTarget.isOnceOnly)
            {
                colectedItemsList.Remove(generateTarget);
            }
            return generateTarget.colectoObj;
        }
        else { return null; }
    }
}
