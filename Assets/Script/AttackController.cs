using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    // オブジェクト
    private GameObject ClearConditionObject = null; // クリア状況を保存しているオブジェクト
    
    // エフェクト
    [SerializeField] private ParticleSystem e_dead_effect; // エネミー消滅エフェクト
    
    // 弾道処理に使用
    [SerializeField] private float a_speed; // 弾の速度
    private Vector3 direction; // 打ち出す方向
    private Vector3 newPos; // 行先の座標
    private Transform thisTransform; // 弾の初期座標
    
    // ほかスクリプト用
    private ClearCondition ClearCondition; // クリア状況を保存しているスクリプトをいれる
    
    void Start()
    {
        thisTransform = transform; // 弾の初期位置を設定
    }

    void Update()
    {
        newPos = thisTransform.position + new Vector3(direction.x, direction.y, 0).normalized * Time.deltaTime * a_speed; // 次の座標を取得
        transform.position = new Vector3(newPos.x, newPos.y, newPos.z); // 計算した方向へ進める
    }
    
    // ベクトル計算　"PlayerControll.cs"内で呼び出される
    public void getVect(Vector3 from, Vector3 to)
    {
        // 各座標の差を取る
        direction = new Vector3(to.x - from.x, to.y - from.y, 0);
    }

    // 衝突検知
    void OnTriggerEnter2D(Collider2D coll) 
    {
        // 壁に当たったとき、弾を消す
        if (coll.gameObject.name == "TilemapObject") Destroy (gameObject);

        // 敵に当たったとき
        if (coll.gameObject.name == "E1_Prefab(Clone)")
        {
            ClearConditionObject = GameObject.Find("ClearCondition"); // "ClearCondition"オブジェクトを探す
            
            if (ClearConditionObject != null) // オブジェクトを発見したとき
            {
                // クリア状況
                ClearCondition = ClearConditionObject.GetComponent<ClearCondition>(); // クリア条件のスクリプトを格納

                ClearCondition.SubjugationCount(); // 撃破数をカウント
            }
            Destroy (gameObject); // 弾を消す
            e_dead_effect.transform.position = coll.gameObject.transform.position; // エフェクトの場所をエネミー座標に設定
            Instantiate (e_dead_effect); // エネミー消滅エフェクトを生成
            Destroy (coll.gameObject); // 敵を消す
        }
    }
}