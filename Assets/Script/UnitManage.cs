using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManage : MonoBehaviour
{
    [SerializeField] public string[] charname = new string[1]; // 敵キャラの名前
    [SerializeField] public float[] charpower = new float[1]; // 敵キャラのパワー
    [SerializeField] public float[] charspeed = new float[1]; // 敵キャラの移動スピード
    [SerializeField] private float AddPower; // パワーの上昇値
    [SerializeField] private float AddSpeed; // スピードの上昇値
    
    public int UnitLength; // 配列の大きさ
    public int unitnumber; // キャラクターパラメータの格納場所を示す
    public float Buff_Power; // 現在のパワーの上昇値
    public float Buff_Speed; // 現在のスピードの上昇値

    // 初期設定
    void Start()
    {
        // 初期値設定
        Buff_Power = 0; // 現在のパワーの上昇値
        Buff_Speed = 0; // 現在のスピードの上昇値

        UnitLength = charname.Length; // 配列の大きさを代入
    }

    // 引数のキャラ名のパラメータが格納された場所を探す
    public void UnitSearch(string name_subject) // 引数の名前の場所を探す
    {
        // 引数の名前を配列から探す
        for (unitnumber = 0; unitnumber < UnitLength; unitnumber++)
        {
            // キャラが見つかった時、for文を抜ける
            if (name_subject == charname[unitnumber]) break;
        }
    }

    // 敵キャラクターのパワーを上げる
    public void UnitPowerUp()
    {
        // 敵キャラ全てが対象
        for (unitnumber = 0; unitnumber < UnitLength; unitnumber++)
        {
            Buff_Power += AddPower;
        }
    }

    // 敵キャラクターのスピードを上げる
    public void UnitSpeedUp()
    {
        // 敵キャラ全てが対象
        for (unitnumber = 0; unitnumber < UnitLength; unitnumber++)
        {
            Buff_Speed += AddSpeed;
        }
    }
}
