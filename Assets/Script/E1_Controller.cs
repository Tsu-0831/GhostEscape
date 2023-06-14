using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Controller : MonoBehaviour
{
    // オブジェクト用
    private GameObject unitmanager; // パラメータを保存しているオブジェクト
    public GameObject player = null; // プレイヤーオブジェクト

    // サウンド
    private AudioSource audioSource; // サウンドコンポーネント用

    // アニメーション
    private Animator animator; // アニメーター変数
    
    // パラメータ
    private float moveSpeed; // 敵のスピード制御変数
    private int UnitNumber_Ghost = 0; // "Ghost"パラメータの格納場所

    // 他スクリプト用
    private UnitManage UnitManage; // パラメータを管理しているスクリプトをいれる

    // 初期設定
    void Start()
    {
        // オブジェクト
        player = GameObject.Find("Player1"); // プレイヤーオブジェクトを取得
        unitmanager = GameObject.Find("UnitManager"); // キャラのパラメータが格納されている場所を探す

        // コンポーネント
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
        animator = GetComponent<Animator>(); // アニメーションコンポーネントを取得
        UnitManage = unitmanager.GetComponent<UnitManage>(); // キャラが保存されているデータを呼び出す
        
    }

    // 入力の監視
    void Update()
    {
        // ポーズ画面の時は音を止める
        if (Time.deltaTime == 0) audioSource.Pause(); 
        else audioSource.UnPause();

        // ゴーストの設定された速度でプレイヤーの方へ移動していく
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (UnitManage.charspeed[UnitNumber_Ghost] + UnitManage.Buff_Speed) * Time.deltaTime);
    }
}
