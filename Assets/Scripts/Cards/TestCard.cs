using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCard : MonoBehaviour
{
    // Start is called before the first frame update
    private CardBase cardbase;
    void Start()
    {
        cardbase = GetComponent<CardBase>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject col in cardbase.GetcolList()){
            Debug.Log("name: "+col.name+" childCount: "+col.transform.childCount+" IsTouching: "+cardbase.isTouching);
        }
    }
}
