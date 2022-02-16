using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    GameObject gameController;
    void Start()
    {
        //ゲーム開始時にGameControllerをFindしておく
        gameController = GameObject.FindWithTag("GameController");
    }

    //TriggerからExitしたらクリアとみなす
    void OnTriggerExit2D(Collider2D other)
    {
        gameController.SendMessage("IncreaseScore");
        //このゲームオブジェクト全体にこのイベント（increaseScore）が発生したよとコールする
        //increaseScoreがその後実行される
        //GetComponent<>().で記述してもOK
    }
}
