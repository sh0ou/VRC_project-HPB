using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// アニメーション処理中継スクリプト
    /// </summary>
    public class AnimRepeater : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("UIマネージャ")]
        private UIManager uiMng;

        [SerializeField, Tooltip("処理番号")]
        private int taskValue;

        /// <summary>
        /// ドラムアクティブ状態を切替
        /// </summary>
        public void SetEvent_0()
        {
            Debug.Log("アクティブ:SetEvent_0");
            uiMng.AnimEnd(0);
        }

        /// <summary>
        /// 選曲画面を表示
        /// </summary>
        public void SetEvent_1()
        {
            Debug.Log("アクティブ:SetEvent_1");
            uiMng.AnimEnd(1);
            gameObject.SetActive(false);
        }

        public void SetEvent()
        {
            Debug.Log("アクティブ:SetEvent");
            uiMng.AnimEnd(taskValue);
            if (gameObject.name == "Window_Play_2"/* && GameObject.Find("GameManager").GetComponent<HPB_SettingsManager>().windowFlag == 3*/)
            {
                Debug.Log("非アクティブ処理がスキップされました");
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            Debug.LogWarning("無効化されました:" + gameObject.name);
        }
    }
}