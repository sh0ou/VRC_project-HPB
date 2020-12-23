using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

/// <summary>
/// 譜面ファイルをテキストに変換し、配列格納するスクリプト。
/// </summary>
public class TextFileConverter : UdonSharpBehaviour
{
    [SerializeField, Tooltip("テキストアセット\n<1列目>参照する譜面番号\n<2列目>0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ")]
    private TextAsset[][] textFile;

    //[SerializeField, Tooltip("テキストUI\n0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ")]
    //private Text[] textUI = new Text[4];

    [Tooltip("テキスト格納用変数\n0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ")]
    private string[] textStorage = new string[4];

    [SerializeField, Tooltip("テキストDB\n<1列目>\n0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ\n<2列目>\n参照する要素番号")]
    public string[][] textDB = new string[4][];

    void Start()
    {
        //テキスト表示
        //textUI[0].text = textDB[0][1];
        //textUI[1].text = textDB[1][1];
        //textUI[2].text = textDB[2][1];
        //textUI[3].text = textDB[3][1];
    }
    /// <summary>
    /// 譜面ファイルを配列に変換するメソッド
    /// </summary>
    /// <param name="参照する譜面番号"></param>
    public void SetTextFile(int i)
    {
        Debug.Log("譜面番号" + i + "のテキストファイル変換を開始します...");
        textStorage[0] = textFile[i][0].text;
        textStorage[1] = textFile[i][1].text;
        textStorage[2] = textFile[i][2].text;
        textStorage[3] = textFile[i][3].text;
        //格納変数を配列化
        Debug.Log("配列代入を開始します...");
        textDB[0] = textStorage[0].Split('\n');
        textDB[1] = textStorage[1].Split('\n');
        textDB[2] = textStorage[2].Split('\n');
        textDB[3] = textStorage[3].Split('\n');
        Debug.Log("<color=green>譜面変換が完了しました</color>");
    }
}