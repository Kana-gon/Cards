using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidEvent : MonoBehaviour
{
    private Dictionary<string,bool> switchs = new Dictionary<string, bool>();
    private GameObject grandChildCard;
    void Awake()
    {
        switchs.Add("giveSuits",false);
    }
    public void PlayerTalkedToGuid()
    {
        grandChildCard = GetComponent<CardBase>().childCard.GetComponent<CardBase>().childCard;
        if (grandChildCard!=null){//子カードがあれば
            //Debug.Log($"name:{grandChildCard.name},ID:{grandChildCard.GetComponent<CardBase>().cardID}");
            if(grandChildCard.GetComponent<CardBase>().cardID == "item_guid'sSuit"&&switchs["giveSuits"] == false){
                GetComponent<NPCCard>().SetTalkIndex("giveSuits");
                switchs["giveSuits"] = true;
            }
        }
    }
}
