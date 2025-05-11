using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataBase/Talk")]
public class TalkDataBase : ScriptableObject
{
    [SerializeField]
    private List<TalkData> talkList = new List<TalkData>();

    public List<TalkData> GetTalkList(){
        return talkList;
    }

    public TalkData GetTalkDataByName(string searchName)
    {
        return talkList.Find(talkName => talkName.GetTalkName() == searchName);
    }
}
