using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Data/Choice")]
public class ChoiceData : ScriptableObject{
    [SerializeField]private string choiceName;
    [Serializable]public struct ChoiceElement{
        [SerializeField] public string word;
        [SerializeField] public string nextTalkName;
    }
    private bool[] choiced = { false, false, false };
    [SerializeField] private List<ChoiceElement> choices = new List<ChoiceElement>(3);//*ひとまず選択肢の個数（=Listの要素数）は3個で固定
    [SerializeField] private string choiceEndTalkThema;//NPCCard側で選択肢の会話をループオンにしていたら、全ての選択肢を見終わった後この会話に飛ぶ　ループオフなら普通に次の会話へ
    public string GetChoiceName()
    {
        return choiceName;
    }
    public List<ChoiceElement> GetChoices(){
        return choices;
    }
    public void SetChoicedFlag(int choiceNum, bool value)
    {
        choiced[choiceNum] = value;
    }
    public bool[] GetChoicedFlag()
    {
        return choiced;
    }
    public string GetChoiceEndTalkThema()
    {
        return choiceEndTalkThema;
    }
}
