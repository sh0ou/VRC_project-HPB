using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ゲームプレイ時に必要な変数を格納するスクリプト
/// </summary>
public class HPB_PlayManager : UdonSharpBehaviour
{
    /* やることリスト
    ・ここのUI変数を追加@OK
    ・ドラムによるUI操作@OK
        1,4＝もどる　2,3=難易度変更
        ドラムを叩く
        叩かれたドラムの番号メソッドを送信
        （switch文で？）表示画面に応じた処理を実行
    ・UI遷移の実装
    ・AH,FC処理の実装
        最終ノーツ（Lencthで取得）通過時に判定
        H判定数 = 総ノーツ時はAHアニメ処理
        （else if）S,M判定が0 = FCアニメ処理
    ・曲終了時にUIを表示させる
        DBで指定された再生時間をすぎると発火
        リザルトUIアニメ
        ランク演出
        アニメ終了後に表示画面変数を変更
        （もどるアクションをアクティブにする）
    ・基本データを取得する@OK
        選曲画面ロード時
        レベル選択画面ロード時
        プレイ画面ロード時
    ・スタート画面@OK
        いずれかのドラムを叩くとスタート
        選曲画面表示
        表示画面変数変更処理
     */
    #region 変数
    [SerializeField, Tooltip("ドラムスティック（右）")]
    public GameObject drumStick;

    [SerializeField]
    public float playTime;

    [SerializeField, Tooltip("楽曲スコア")]
    public int score_now;

    [SerializeField, Tooltip("1ノーツあたりのスコア")]
    public int score_Once;

    [SerializeField, Tooltip("クリアランク\n0=D 1=C 2=B\n3=A 4=S")]
    public int clearRank;

    [SerializeField, Tooltip("チェイン数")]
    public int chain;

    [SerializeField, Tooltip("ノーツ判定数\n0=Happy\n1=Good\n2=Sad\n3=Miss")]
    public int[] judgedValue = new int[4];

    [SerializeField, Tooltip("フルチェインフラグ")]
    public bool fcFlag;

    [SerializeField, Tooltip("オールハッピーフラグ")]
    public bool ahFlag;
    #endregion

    void Start()
    {
        playTime = 0;
    }
}
