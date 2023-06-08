using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fairy_Move : MonoBehaviour
{
    // オブジェクト
    [SerializeField] private GameObject unitmanager; // パラメータを保存しているオブジェクト(妖精のスピードを取得)
    [SerializeField] private GameObject Player; // プレイヤーのオブジェクト
    
    // サウンド
    private AudioSource audioSource; // サウンドコンポーネント用
    [SerializeField] private AudioClip Enemy_Reflect; // 敵にぶつかり跳ね返した時のサウンド

    // マウス
    private Vector3 Mouse_possition; // マウスのある座標
    private Vector3 Target_Position; // 妖精が向かう方向

    // ベクトル処理
    private Vector3 ReflectPos; // 敵とぶつかった時にベクトルを処理する

    // 妖精のキャラナンバー
    public int Fairy_number;

    // パラメータ
    private float Move_Speed; // 移動スピード
    [SerializeField] private float Add_Speed; // キャラの移動によるスピードの補正

    // 他スクリプト用
    private PlayerControll PlayerControll; // プレイヤーのスクリプトをいれる

    void Start()
    {
        // サウンド
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得

        // マウスカーソルの非表示
        Cursor.visible = false;

        // プレイヤーのスクリプトを取得
        PlayerControll = Player.GetComponent<PlayerControll>();

        // キャラが保存されているデータを呼び出す
        Move_Speed = unitmanager.GetComponent<UnitManage>().charspeed[Fairy_number]; 
        
    }

    // Update is called once per frame
    void Update()
    {
        // マウスの座標を取得
        Mouse_possition = Input.mousePosition;

        // 向かう方向の取得
        Target_Position = Camera.main.ScreenToWorldPoint(new Vector2(Mouse_possition.x, Mouse_possition.y));

        // マウスの座標の方へ進む
        transform.position = Vector2.MoveTowards(transform.position, Target_Position, Move_Speed * Time.deltaTime + Mathf.Sign(PlayerControll.p_x) * PlayerControll.p_x * Add_Speed 
            + Mathf.Sign(PlayerControll.p_y) * PlayerControll.p_y * Add_Speed);
    }

    // 衝突判定
    void OnTriggerEnter2D(Collider2D other)
    {
        // 敵に当たったとき
        if (other.gameObject.name == "E1_Prefab(Clone)")
        {
            // 敵と妖精のベクトルを取得
            ReflectPos = other.transform.position - this.transform.position;
            
            // 妖精とは逆方向に跳ね返る
            other.transform.position = other.transform.position + ReflectPos.normalized;

            // 跳ね返り音を出す
            audioSource.PlayOneShot(Enemy_Reflect);
        }
    }
}
