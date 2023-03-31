using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Button pauseButton; // 一時停止ボタン
    [SerializeField] private Button homeButton; // ホームボタン
    [SerializeField] private Button pauseCloseButton; // ポーズを閉じるボタン
    [SerializeField] private GameObject pauseScene; // ポーズ画面のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        pauseScene.SetActive(false); // ポーズ画面非表示
        pauseButton.onClick.AddListener(Pause); // 一時停止ボタンが押された時Pause関数を呼び出す
        homeButton.onClick.AddListener(Home); // ホームボタンが押された時Home関数を呼び出す
        pauseCloseButton.onClick.AddListener(PauseClose); // ポーズを閉じるボタンが押されたときPauseClose関数を呼び出す   
    }
    
    // ポーズを開く関数
    private void Pause()
    {
        Time.timeScale = 0; // 全てのオブジェクトの動きを止める
        pauseScene.SetActive(true); // ポーズ画面表示
    }

    // タイトル画面に戻る関数
    private void Home()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single); // タイトル画面に遷移する
        pauseScene.SetActive(false); // ポーズ画面非表示
    }

    // ポーズを閉じる関数
    private void PauseClose()
    {
        Time.timeScale = 1; // 全てのオブジェクトを動かす
        pauseScene.SetActive(false); // ポーズ画面非表示
    }

}
