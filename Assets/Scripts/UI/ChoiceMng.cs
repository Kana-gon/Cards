using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChoiceMng : MonoBehaviour
{
    public bool isClicked = false;
    public bool isAllChoiced = false;

    public string NextTalkName;
    private string choiceID;
    private ChoiceData choiceData;
    [SerializeField] TextMeshPro[] choiceTextObjects = new TextMeshPro[3];
    [SerializeField] ChoiceDataBase choiceDataBase;
    // Start is called before the first frame update
    private List<string> choiceWords = new List<string>();
    private List<string> choiceNextTalkNames = new List<string>();
    private bool firstChoice = true;//*あんまり綺麗ではない
    void Start()
    {

        gameObject.SetActive(false);
    }

    //TODO TalkMngとChoiceMngの両方でChoiceDataBaseを参照してる。片方でいいと思う
    //TODO?choiceに選択済みかのステータスを持たせることにした、あんまり良くない気もする
    /// <summary>
    /// 選択肢が全て選択済ならtrueを返す
    /// </summary>
    /// <param name="searchChoiceID"></param>
    /// <returns></returns>
    public void SetChoice(string searchChoiceID)
    {
        choiceID = searchChoiceID;
        //Debug.Log($"{choiceID}の選択肢を表示します");
        choiceData = choiceDataBase.GetChoice(choiceID);
        var choices = choiceData.GetChoices();
        if (firstChoice)//選択肢を初めて見るなら、choiceDataなどを初期化
        {
            firstChoice = false;
            isAllChoiced = false;
            choiceData.SetChoicedFlag(0, false);
            choiceData.SetChoicedFlag(1, false);
            choiceData.SetChoicedFlag(2, false);
        }
        foreach (var choice in choices)
        {
            choiceWords.Add(choice.word);
            choiceNextTalkNames.Add(choice.nextTalkName);
        }
        int i = 0;
        foreach (var choiceTextObject in choiceTextObjects)
        {
            if (choiceData.GetChoicedFlag()[i])
                choiceTextObject.text = choiceWords[i] + "\n(選択済)";
            else
                choiceTextObject.text = choiceWords[i];
            i++;
        }
        i = 0;

        isClicked = false;
        gameObject.SetActive(true);
    }
    public void ChooseChoice(int choiceNum)
    {
        //Debug.Log($"{choiceNum}番を選択：");
        NextTalkName = choiceNextTalkNames[choiceNum];//TalkMngから参照される変数
        gameObject.SetActive(false);
        choiceData.SetChoicedFlag(choiceNum, true);
        if (choiceData.GetChoicedFlag().Contains(false) != true)
        {//選択肢が全て選択済なら
            Debug.Log("allChoiced:choiceMng");
            firstChoice = true;
            isAllChoiced = true;
        }
        isClicked = true;
    }
}
