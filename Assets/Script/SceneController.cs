using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // [SerializeField] private GameObject player; // プレイヤーを格納

    // Update is called once per frame
    void Update()
    {
        // スペースキーが押されたとき
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (SceneManager.GetActiveScene().name == "TitleScene") // タイトル画面の時
            {
                // チュートリアル画面へ
                SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
            }
            else if (SceneManager.GetActiveScene().name == "Tutorial")// チュートリアル画面の時
            {
                // プレイ画面へ
                SceneManager.LoadScene("PlayScene", LoadSceneMode.Single);
            }
            else if (SceneManager.GetActiveScene().name == "GameClear" || SceneManager.GetActiveScene().name == "GameClear")// ゲームクリア画面かゲームオーバー画面の時
            {
                // タイトル画面へ
                SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
            }
       }
    }

    // プレイヤーがクリアまたはタイトルへ戻るためのオブジェクトにぶつかった時
    void OnTriggerEnter2D(Collider2D coll){
        // クリアの箇所に来た時、クリア画面へ
        if (coll.gameObject.name == "Clear") SceneManager.LoadScene("GameClear", LoadSceneMode.Single);

        // 戻る場所に来た時タイトルに戻る
        if (coll.gameObject.name == "Title") SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
}
