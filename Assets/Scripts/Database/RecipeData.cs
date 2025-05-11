using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObject/Data/Recipe")]

public class RecipeData : ScriptableObject
{
    [Serializable]
    public struct RecipeElement
    {
        public string itemID;
        public int num;
        public RecipeElement(string itemID,int num)
        {
            this.itemID = itemID;
            this.num = num;
        }
    }//* GameObjectじゃなくてID管理に変えたほうが良いかも→変えた
    [Serializable]
    public struct RecipeProduct{
        public GameObject itemObject;
        public int num;
    }

    [SerializeField]
    List<RecipeElement> Materials = new List<RecipeElement>();
    [SerializeField]
    RecipeProduct product;
    [SerializeField]
    String recipeName;
    [SerializeField]
    List<GameObject> craftCard;
    public List<RecipeElement> GetMaterials(){
        return Materials;
    }
    public RecipeProduct GetProduct(){
        return product;
    }
    public String GetRecipeName(){
        return recipeName;
    }
    public List<GameObject> GetCraftCard(){
        return craftCard;
    }
}