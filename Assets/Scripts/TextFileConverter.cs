using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

namespace HPB
{
    /// <summary>
    /// 譜面ファイルをテキストに変換し、配列格納するスクリプト。
    /// </summary>
    public class TextFileConverter : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("テキストアセット\n（1,2列目はparamタグを参照）\n<3列目>0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ")]
        private TextAsset[][][] textFile;

        [Tooltip("テキスト格納用変数\n0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ")]
        private string[] textStorage = new string[4];

        [SerializeField, Tooltip("テキストDB\n<1列目>\n0=基本データ\n1=時間データ\n2=ノーツデータ\n3=レーンデータ\n<2列目>\n参照する要素番号")]
        public string[][] textDB = new string[4][];

        void Start()
        {

        }
        /// <summary>
        /// 譜面ファイルを配列に変換するメソッド（譜面番号,レベル）
        /// </summary>
        ///<param name="i">譜面番号</param><param name="i2">レベル種</param>
        public void SetTextFile(int i, int i2)
        {
            //テキストファイルを変換
            textStorage[0] = textFile[i][i2][0].text;
            textStorage[1] = textFile[i][i2][1].text;
            textStorage[2] = textFile[i][i2][2].text;
            textStorage[3] = textFile[i][i2][3].text;
            //格納変数を配列化
            textDB[0] = textStorage[0].Split('\n');
            textDB[1] = textStorage[1].Split('\n');
            textDB[2] = textStorage[2].Split('\n');
            textDB[3] = textStorage[3].Split('\n');
            Debug.Log("<color=green>譜面変換が完了しました</color>");
        }

        /// <summary>
        /// 曲数を返す
        /// </summary>
        /// <returns></returns>
        public int SendMusicLength()
        {
            Debug.Log("曲数:" + textFile.Length);
            return textFile.Length - 1;
        }
    }
}