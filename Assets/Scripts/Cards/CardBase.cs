using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;
using DG.Tweening;
using TMPro;
using System;
using App.BaseSystem.DataStores.ScriptableObjects.Card;

//* プレハブのプレビューがくっそ小さい →カードイラストを付けると解決する
//TODO 親子関係が外れる時・付く時の音をつける
//*    「カード名・テキスト・イラスト」も「子としてついてるカード」も同じ子オブジェクトなのがクソめんどい→GetChildCardの活用でなんとかなりそう
public class CardBase : MonoBehaviour
{
    public List<string> childCardTypeBlackList = new List<string>();//これに重ねられないカードタイプのリスト
    public string cardType = null;//カードタイプの判別に使う
    public string cardID = null;//カードの種類の判別に使う
    public int num = 1;//カードのスタックされている数
    public GameObject childCard = null;
    public Transform objBaseTransform;//背景(他カードについていないときの親) カード種ごとのコードからもアクセスするためpublic(都度読み込んだっていいけど)

    public bool stackable = false;//スタック可能か？
    public bool isTouching = false;
    public bool isGrapped = false;
    public bool isFollowing = false;

    public Vector3 displacement = new Vector3(0.3f, -0.3f, 0);
    private List<GameObject> colList = new List<GameObject>();
    private Vector3 screenPoint;
    private Vector3 offset;

    [SerializeField] private int grappingSortingOrder = 100;//つかんだ時にどれだけsortingorderを上げるか　別レイヤーに持っていっても良いかも
    [SerializeField] private Renderer shade;
    [SerializeField] private Renderer frame;
    [SerializeField] private GameObject numObj;
    [SerializeField] private TextMeshPro numStr;
    [SerializeField] private GameObject selfPrefab;

    [SerializeField] private bool useNumSwitch = false;

    [SerializeField] private CardData carddata;
    [SerializeField] private SpriteRenderer cardIllast;
    [SerializeField] private TextMeshPro cardName;
    [SerializeField] private GameObject cardTextWindow;
    //[SerializeField] private TextMeshPro cardText;
    private bool canStackFlag = false;

    private SortingGroup sortinggroup;

    void Start()
    {
        if (carddata)
        {
            cardIllast.sprite = carddata.cardIllast;
            cardName.text = carddata.cardName;
            cardTextWindow.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = carddata.cardText;
            childCardTypeBlackList = carddata.childCardTypeBlackList;
            cardType = carddata.cardType;
            cardID = carddata.cardID;
        }
        sortinggroup = GetComponent<SortingGroup>();
        objBaseTransform = transform.parent;
        shade.enabled = false;
        numObj.SetActive(useNumSwitch);
        cardTextWindow.SetActive(false);
    }
    private void Update()
    {
        if (isFollowing)
        {
            //Debug.Log("following!");
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
            currentPosition.z = 0;
            currentPosition.x = Mathf.Clamp(currentPosition.x, -8.2f, 8.2f);
            currentPosition.y = Mathf.Clamp(currentPosition.y, -4.0f, 4.0f);
            transform.position = currentPosition;
            //Debug.Log(shade.enabled);
            shade.enabled = true;
            if (Input.GetMouseButtonUp(1))
            {
                isFollowing = false;
                OnReleased();
            }
        }
        //cardTextWindow.SetActive(false);
    }
    //コリダーの処理
    private void OnTriggerEnter2D(Collider2D other)
    {
        colList.Add(other.gameObject);
        isTouching = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        foreach (GameObject col in colList)
        {
            if (isGrapped && col.transform.childCount == 6 && col.GetComponent<CardBase>().isGrapped == false && transform.GetComponentsInChildren<Transform>().Contains(col.transform) == false)
            {
                if (col.GetComponent<Renderer>() && col.GetComponent<CardBase>().childCardTypeBlackList.Contains(cardType) == false)//rendererが設定されてないとエラー落ちする
                    col.GetComponent<Renderer>().material.color = Color.gray;
            }
        }
    }
    //他オブジェクトとの重なりexit
    private void OnTriggerExit2D(Collider2D other)
    {
        colList.Remove(other.gameObject);
        if (other.GetComponent<Renderer>())
            other.GetComponent<Renderer>().material.color = Color.white;
        if (colList == null)
        {
            isTouching = false;
        }
    }

