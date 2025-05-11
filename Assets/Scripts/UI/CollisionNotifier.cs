using UnityEngine;

public class CollisionNotifier : MonoBehaviour
{
    // 当たり判定のイベント
    public delegate void CollisionDetectedHandler(GameObject collidedObject, GameObject sourceObject);
    public event CollisionDetectedHandler OnCollisionDetected;

    // OnCollisionEnterでの当たり判定
    private void OnCollisionEnter(Collision collision)
    {
        if (OnCollisionDetected != null)
        {
            OnCollisionDetected.Invoke(collision.gameObject, this.gameObject);
        }
    }

    // OnTriggerEnterでのトリガー判定
    private void OnTriggerEnter(Collider other)
    {
        if (OnCollisionDetected != null)
        {
            OnCollisionDetected.Invoke(other.gameObject, this.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (OnCollisionDetected != null)
        {
            OnCollisionDetected.Invoke(null, this.gameObject);
        }
    }
}