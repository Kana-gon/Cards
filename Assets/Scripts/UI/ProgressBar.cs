using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    // Start is called before the first frame update
    public float progress = 0.0f;
    [SerializeField]private GameObject bar;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = bar.transform.localScale;
        scale.x=Mathf.Pow(0.01f*progress,1/2f);
        bar.transform.localScale = scale;
    }
}
