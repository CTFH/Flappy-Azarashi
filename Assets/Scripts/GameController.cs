using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //シーンの切り替えを行うため、インポート
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //ゲームステート
    enum State　//ステートやタイプなど、連続した要素をわかりやすい名前で定義し識別できる仕組み
    {   
        //状態が2つの時はboolで管理
        //３つ以上は列挙定数enum(erator)(クラスみたいな感じ)
        Ready,　//index0
        Play, //index1
        GameOver
    }

    State state;　//(nullでなくReadyが入っている。空の状態はない)フィールドは初期値がある。boolはfalse
    //stateにはReady、Play、GameOverの3つしか入らない

    int score;

    public AzarashiController azarashi;
    public GameObject blocks;

    public Text scoreText;
    public Text stateText;
    void Start()
    {
        //開始と同時にReadyステートに移行
        Ready();
    }

    private void LateUpdate()　//他のアップデートが終わった後に行われる（1秒間に60回）
    {
        //ゲームのステートごとにイベントを監視
        switch (state)
        {
            case State.Ready:
                //タッチしたらゲームスタート
                if (Input.GetButtonDown("Fire1"))
                { 
                        GameStart();
                }
                break;
            case State.Play:
                //キャラクターが死亡したら(Trueになったら)ゲームオーバー
                if (azarashi.IsDead())
                {
                    GameOver();
                }
                break;
            case State.GameOver:
                //タッチしたらシーンをリロード　（ゲームを初期化）
                if (Input.GetButtonDown("Fire1"))
                {
                    Reload();
                }
                break;
        }
    }
    
    void Ready()
    {
        //ステートをReady状態に移行し、キャラクターの操作をオフの状態にする(3つ以外の型をいれるとコンパイルエラー）
        state = State.Ready;

        //またBlock全体をSetActive関数で無効化することでブロックがスクロールしないようにする
        //各オブジェクトを無効状態にする
        azarashi.SetSteerActive(false);　//isKinematic Trueで動けなくなる
        blocks.SetActive(false); //ゲームオブジェクトの上のところのチェックボタンのオンオフをsetActiveはする（メソッド）
        //enabledはプロパティでコンポーネントのオンオフをする

        //ラベルを更新
        scoreText.text = "Score : " + 0;

        stateText.gameObject.SetActive(true);
        //インスペクターの（一番）上のチェックをつけたり外したりして文字を見えるか見なくするかしている
        stateText.text = "Ready";
    }

    void GameStart()
    {   
        //ステートをプレイ状態にし、キャラ操作を可能にし、ブロックのスクロールを開始する
        //このときキャラ操作はまだ無効の為、ゲーム開始のタップ操作ではキャラは動かない
        //そのためゲーム開始時の最初だけGameControllerからFlap関数を呼び出し、通常の操作と同じような動きにする
        state = State.Play;

        //各オブジェクトを有効にする
        azarashi.SetSteerActive(true);//isKinematic falseになって動けるようになる
        blocks.SetActive(true);

        //最初の入力だけゲームコントローラーから渡す
        azarashi.Flap();

        //ラベルを更新
        stateText.gameObject.SetActive(false);
        stateText.text = "";
    }

    void GameOver()
    {
        state = State.GameOver;

        //Find関数を用いることでシーン中からヒエラルキーにある全てのScrollObjectのコンポーネントを配列として探す。
        //シーン中の全てのScrollObjectコンポーネントを探し出す
        ScrollObject[] scrollObjects = FindObjectsOfType<ScrollObject>();

        //enabledフラグをfalseにすることでスクロールを停止させる
        //全ScrollObjectのスクロール処理を無効にする
        foreach (ScrollObject so in scrollObjects) so.enabled = false;
        //foreachは拡張for文 soは変数名 inはコロンの代わり　

        //ラベルを更新
        stateText.gameObject.SetActive(true);
        stateText.text = "GameOver";
    }

    void Reload()
    {
        //現在読み込んでいるシーンを再読み込み
        //SceneManager.GetActiveScene関数では、現在読み込んでいるシーンのシーン構造体（シーンの名前を）取得可能。
        //Scene構造体のnameプロパティをSceneManager.LoadScene関数に渡すことで、シーンをリロードすることが可能。
        //（シーン番号でシーン遷移することも可能LoadScene0）
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
        //SceneManagerはこのためにimportした
    }
    
    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score : " + score;
    }
}
