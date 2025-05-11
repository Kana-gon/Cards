using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CraftCard : MonoBehaviour
{

    // [SerializeField] 
    // public struct RecipeElement{
    //     public String materialID;
    //     public int materialNum;
    // } //* レシピだけじゃなくカード全般をこの構造体で管理する必要があるかも 今のところは無しで
    // public List<RecipeElement> recipe = new List<RecipeElement>();
    // [SerializeField]public List<List<RecipeElement>> recipeList = new List<List<RecipeElement>>();
    private List<GameObject> childList = new List<GameObject>();
    public Vector3 generateRandomness = new Vector3(1.5f, 1.5f, 0);
    public float moveDuration = 0.8f;

    private bool craftedflag = false;
    private bool childAvailableFlag = false;
    [SerializeField] RecipeDataBase recipeDataBase;
    [SerializeField] CraftPrevew craftPrevew;
    private int nowChildCount = 0;
    void Start()
    {
        nowChildCount = transform.childCount;
    }
    void OnTransformChildrenChanged()
    {
        //Debug.Log(name);
        if (transform.childCount > nowChildCount)
        {
            //Debug.Log(name + "childrenの増加を検知");
            childAvailableFlag = true;
        }
        else if (transform.childCount < nowChildCount)
        {
            //Debug.Log(name + "childrenの減少を検知");
            childAvailableFlag = false;
            craftPrevew.HidePrevew();

        }
        nowChildCount = transform.childCount;//nowChildCountは一連の判定・処理が終わってから更新
    }
    // Update is called once per frame
    void Update()
    {
        childList = this.GetComponent<CardBase>().GetChildCardList();
        childList.Remove(gameObject);

        //GetRecipeFromMaterialのテスト
        //List<RecipeData> recipeListFromMaterial = new List<RecipeData>();
        // foreach(GameObject child in childList)
        // {
        //     Debug.Log($"{child.name}でレシピ検索……");
        //     recipeListFromMaterial = recipeDataBase.GetRecipeFromMaterial(child);
        //     foreach(RecipeData recipe in recipeListFromMaterial){
        //         Debug.Log($"{child.name} は{recipe.GetRecipeName()}に使われています。");
        //     }
        // }

        if (childAvailableFlag)
        {
            //子オブジェクト郡と一致するレシピを検索
            if (recipeDataBase.GetRecipeFromAllMaterial(childList))//子オブジェクト郡と一致するレシピが存在するなら
            {
                RecipeData recipe = recipeDataBase.GetRecipeFromAllMaterial(childList);

                bool isCraftRight = false;
                foreach (GameObject craftcard in recipe.GetCraftCard())
                {
                    if (craftcard.GetComponent<CardBase>().cardID == GetComponent<CardBase>().cardID)
                    {
                        isCraftRight = true;
                    }
                }
                if (isCraftRight)//作業台が正当なら
                {
                    craftPrevew.DisplayPrevew(recipe.GetProduct().itemObject, recipe.GetProduct().num);
                    if (craftPrevew.clicked && craftedflag == false)
                    {//クラフト処理
                     //成果物の生成
                        for (int i = 0; i < recipe.GetProduct().num; i++)
                        {
                            var generatingTarget = recipe.GetProduct().itemObject;
                            var generatedTarget = Instantiate(generatingTarget, transform.position, Quaternion.identity, this.GetComponent<CardBase>().objBaseTransform);
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
                            craftedflag = true;
                        }
                        //素材の消滅
                        craftPrevew.clicked = false;
                        for (int i = childList.Count - 1; i >= 0; i--)
                        {
                            Destroy(childList[i]);
                        }
                        craftPrevew.HidePrevew();
                    }
                }
            }
            else
            {
                craftPrevew.HidePrevew();

            }
            if (craftPrevew.clicked == false)
            {
                craftedflag = false;
            }
        }
    }



}
