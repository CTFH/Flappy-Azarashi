using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public GameObject root;
    public GameObject TopBlock;
    
    public float gap;
    void Start()
    {
        //開始時に隙間の高さを変更
        ChangeHeight();
        ChangeGap();
    }

    void ChangeHeight()
    {
        //ランダムな高さを生成して設定
        float height = Random.Range(minHeight, maxHeight);
        root.transform.localPosition = new Vector3(0.0f, height, 0.0f);
    }

    void ChangeGap()
    {
        gap = Random.Range(10,13);
        TopBlock.transform.localPosition = new Vector3(0.0f, gap, 0.0f);
    }
    
    //ScrollObjectスクリプトからのメッセージを受け取って高さを変更
    void OnScrollEnd() 
    {
        ChangeHeight();
        ChangeGap();
    }
}
