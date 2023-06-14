using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E1_Generator : MonoBehaviour
{
	// オブジェクト用
    [SerializeField] private GameObject E1Prefab; // エネミーオブジェクト取得用
	[SerializeField] private GameObject player1; // プレイヤーオブジェクト取得用
	[SerializeField] private GameObject ClearConditionObject; // クリア条件を管理するオブジェクト
	
	// エフェクト用
	[SerializeField] private ParticleSystem e_SpownFin; // スポーン停止エフェクト
	[SerializeField] private ParticleSystem e_Spown; // スポーンエフェクト
	
	// サウンド用
	[SerializeField] private AudioClip spawn_sound; // スポーンのサウンド変数
	private AudioSource audioSource; // サウンドコンポーネント用

	// スポーン関係
	[SerializeField] private int SpawnTime; // スポーンする間隔
	[SerializeField] private int FirstSpawnTime; // 最初にスポーンするまでの時間
	[SerializeField] private int SpawnRange; // スポーン位置からプレイヤーを検知する範囲

	// スクリプト
	private ClearCondition ClearCondition; // クリア条件を管理しているスクリプトをいれる
	
	// 初期設定
	void Start () {
		// サウンド
		audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得
		
		// エネミー生成開始
		InvokeRepeating ("GeneE1", FirstSpawnTime, SpawnTime); // FirstSpawnTime秒後にSpawnTime秒間隔で呼び出す

		// クリア状況
		ClearCondition = ClearConditionObject.GetComponent<ClearCondition>(); // クリア条件のスクリプトを格納
	}

	// プレイヤー座標の監視
	void Update()
	{
		if (gameObject.activeSelf == true) // スポーン床がアクティブなら
		{
			// プレイヤーがスポーン位置からSpawnRangeの範囲に入ったら
			if (player1.transform.position.x > transform.position.x - SpawnRange && player1.transform.position.x < transform.position.x + SpawnRange && player1.transform.position.y < transform.position.y + SpawnRange && player1.transform.position.y > transform.position.y - SpawnRange)
			{
				CancelInvoke(); // スポーンを止める
				e_SpownFin.transform.position = this.transform.position; // エフェクトの場所をスポーン場所へ
				Instantiate (e_SpownFin); // スポーン停止エフェクトを生成
				gameObject.SetActive (false); // スポーン床のアクティブをオフにする
				ClearCondition.CheckCondition(); // クリア状況の確認
			}
		}
	}
	
	// エネミー生成
	void GeneE1 () {
		audioSource.PlayOneShot(spawn_sound); // スポーン音を出す
		e_Spown.transform.position = this.transform.position; // エフェクトの場所をスポーン場所へ
		Instantiate (e_Spown); // スポーンエフェクトを生成

		// エネミーを現在座標または少しずれた場所に出現させる
		Instantiate (E1Prefab, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
	}
}
