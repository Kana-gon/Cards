using System.Collections;
using System.Collections.Generic;
using App.BaseSystem.DataStores.ScriptableObjects.Card;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataBase/Card")]
public class CardDataBase : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    List<CardData> cardList = new List<CardData>();

    public List<CardData> GetCardList(){
        return cardList;
    }
    public CardData GetCardDataByName(string searchName)
    {
        return cardList.Find(cardName => cardName.GetCardName() == searchName);
    }

    public CardData GetCardData(string searchID){
        return cardList.Find(cardID => cardID.GetCardID() == searchID);
    }
}
