using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    /// <summary>
    /// テキストをページ毎に表示させるスクリプト
    /// </summary>
    public class PagingViewer : UdonSharpBehaviour
    {
        [SerializeField] TextMeshProUGUI targetText;
        [SerializeField] TextMeshProUGUI pageNumText_now;
        [SerializeField] TextMeshProUGUI pageNumText_max;
        [SerializeField] TextFileReader textFileReader;
        int pageNum = 0;

        void Start()
        {
            //Debug.Log("pagetext:" + textFileReader.textStorages.Length);
            pageNum = textFileReader.textStorages.Length - 1;
            targetText.text = textFileReader.textStorages[pageNum];
            pageNumText_max.text = textFileReader.textStorages.Length.ToString();
            pageNumText_now.text = pageNum + 1 + " / ";
        }

        /// <summary>
        /// 次のページを表示
        /// </summary>
        public void NextPage()
        {
            if (pageNum == 0)
            {
                pageNum = textFileReader.textStorages.Length - 1;
            }
            else
            {
                pageNum--;
            }
            targetText.text = textFileReader.textStorages[pageNum];
            pageNumText_now.text = pageNum + 1 + " / ";
        }

        /// <summary>
        /// 前のページを表示
        /// </summary>
        public void BackPage()
        {
            if (pageNum == textFileReader.textStorages.Length - 1)
            {
                pageNum = 0;
            }
            else
            {
                pageNum++;
            }
            targetText.text = textFileReader.textStorages[pageNum];
            pageNumText_now.text = pageNum + 1 + " / ";
        }
    }
}