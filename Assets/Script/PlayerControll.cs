using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControll : MonoBehaviour
{
    // パラメータとオブジェクトの設定
    [SerializeField] float moveSpeed; // プレイヤーのスピード
    [SerializeField] float chargeTime; // 弾を発射始める時間
    [SerializeField] float lounchTime; // 次の弾を発射する時間
    [SerializeField] private AudioClip attack_sound; // 発射のサウンド変数
    [SerializeField] private AudioClip charge_sound; // チャージのサウンド変数
    [SerializeField] private Slider hp_bar; // HPバーのスライダーをいれる
    [SerializeField] private Slider mp_bar; // MPバーのスライダーをいれる
    [SerializeField] private float maxHP; // 最大HPを設定
    [SerializeField] private float maxMP; // 最大MPを設定
    [SerializeField] private GameObject unitmanager; // パラメータを保存しているオブジェクト

    // オブジェクト
    private GameObject Bullet; // プレイヤーが打つ弾のオブジェクト
    
    // アニメーションとサウンド
    private Animator animator; // プレイヤーのアニメーション
    private AudioSource audioSource; // サウンドコンポーネント用
    
    // ライフ管理
    private float currentHP; // 現在のHP
    private float currentMP = 0; // 現在のMP　初期値:0
    [SerializeField] private GameObject a_Prefab; // 弾オブジェクト
    public static float LifeRate = 0; // 被ダメの計算
    
    // パラメータ取得用
    private float receive_dg; // プレイヤーが受けるダメージ
    private int unitnumber; // パラメータが保存されている場所を示す

    // プレイヤーの入力
    private float p_x; // 横移動
    private float p_y; // 縦移動

    // 他スクリプト用
    private UnitManage UnitManage; // パラメータを管理しているスクリプトをいれる

    // プレイヤーの攻撃用
    public float Magic_cs; // 消費MP
    public float charge_MPTime; // MPをチャージする間隔
    public float charge_MP; // 加算していくMPの値

    private void Start()
    {
        // アニメーション
        animator = GetComponent<Animator>(); // Animatorコンポーネントを取得
        
        // サウンド
        audioSource = GetComponent<AudioSource>(); // AudioSourceコンポーネントを取得

        // キャラクターパラメータ
        UnitManage = unitmanager.GetComponent<UnitManage>(); // キャラが保存されているデータを呼び出す

        // プレイヤーパラメータ
        hp_bar.value = 1; // HPバーを満タンにしておく
        mp_bar.value = 0; // MPバーをゼロにしておく
        currentHP = maxHP; // 現在のHPを最大HPと同じ値にしておく
        currentMP = 0;
        Magic_cs = 10; // 消費MPを初期値10に設定する
        charge_MPTime = 0.1f; // MPのチャージ間隔
        charge_MP = 1;
    }

    void Update()
    {
        p_x = Input.GetAxisRaw("Horizontal"); // 横方向の入力
        //p_y = Input.GetAxisRaw("Vertical"); // 縦方向の入力。三項条件演算子で十字移動可
        p_y = (p_x == 0) ? Input.GetAxisRaw("Vertical") : 0.0f; // 十字移動の場合

        // 移動入力に応じた値の設定
        if (p_x != 0 || p_y != 0)
        {
            animator.SetFloat("x", p_x); // 横移動
            animator.SetFloat("y", p_y); // 縦移動
        }

        

        //動く
        transform.position += new Vector3(p_x, p_y).normalized * Time.deltaTime * moveSpeed;
        if (Time.deltaTime != 0) // ポーズ画面じゃないとき
        {
            if (Input.GetMouseButtonDown(0)) { // 左クリックされているとき
                if (currentMP < maxMP)
                {
                    audioSource.PlayOneShot(charge_sound); // チャージ音を出す
                    InvokeRepeating ("MPChargeGauge", 0, charge_MPTime); // charge_MPTimeの間隔でMPチャージ
                }
            }

            if (Input.GetMouseButtonUp(0)){ // 左クリックを離したとき
                audioSource.Stop(); // チャージ音を止める
                CancelInvoke(); // チャージを止める 
            }

            if (Input.GetMouseButtonDown(1)){ // 右クリックが押されたとき
                InvokeRepeating ("GeneAt", 0, lounchTime); // lounchTimeの間隔で攻撃
            }

            if (Input.GetMouseButtonUp(1)){ // 右クリックを離したとき
                CancelInvoke(); // 攻撃を止める
            }
        }
    }

    // 衝突判定
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "E1_Prefab(Clone)") // 敵と当たった時
        {
            UnitManage.UnitSearch(other.gameObject.tag); // パラメータの保存場所を取得

            lifemanager(); // プレイヤーライフ処理
        }

        // クリアの箇所に来た時、クリア画面へ
        if (other.gameObject.name == "Clear") GameClear();

        // 戻る場所に来た時タイトルに戻る
        if (other.gameObject.name == "Title") Title();

    }

    // MPをチャージする
    void MPChargeGauge()
    {
        currentMP = currentMP + charge_MP; // 現在のMPに加算
        mp_bar.value = currentMP / maxMP; // MPバーに反映
    }

    // プレイヤーのライフ処理
    void lifemanager()
    {
        receive_dg = UnitManage.charpower[UnitManage.unitnumber] + UnitManage.Buff_Power; // ダメージを決定

        currentHP = currentHP - receive_dg; // ダメージ分を引く
        LifeRate = currentHP / maxHP; // 体力バーの長さを計算
        hp_bar.value = LifeRate; // HPバーに反映

        if (hp_bar.value <= 0) // ライフがゼロになった時
        {
            GameOver(); // ゲームオーバー画面へ遷移
        }
    }

    // 弾を生成する
    void GeneAt()
    {
        audioSource.Stop(); // チャージ音を止める

        if (currentMP > Magic_cs)
        {
            currentMP = currentMP - Magic_cs; // 現在のMPに消費MPを引く
            mp_bar.value = currentMP / maxMP; // MPバーに反映

            // 弾のインスタンスを代入
            Bullet = Instantiate(a_Prefab, transform.position, Quaternion.identity);
            
            // getVect関数を呼び出し打ち出す方向を取得
            Bullet.GetComponent<AttackController>().getVect(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            
            audioSource.PlayOneShot(attack_sound); // 発射音を出す


        }
    }

    // タイトル画面への遷移
    void Title()
    {
        SceneManager.LoadScene("TitleScene", LoadSceneMode.Single);
    }
    // クリア画面への遷移
    void GameClear()
    {
        SceneManager.LoadScene("GameClear", LoadSceneMode.Single);
    }
    // ゲームオーバーへの遷移
    void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single); // ゲームオーバー画面に遷移する
    }
}
