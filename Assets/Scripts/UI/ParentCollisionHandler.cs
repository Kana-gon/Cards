using UnityEngine;

public class ParentCollisionHandler : MonoBehaviour
{
    // 子オブジェクトを参照
    public GameObject childObject1;
    public GameObject childObject2;
    public GameObject childObject3;

    private ChoiceMng choiceMng;
    void Start()
    {
        choiceMng = GetComponent<ChoiceMng>();
        // 子オブジェクトにコリジョン通知スクリプトを追加
        if (childObject1 != null)
        {
            CollisionNotifier notifier1 = childObject1.AddComponent<CollisionNotifier>();
            notifier1.OnCollisionDetected += HandleCollision;
        }

        if (childObject2 != null)
        {
            CollisionNotifier notifier2 = childObject2.AddComponent<CollisionNotifier>();
            notifier2.OnCollisionDetected += HandleCollision;
        }
        if (childObject3 != null)
        {
            CollisionNotifier notifier3 = childObject3.AddComponent<CollisionNotifier>();
            notifier3.OnCollisionDetected += HandleCollision;
        }
    }

    // 子オブジェクトの当たり判定処理
    private void HandleCollision(GameObject collidedObject, GameObject sourceObject)
    {
        if (sourceObject.name == "Choice1")
        {
            choiceMng.ChooseChoice(0);
        }
        if (sourceObject.name == "Choice2")
        {
            choiceMng.ChooseChoice(1);
        }
        if (sourceObject.name == "Choice3")
        {
            choiceMng.ChooseChoice(2);
        }
        //Debug.Log(sourceObject.name + " に " + collidedObject.name + " が衝突しました");
    }
}