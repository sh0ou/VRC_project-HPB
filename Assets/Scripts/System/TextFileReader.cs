using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// テキストファイルを文字列化し、格納します。
    /// </summary>
    public class TextFileReader : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("テキストアセット")]
        private TextAsset[] textfiles;
        [SerializeField] private PagingViewer pagingViewer;

        [TextArea] public string[] textStorages;

        void Start()
        {
            textStorages = new string[textfiles.Length];
            for (int i = 0; i < textfiles.Length; i++)
            {
                //Debug.Log("[<color=yellow>TextFileReader</color>]textconvert..." + i);
                ConvertFileToString(i);
            }
            //Debug.Log("[<color=yellow>TextFileReader</color>]Check:" + textStorages[0]);
            //Debug.Log("[<color=green>TextFileReader</color>]Convert OK");
            pagingViewer.Initialize();
        }

        /// <summary>
        /// TextAssetをStringに変換します
        /// </summary>
        public void ConvertFileToString(int i)
        {
            textStorages[i] = textfiles[i].text;
        }
    }
}