    //ここから下はマウス処理
    private void OnMouseDown()
    {
        OnGrapped();
    }
    // ドラッグ中、このオブジェクトを追従させる
    private void OnMouseOver()
    {
        //スタック時、右クリックされたら1枚取り出す　これのチュートリアルはどうする？
        if (objBaseTransform.GetComponent<CardMng>().cardsClickable)
        {
            if (Input.GetMouseButtonDown(1) && stackable && num > 1)
            {
                num -= 1;
                GameObject clone = Instantiate(selfPrefab, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation, objBaseTransform);
                if (clone.transform.childCount > 6)
                {
                    Destroy(clone.transform.GetChild(6).gameObject);
                }
                clone.transform.localScale = new Vector3(1.5f, 2f, 1);
                clone.GetComponent<CardBase>().num = 1;
                clone.GetComponent<CardBase>().Start();
                clone.GetComponent<CardBase>().OnGrapped();
                clone.GetComponent<CardBase>().isFollowing = true;

                ReloadNum();
                clone.GetComponent<CardBase>().ReloadNum();
            }
            //中クリックでデバッグログを出力
            if (Input.GetMouseButtonDown(2))
            {
                //Debug.Log("中クリックど");
                Debug.Log($"name:{name} sortingGroup.sortingOrder:{GetComponent<SortingGroup>().sortingOrder}");
            }
            if (Input.GetMouseButton(0) == false && Input.GetMouseButton(1) == false && Input.GetMouseButton(2) == false)
            {//どのボタンも押されていなかったら
                Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint);//+ this.offset;
                currentPosition.z = 0;
                currentPosition.x += 2.3f;
                currentPosition.y += 1.3f;
                currentPosition.x = Mathf.Clamp(currentPosition.x, -8.2f, 7.0f);
                currentPosition.y = Mathf.Clamp(currentPosition.y, -4.0f, 3.8f);
                cardTextWindow.SetActive(true);
                cardTextWindow.transform.position = currentPosition;
                //Debug.Log(shade.enabled);
            }
        }
    }
    //マウスで掴んだ時の処理
    void OnMouseExit()
    {
        cardTextWindow.SetActive(false);
    }
    public void OnGrapped()
    {
        if (objBaseTransform.GetComponent<CardMng>().cardsClickable)
        {
            transform.DOKill(false);
            cardTextWindow.SetActive(false);
            //Debug.Log("Grapped!");
            canStackFlag = true;
            foreach (Transform child in GetComponentsInChildren<Transform>())//自身と子カード群の影を表示する
            {
                if (child.CompareTag("CardShade"))
                {
                    var shade = child;
                    shade.gameObject.GetComponent<Renderer>().enabled = true;
                }
            }
            isGrapped = true;

            shade.enabled = true;
            GetComponent<Renderer>().material.color = Color.white;
            sortinggroup.sortingOrder += grappingSortingOrder;
            frame.sortingOrder = 0;
            screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z)) + new Vector3(0.1f, 0.1f, 0.1f);
            transform.SetParent(objBaseTransform);
        }
        //this.offset = new Vector3(0.1f,0.1f,0.1f);}
    }
    private void OnMouseDrag()
    {
        if (objBaseTransform.GetComponent<CardMng>().cardsClickable)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
            currentPosition.x = Mathf.Clamp(currentPosition.x, -8.2f, 8.2f);
            currentPosition.y = Mathf.Clamp(currentPosition.y, -4.0f, 4.0f);
            transform.position = currentPosition;
        }
    }
    private void OnMouseUp()
    {
        OnReleased();
    }
    //マウスを離した時（他カードの上に乗せる（子にする）などの処理はここ）
    //transform.childCount == 6→カードを子に持たない、という意味　GetChildCard()使ったほうがいいか？
    //TODO? sortingOrder周りの処理は未完成(A->B->CのBを外してベースに置くと、B:0,C:12 とかになる　今のところ支障はないので一旦これで)
    private void OnReleased()
    {
        if (objBaseTransform.GetComponent<CardMng>().cardsClickable)
        {
            //Debug.Log("Released!");
            Transform childDestroyTarget = null;
            foreach (GameObject col in colList)
            {

                if (isTouching && col.transform.childCount == 6 && col.GetComponent<CardBase>().childCardTypeBlackList.Contains(cardType) == false)
                {
                    if (stackable && cardID == col.GetComponent<CardBase>().cardID && canStackFlag && GetChildCardList().Contains(col) == false) //スタック可アイテム&スタック先と触れてる&canstackflagは二重スタック防止用&スタック先がツリーにいない
                    {//もしスタックするべき状況なら
                     //親子関係の処理
                        if (col.transform.parent != objBaseTransform)
                        {
                            //親側(現状無し)
                            // col.transform.SetParent(objBaseTransform);
                            // var target = new Vector3(col.transform.position.x + 0.2f, col.transform.position.y + 0.2f, 0);
                            // col.GetComponent<CardBase>().MoveCard(target, 0.3f);
                        }
                        if (transform.childCount > 6)
                        {
                            //子側
                            var childTransform = transform.GetChild(6).transform;
                            childTransform.SetParent(col.transform);
                            childTransform.transform.localPosition = displacement;
                            // var target = new Vector3(col.transform.position.x + 0.2f, col.transform.position.y + 0.2f, 0);
                            // col.GetComponent<CardBase>().MoveCard(target, 0.3f);

                        }
                        // これを消す　親のスタック数を1増やす
                        col.GetComponent<CardBase>().num += this.num;
                        col.GetComponent<CardBase>().ReloadNum();
                        childDestroyTarget = transform;
                        canStackFlag = false;
                    }
                    transform.SetParent(col.transform);
                    col.GetComponent<Renderer>().material.color = Color.white;
                    if (transform.parent != objBaseTransform.transform)
                    {
                        transform.localPosition = displacement;
                    }
                }
            }
            if (childDestroyTarget)
            {
                Destroy(childDestroyTarget.gameObject);//スタック処理の一部
            }
            //thisCanvas.sortingOrder -= grappingSortingOrder;
            sortinggroup.sortingOrder -= grappingSortingOrder;
            var childList = GetChildCardList();
            foreach (GameObject child in childList)
            {
                child.GetComponent<CardBase>().ReloadSortingOrder();
            }
            // 影と枠だけsortinggroupから除外する→一旦諦め
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
            frame.sortingOrder = 0;


            transform.position -= new Vector3(0.1f, 0.1f, 0);
            isGrapped = false;
            // transform.Find("card_shade").GetComponent<Renderer>().enabled = false;
            foreach (Transform child in GetComponentsInChildren<Transform>())
            {
                if (child.CompareTag("CardShade"))
                {
                    var shade = child;
                    shade.gameObject.GetComponent<Renderer>().enabled = false;
                }
            }
            GameObject[] shades = GameObject.FindGameObjectsWithTag("CardShade");
            foreach (GameObject shade in shades)
            {
                shade.GetComponent<Renderer>().enabled = false;
            }
            shade.enabled = false;
        }
    }
    private void OnTransformChildrenChanged()
    {
        childCard = GetChildCard();
        // if(childCard != null)
        //     Debug.Log(childCard.name);
    }

    //public関数
    public void MoveCard(Vector3 target, float duration)
    {
        transform.DOMove(target, duration).SetEase(Ease.OutQuart);
    }
    public List<GameObject> GetcolList()
    {
        return colList;
    }
    //TODO? ↓の関数使ってコードを読みやすくする
    public GameObject GetChildCard()
    {
        if (transform.childCount > 6)
            return transform.GetChild(6).gameObject;
        else
            return null;
    }
    public List<GameObject> GetChildCardList()
    {
        List<GameObject> childCardList = new List<GameObject>();
        if (transform.childCount > 6)
        {
            childCardList.AddRange(transform.GetChild(6).GetComponent<CardBase>().GetChildCardList());
            childCardList.Add(this.gameObject);
        }
        else
        {
            childCardList.Add(this.gameObject);
        }
        return childCardList;
    }
    public void ReloadNum()
    {
        numStr.text = num.ToString();
    }
    //*そのうちreloadUIになるかも
    public void ReloadSortingOrder()
    {
        if (transform.parent == objBaseTransform) GetComponent<SortingGroup>().sortingOrder = 0;
        else { GetComponent<SortingGroup>().sortingOrder = transform.parent.GetComponent<SortingGroup>().sortingOrder + 6; }
    }
}
