using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DataBase/Recipe")]
public class RecipeDataBase : ScriptableObject
{
    [SerializeField]
    List<RecipeData> recipeList = new List<RecipeData>();
    public List<RecipeData> GetRecipeList(){
        return recipeList;
    }
    public RecipeData GetRecipe(string searchName){
        return recipeList.Find(recipeName => recipeName.GetRecipeName() == searchName);
    }
    public List<RecipeData> GetRecipeFromMaterial(GameObject searchObj){//素材が一つでも合うRecipeを探して返却
        List<RecipeData> matchedRecipeList = new List<RecipeData>();
        List<RecipeData.RecipeElement> materialList = new List<RecipeData.RecipeElement>();
        foreach (RecipeData recipe in recipeList){
            //Debug.Log($"{recipe.GetRecipeName()}に使われているか？");
            materialList = recipe.GetMaterials();
            foreach(RecipeData.RecipeElement material in materialList){
                if(material.itemID == searchObj.GetComponent<CardBase>().cardID)
                {
                    //Debug.Log($"{searchObj.name}が使われています");
                    matchedRecipeList.Add(recipe);
                }
            }
        }
        return matchedRecipeList;
    }

    public RecipeData GetRecipeFromAllMaterial(List<GameObject> searchMaterials)
    {
        //* 棒5+ロープ1のレシピに、棒4+ロープ1+棒1みたいな乗せ方すると反応しない そういうレシピにするか？　一旦はこのままで　数まで完全一致でレシピってことにしよう
        //サーチ側のソート
        searchMaterials.Sort((a, b) => a.name.CompareTo(b.name));
        List<RecipeData.RecipeElement> materialSearch = new List<RecipeData.RecipeElement>();

        foreach (GameObject searchMaterial in searchMaterials){
            materialSearch.Add(new RecipeData.RecipeElement(searchMaterial.GetComponent<CardBase>().cardID,searchMaterial.GetComponent<CardBase>().num));
        }
        // foreach (RecipeData.RecipeElement material in materialSearch)
        // {
        //     Debug.Log($"{material.itemID}×{material.num}");
        // }
        foreach (RecipeData recipe in recipeList)
        {
            //レシピ側のソート
            var materialList = recipe.GetMaterials();
            materialList.Sort((a,b) => a.itemID.CompareTo(b.itemID));

            //Debug.Log(materialSearch.SequenceEqual(materialList));
            if(materialSearch.SequenceEqual(materialList)){
                //Debug.Log($"{recipe.GetRecipeName()}が作成可能です");
                return recipe;
            }
            // Debug.Log($"{recipe.GetRecipeName()}:");
            // foreach(RecipeData.RecipeElement material in materialList){
            //     Debug.Log($"{material.itemID}×{material.num}");
            // }


        }
        return null;
    }
}
