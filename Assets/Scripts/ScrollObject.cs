using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public float speed = 1.0f;
    public float startPosition;
    public float endPosition;
    void Update()
    {
        //毎フレームxポジションを少しずつ移動させる
        transform.Translate(-1 * speed * Time.deltaTime, 0, 0);
        //指定スピードでｘ軸のマイナス方向に毎フレームごとに移動させる
        //TransformコンポーネントのTranslate関数は、各成分に指定した量だけPositionを移動させることが可能。
        //time.deltaTimeをかけているので1秒間に進スピード

        //スクロールが目標ポイントまで到達したかをチェック
        if (transform.position.x <= endPosition) 
            { ScrollEnd(); }
    }

    void ScrollEnd()
    {
        //transform.position = new Vector(8f,0,0)だと隙間ができるwhileTrueでなくUpdate（１秒間に60回）だから微妙な誤差ができる
        //通り過ぎた分を加味してポジションを再設定
        float diff = transform.position.x - endPosition;//-8.0012-(-8)
        Vector3 restartPosition = transform.position;//transformPositionは３つセットじゃないとダメp178 仮の変数にコピーする
        restartPosition.x = startPosition + diff;//7.99.. (
        //startPositionはインスペクターから登録(8)
        transform.position = restartPosition;//(7.99,0,0)

        //地面に関してはtransform.position=new Vector(startPosition + diff,0,0);がVector3～の３行に該当する

        //同じゲームオブジェクトにアタッチされているコンポーネントにメッセージを送る
        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
        //SendMessage　このスクリプトがアタッチされているゲームオブジェクト全体に
        //イベントが発生したとコールしてくれる　
        //このゲームメソッド内に他のスクリプトがついていてOnScrollEnd（メソッド）があったらそれを実行できる
        //（自分のOnscrollEndも実行できる）
        //第一引数はメソッド名。メソッド名を変更したら別のメソッドを呼び出す
        //SendMessageOptionは受け取り手いないと警告がでる
    }
}
