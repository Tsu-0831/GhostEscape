using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultShow : MonoBehaviour
{
    // オブジェクト
    public GameObject SubjugationTextObject; // 撃破数を表示するテキストオブジェクト
    public GameObject ClearTimeTextObject; // クリア時間を表示するテキストオブジェクト
    public GameObject Clear_CalculateScoreTextObject; // クリアスコアの計算内容を表示するテキストオブジェクト
    public GameObject Clear_TotalScoreTextObject; // クリアスコアの合計値を表示するテキストオブジェクト
    public GameObject Rank_AssesmentTextObject; // クリアランクを表示するテキストオブジェクト

    // テキスト
    private Text SubjugationText; // 敵の撃破数として表示する文字
    private Text ClearTimeText; // クリアタイムとして表示する文字
    private Text Clear_CalculateScoreText; // クリアスコアの計算内容として表示する文字
    private Text Clear_TotalScoreText; // クリアスコアの合計として表示する文字
    private Text Rank_AssesmentText; // クリアランクとして表示する文字

    // スコア関連
    private int ClearTime; // 秒単位の合計クリアタイム
    private int ClearTime_Second; // クリアタイム分単位
    private int ClearTime_Minute; // クリアタイム(分に直した時の)秒単位
    private int TotalScore; // 最終スコア(スコアの合計)
    private int GeneralScore; // 通常スコア(クリアタイム)
    private int AddScore_Subjugation; // 加算スコア(撃破数)
    private int AddScore_Damage; // 加算スコア(被ダメ)
    [SerializeField] int Score_Calculation_BaseTime; // スコア計算の時間基準を決める
    [SerializeField] private int Rank_Base_S; // S判定の基準
    [SerializeField] private int Rank_Base_A; // A判定の基準
    [SerializeField] private int Rank_Base_B; // B判定の基準

    // 初期設定
    void Start()
    {
        // テキストコンポーネントを取得
        SubjugationText = SubjugationTextObject.GetComponent<Text>();
        ClearTimeText = ClearTimeTextObject.GetComponent<Text>();
        Clear_CalculateScoreText = Clear_CalculateScoreTextObject.GetComponent<Text>();
        Clear_TotalScoreText = Clear_TotalScoreTextObject.GetComponent<Text>();
        Rank_AssesmentText = Rank_AssesmentTextObject.GetComponent<Text>();

        // クリアタイム計算
        ClearTime = (int)ClearCondition.Second;
        ClearTime_Second = ClearTime / 60; // 分単位にする
        ClearTime_Minute = ClearTime % 60; // 秒単位の部分を取得

        // スコア計算
        GeneralScore = Score_Calculation_BaseTime * 10000 - (ClearTime - Score_Calculation_BaseTime) * 2000;
        AddScore_Subjugation = ClearCondition.Subjugation * 1000;
        AddScore_Damage = (int)(PlayerControll.LifeRate * 10000);
        TotalScore = GeneralScore + AddScore_Subjugation + AddScore_Damage;

        // ランク確定
        if (TotalScore >= Rank_Base_S)
        {
            Rank_AssesmentText.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); // 文字の色を黄色にする
            Rank_AssesmentText.text = "S"; // ランクS判定
        }
        else if (TotalScore >= Rank_Base_A)
        {
            Rank_AssesmentText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f); // 文字の色を赤色にする
            Rank_AssesmentText.text = "A"; // ランクA判定
        }
        else if (TotalScore >= Rank_Base_B)
        {
            Rank_AssesmentText.color = new Color(0.0f, 0.0f, 1.0f, 1.0f); // 文字の色を青色にする
            Rank_AssesmentText.text = "B"; // ランクB判定
        }
        else
        {
            Rank_AssesmentText.color = new Color(0.3f, 0.3f, 0.3f, 1.0f); // 文字の色を濃い灰色にする
            Rank_AssesmentText.text = "C"; // ランクC判定
        }

        // テキスト設定
        SubjugationText.text = "撃破数:" + ClearCondition.Subjugation + "体"; // 撃破数を表記
        ClearTimeText.text = "クリアタイム:" + ClearTime_Second + "分" + ClearTime_Minute +"秒";// クリアタイムを表記
        Clear_CalculateScoreText.text = "クリアタイムスコア:" + GeneralScore + "\n"
                                         + "撃破数ボーナス:" + ClearCondition.Subjugation +"体×1000" + "\n"
                                         + "被ダメボーナス:" + AddScore_Damage; // クリアスコア概要を表示
        Clear_TotalScoreText.text = "合計スコア:" + TotalScore;

    }
}
