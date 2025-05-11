using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataBase/Choice")]
public class ChoiceDataBase : ScriptableObject
{
    [SerializeField]
    List<ChoiceData> choiceList = new List<ChoiceData>();
    public List<ChoiceData> GetChoiceList(){
        return choiceList;
    }
    public ChoiceData GetChoice(string searchName){
        return choiceList.Find(choiceName => choiceName.GetChoiceName() == searchName);
    }
}
