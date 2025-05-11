using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFirstSetting : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private bool onStartEnableSwitch = true;
    void Start()
    {
        gameObject.SetActive(onStartEnableSwitch);
        var canvas = GameObject.Find("Canvas").transform;
        transform.SetParent(canvas);
        transform.localScale = new Vector3(2,1.5f,1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
