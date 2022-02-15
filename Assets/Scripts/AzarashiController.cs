using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzarashiController : MonoBehaviour
{
    Rigidbody2D rb2d;
    Animator animator; 
    float angle; //z軸に値を入れるとアザラシが回転する
    bool isDead;

    public float maxHeight;　//画面外に飛んでいかないように上限決める
    public float flapVelocity;　//y軸に飛んでいく速度　上昇速度
    public float relativeVelocityX; //相対速度X　見かけのアザラシの速度は背景移動の３の速度
    public GameObject sprite;　//子要素
    //子のAzarashiSpriteオブジェクトの参照をInspectorビューから設定

    public bool IsDead()
    {
        return isDead;
    }

    private void Awake()//Awake関数はオブジェクトが生成された瞬間に呼ばれる
                        //（コンポーネントの取得を全てのオブジェクトのStart関数より早い段階で行う）
    //順番的には　New（インスタンスの生成）→Awake→Start→Update
    
    {
        rb2d = GetComponent<Rigidbody2D>();//自分のコンポーネントのrigidbody2の種地区
        //スプライトに設定されているAnimatorコンポーネントをAwake時に取得しておく
        animator = sprite.GetComponent<Animator>();
    }
    void Update()
    {
        //最高高度に達していない場合に限りタップの入力を受け付ける
        if(Input.GetButtonDown("Fire1")&& transform.position.y < maxHeight)
            //いきなりトランスフォームなのでアタッチされているゲームオブジェクトのトランスフォーム
            //（AzarashiのTransition）
        {
            Flap();
        }

        //角度を反映
        ApplyAngle();

        //ApplyAngleで計算した角度が0度以上、つまりキャラクターが平行よりも上を向いてるときに
        //Flapアニメーションを再生するようにAnimatorのパラメーターを設定する
        //angleが水平以上だったら、アニメーターのflapフラグをTrueにする
        animator.SetBool("flap", angle >= 0.0f && !isDead);
    }

    public void Flap()
    {
        //死んだら羽ばたけない
        if (isDead)
        {
            return;
        }

        //Velocityを直接書き換えて上方向に加速
        rb2d.velocity = new Vector2(0.0f, flapVelocity);
    }

    void ApplyAngle()
    {
        //現在の速度、相対速度から進んでいる角度を求める
        float targetAngle;

        //死亡したら常にひっくり返る
        if (isDead)
        {
            targetAngle = 180.0f;
        }
        else
        {
            targetAngle = //A(rc)tan2(to)逆三角関数2辺の比から角度を教えてくれる）
                          //（１，１）→45°　
                          //三角関数は角度から2辺の比を教えてくれる　
                          //Rad2(to)Degラジアンからディグリーに変換
               Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
            //relativeVelocityX=3 背景の速度
        }

        //向きたい方向のTargetAngleに急激に変更しないように、一旦保存しておいたangleを元に
        //滑らかにアニメーションするようにしている
        //回転アニメをスムージング
        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);
        //Lerp...第一引数と第二引数の差を第三引数の割合で決める返す（0~1の間）
        //例）angle 0, targetAngle 30, time.deltaTime(0.016秒)30度の16％で4.5度位
        //Lerp使用するとゆっくりになる　使用しないと

        //Rotationの反映
        sprite.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
        //azarashiのトランスフォームの子要素（ローカル）ローテーションで回転を決めてる
        //（2Dなのでアザラシの上下斜めの向きが決まる）
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead)
        {
            return;
        }
        
        //何かにぶつかったら死亡フラグをたてる
        isDead = true;
    }
}
