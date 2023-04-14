using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearCondition : MonoBehaviour
{
    // ゲームオブジェクト用
    [SerializeField] private GameObject OpenedObject; // 条件を満たすと破壊されるオブジェクト
    [SerializeField] private GameObject[] SpawnBoad = new GameObject[3]; // スポーンボードオブジェクト
    [SerializeField] private GameObject SubjugationObject; // 撃破数を表示するテキストオブジェクト
    [SerializeField] private GameObject TimeLimitObject; // 撃破数を表示するテキストオブジェクト
    [SerializeField] public GameObject DiffLevelObject; // 難易度が上がったことを表示するテキストオブジェクト
    [SerializeField] public GameObject MissionTextObject; // 道筋を示すテキストオブジェクト
    [SerializeField] private GameObject unitmanager; // パラメータを保存しているオブジェクト

    // カウント
    public int Condition = 0; // クリアまでの達成数
    public static int Subjugation; // エネミー討伐数(結果をゲームクリア画面に持ってくる)
    public int SpawnBoadLength; // SpawnBoad配列の大きさ
    public int CurrentTimeLimit; // 現在のカウントダウン表記
    public static float Second; // 経過時間を記録
    [SerializeField] public int PowerUpTimeLimit; // 何秒ごとに敵がパワーアップするまでのタイムリミットを設定する
    [SerializeField] public int LossWaitTime; // オブジェクトを非表示にするまでにかける時間
    [SerializeField] public int RandamRange; // 0～RandamRangeのランダム生成の範囲を指定する(パワーアップ内容の決定をする)
    private int TimeCount = 2; // 秒経過を記録する
    public int PowerUpNumber; // 敵の難易度アップ内容を番号で管理するため
    

    // テキスト
    private Text SubjugationText; // 撃破数を表示するテキスト文字
    private Text PowerUpTimeLimitText; // 敵のパワーアップのタイムリミットを表示するテキスト文字
    public Text DiffLevelText; // 難易度が上がったことを知らせるテキスト
    public Text MissionText; // 道筋を示すテキスト

    // 他スクリプト用
    private UnitManage UnitManage; // パラメータを管理しているスクリプトをいれる

    // Start is called before the first frame update
    void Start()
    {
        // 初期値設定
        Subjugation = 0; // 討伐数の初期値0
        Second = 0; // 経過時間の初期値0
        TimeCount = 2; // 秒経過記録のための初期値2


        // キャラクターパラメータ
        UnitManage = unitmanager.GetComponent<UnitManage>(); // キャラが保存されているデータを呼び出す

        // 長さ取得
        SpawnBoadLength = SpawnBoad.Length; // SpawnBoad配列の大きさを取得

        // テキスト
        SubjugationText = SubjugationObject.GetComponent<Text>(); // コンポーネント取得
        PowerUpTimeLimitText = TimeLimitObject.GetComponent<Text>();
        DiffLevelText = DiffLevelObject.GetComponent<Text>();
        MissionText = MissionTextObject.GetComponent<Text>();
        DiffLevelObject.SetActive(false); // 難易度上昇文を非表示
        
        // 時間
        CurrentTimeLimit = PowerUpTimeLimit; // 初期時間をセット

        // 初期表記
        PowerUpTimeLimitText.text = "パワーアップまで:" + CurrentTimeLimit.ToString ("00"); // パワーアップまでの時間
        MissionText.text = "全てのゴーストの出現場所を見つけ、魔法陣を壊そう!!"; // 道筋
    }

    void Update()
    {
        Second += Time.deltaTime; // 経過時間を計測

        if (Second % TimeCount >= TimeCount - 1) // 1秒経過したとき
        {
            PowerUpTimeLimitText.text = "パワーアップまで:" + CurrentTimeLimit.ToString ("00");

            CurrentTimeLimit--; // タイムリミットを一秒減らす

            TimeCount++;

            if (CurrentTimeLimit <= 0) // カウントがゼロになったら
            {
                PowerUpNumber = Random.Range(0, RandamRange); // 0～RandamRangeまでの範囲で乱数を生成し、敵のキャラのパワーアップ内容を決定する

                // 難易度アップ内容切り替え
                switch(PowerUpNumber)
                {
                    // 敵のパワーがアップ
                    case 0:
                        DiffLevelText.text = "モンスターの攻撃力が上がります";
                        UnitManage.UnitPowerUp(); // 攻撃力アップ
                        break;

                    // 敵のスピードがアップ
                    case 1:
                        DiffLevelText.text = "モンスターの移動速度が上がります";
                        UnitManage.UnitSpeedUp(); // スピードアップ
                        break;
                }
                
                CurrentTimeLimit = PowerUpTimeLimit; // 設定値まで戻す

                StartCoroutine(LossText(DiffLevelObject)); // 難易度変化文の表示・非表示
            }
        }
    }

    // 撃破数をカウントする関数
    public void SubjugationCount()
    {
        Subjugation++; // 撃破数をカウント

        SubjugationText.text = "撃破数:" + Subjugation; // 撃破数を表記
    }

    // クリア条件を満たしているか確認
    public void CheckCondition()
    {
        for (int i = 0; i < SpawnBoadLength; i++)
        {
            if (SpawnBoad[i].activeSelf == true) break; // スポーンがアクティブの場合for文を抜ける
            else Condition++; // スポーンがアクティブではない場合、解除状態+1
        }

        
        if (Condition == SpawnBoadLength)// 全部のスポーンを解除したとき(クリア条件)
        {
            OpenedObject.SetActive(false); // 出口のオブジェクトを消す

            MissionText.text = "全ての魔法陣が壊れ、出口の封印が解けました。いち早く抜け出そう。";
        }
    }

    //指定したオブジェクトを表示し、LossWaitTime秒後に非表示にするコルーチン
    IEnumerator LossText(GameObject TextObjectName)
    {
        TextObjectName.SetActive(true); // 難易度上昇文を表示

        yield return new WaitForSeconds(LossWaitTime); // LossWaitTime分待つ

        TextObjectName.SetActive(false); // オブジェクトの非表示
    }
}
