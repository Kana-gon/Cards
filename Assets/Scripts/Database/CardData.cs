using System;
using System.Collections.Generic;
using UnityEngine;
namespace App.BaseSystem.DataStores.ScriptableObjects.Card{
    [CreateAssetMenu(menuName ="ScriptableObject/Data/Card")]
    public class CardData:ScriptableObject{
        
        [SerializeField]
        public string cardName;
        [SerializeField]
        public string cardID;
        [SerializeField]
        public string cardType;

        [SerializeField,TextArea]
        public string cardText;
        [SerializeField]
        public Sprite cardIllast;

        public string GetCardName(){
            return cardName;
        }
        public string GetCardID(){
            return cardID;
        }
        public string GetCardType(){
            return cardType;
        }
        public List<string> childCardTypeBlackList;
    }
}