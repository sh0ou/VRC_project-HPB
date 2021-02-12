﻿using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    /// <summary>
    /// ゲームプレイ時に必要なUIを格納するスクリプト
    /// </summary>
    public class UIManager : UdonSharpBehaviour
    {
        /*メモ
        色は0~1の小数で指定　255指定はできない
        bottomが下　topが上
        */

        #region 共通
        [Header("共通")]
        [SerializeField, Tooltip("ゲームマネージャ")]
        private GameManager gameMng;

        [SerializeField, Tooltip("プレイマネージャ")]
        private PlayManager playMng;

        [SerializeField, Tooltip("設定マネージャ")]
        private SettingsManager settingsMng;

        [SerializeField, Tooltip
            ("ドラムガイドUI\n0=左UI_選曲\n1=左UI_レベル\n2=右UI=選曲\n3=右UI_レベル")]
        private GameObject[] guideUI;

        [SerializeField, Tooltip("オプションUI")]
        private GameObject[] optionUI;

        [SerializeField, Tooltip
            ("ジャケット格納変数\n1列目=曲番号\n2列目=レベル番号")]
        public Material[][] jacketList;

        [SerializeField, Tooltip("パーティクルジェネレータ")]
        private ParticleGenerator particleGenerator;

        #endregion
        #region ウインドウ（アニメーションに使用）
        [SerializeField, Tooltip
            ("ウインドウアニメーター\n0=タイトル\n1=選択1\n2=選択2\n3=プレイ1\n4=プレイ2\n5=リザルト")]
        private Animator[] windowAnims;

        #endregion
        #region 選曲画面（値更新に使用）
        [Header("選曲画面1")]
        [SerializeField, Tooltip
            ("値更新用UI\n0=タイトル\n1=アーティスト")]
        public TextMeshProUGUI[] tmp_select_1;

        [SerializeField, Tooltip
            ("選曲画面1ジャケット画像\n0=中\n1=左\n2=右")]
        public GameObject[] jacketImage_select_1;

        [Header("選曲画面2")]
        [SerializeField, Tooltip
            ("値更新用UI\n0=タイトル\n1=アーティスト\n2=レベル1\n3=レベル2\n4=レベル3")]
        public TextMeshProUGUI[] tmp_select_2;

        [SerializeField, Tooltip("選曲画面2ジャケット画像")]
        public GameObject jacketImage_select_2;

        #endregion
        #region プレイ画面（値,色更新に使用）
        [Header("プレイ画面1")]
        [SerializeField, Tooltip
            ("値更新用UI\n0=タイトル\n1=アーティスト\n2=レベル1\n3=レベル2\n4=レベル3")]
        public TextMeshProUGUI[] tmp_play_1;

        [SerializeField, Tooltip("UIオブジェクト\n0=レベル1\n1=レベル2\n2=レベル3")]
        private GameObject[] uiObj_play_1;

        [Header("プレイ画面2")]
        [SerializeField, Tooltip
            ("値更新用UI\n0=スコアD\n1=スコアC\n2=スコアB\n3=スコアA\n4=スコアS\n5=チェイン\n6=チェインFC\n7=チェインAH")]
        public TextMeshProUGUI[] tmp_play_2;

        [SerializeField, Tooltip
            ("値更新アニメUI")]
        private Animator fixedText_play_2;

        [SerializeField, Tooltip("ノーツラインUI")]
        private GameObject notesLineUI;

        //[SerializeField, Tooltip("BPMガイドUI")]
        //private Animator[] bpmGuideUI;

        [SerializeField, Tooltip
            ("UIオブジェクト\n0=RankD 1=RankC 2=RankB\n3=RankA 4=RankS\n5=chain 6=chain_FC 7=chain_AH\n8=chain_root")]
        private GameObject[] uiObj_play_2;

        [SerializeField, Tooltip("判定UIオブジェクト\n0=H判定\n1=G判定\n2=S判定")]
        public GameObject[] uiObj_judge;

        #endregion
        #region リザルト画面（値,色更新に使用）
        [Header("リザルト画面")]
        [SerializeField, Tooltip
            ("値更新用UI\n0=スコア\n1=スコア_FC\n2=スコア_AH\n3=H判定\n4=G判定\n5=S判定\n6=M判定")]
        public TextMeshProUGUI[] tmp_result;

        [SerializeField, Tooltip("リザルト画面ジャケット画像")]
        public GameObject jacketImage_result;

        [SerializeField, Tooltip("プレイヤー名")]
        public Text text_playerName;

        [SerializeField, Tooltip
        ("UIオブジェクト\n0=RankD 1=RankC 2=RankB\n3=RankA 4=RankS\n5=score 6=score_FC 7=score_AH")]
        private GameObject[] uiObj_result;

        //[SerializeField, Tooltip
        //    ("色更新用UI\n0=ランク\n1=スコア値\n2=スコアテキスト")]
        //private Animator[] fixedText_result;

        #endregion
        void Start()
        {
            optionUI[0].SetActive(true);
            optionUI[1].SetActive(false);
            notesLineUI.SetActive(false);
            windowAnims[0].gameObject.SetActive(true);
            windowAnims[1].gameObject.SetActive(false);
            windowAnims[2].gameObject.SetActive(false);
            windowAnims[3].gameObject.SetActive(false);
            windowAnims[4].gameObject.SetActive(false);
            windowAnims[5].gameObject.SetActive(false);
        }
        #region アニメーション処理関数
        /// <summary>
        /// タイトル画面クローズ
        /// </summary>
        public void Close_title()
        {
            Debug.Log("アクティブ:Close_title");
            windowAnims[0].Play("close_title");
        }

        /// <summary>
        /// 選曲画面クローズ
        /// </summary>
        public void Close_select1()
        {
            Debug.Log("アクティブ:Close_select1");
            windowAnims[1].Play("close_select_1");
        }

        /// <summary>
        /// レベル選択画面クローズ
        /// </summary>
        public void Close_select2()
        {
            Debug.Log("アクティブ:Close_select2");
            windowAnims[2].Play("close_select_2");
            guideUI[0].SetActive(false);
            guideUI[1].SetActive(false);
            guideUI[2].SetActive(false);
            guideUI[3].SetActive(false);
        }

        /// <summary>
        /// 選曲画面に戻る
        /// </summary>
        public void Close_select2_return()
        {
            Debug.Log("アクティブ:Close_select2_return");
            windowAnims[2].Play("close_select_2r");
        }

        /// <summary>
        /// プレイ画面クローズ
        /// </summary>
        public void Close_play()
        {
            Debug.Log("アクティブ:Close_play");
            windowAnims[3].Play("close_play_1");
            windowAnims[4].Play("close_play_2");
        }

        /// <summary>
        /// リザルト画面クローズ
        /// </summary>
        public void Close_result()
        {
            Debug.Log("アクティブ:Close_result");
            windowAnims[5].Play("close_result");
        }

        /// <summary>
        /// 選曲画面1
        /// </summary>
        public void Open_select1()
        {
            Debug.Log("アクティブ:Open_select1");
            windowAnims[1].gameObject.SetActive(true);
            windowAnims[1].Play("open_select_1");
            guideUI[0].SetActive(true);
            guideUI[1].SetActive(false);
            guideUI[2].SetActive(true);
            guideUI[3].SetActive(false);
        }

        /// <summary>
        /// 選曲画面2
        /// </summary>
        public void Open_select2()
        {
            Debug.Log("アクティブ:Open_select2");
            windowAnims[2].gameObject.SetActive(true);
            windowAnims[2].Play("open_select_2");
            guideUI[0].SetActive(false);
            guideUI[1].SetActive(true);
            guideUI[2].SetActive(false);
            guideUI[3].SetActive(true);
        }

        public void Open_play()
        {
            Debug.Log("アクティブ:Open_play");
            windowAnims[3].gameObject.SetActive(true);
            windowAnims[3].Play("open_play_1");
            windowAnims[4].gameObject.SetActive(true);
            gameMng.SetUIData();
            windowAnims[4].Play("open_play_2");
        }

        public void Open_result()
        {
            Debug.Log("アクティブ:Open_result");
            windowAnims[5].gameObject.SetActive(true);
            if (playMng.ahFlag)
            {
                if (settingsMng.effectFlag)
                {
                    particleGenerator.GenerateParticle(90);
                }
                UIActive_chain(0);
            }
            else if (playMng.fcFlag)
            {
                if (settingsMng.effectFlag)
                {
                    particleGenerator.GenerateParticle(90);
                }
                UIActive_chain(1);
            }
            else
            {
                UIActive_chain(2);
            }
            windowAnims[5].Play("open_result");
        }
        #endregion

        /// <summary>
        /// レベルUIのアクティブ切替
        /// </summary>
        /// <param name="i">レベル</param>
        public void UIActive_level(int i)
        {
            Debug.Log("アクティブ:UIActive_level");
            switch (i)
            {
                case 0:
                    uiObj_play_1[0].SetActive(true);
                    uiObj_play_1[1].SetActive(false);
                    uiObj_play_1[2].SetActive(false);
                    break;
                case 1:
                    uiObj_play_1[0].SetActive(false);
                    uiObj_play_1[1].SetActive(true);
                    uiObj_play_1[2].SetActive(false);
                    break;
                case 2:
                    uiObj_play_1[0].SetActive(false);
                    uiObj_play_1[1].SetActive(false);
                    uiObj_play_1[2].SetActive(true);
                    break;
            }
        }

        /// <summary>
        /// ランクUIのアクティブ切替
        /// </summary>
        /// <param name="i">ランク</param>
        public void UIActive_rank(int i)
        {
            if (settingsMng.windowFlag != 3)
            {
                Debug.Log("アクティブ:UIActive_rank");
            }
            switch (i)
            {
                //rankS
                case 0:
                    uiObj_play_2[0].SetActive(false);
                    uiObj_play_2[1].SetActive(false);
                    uiObj_play_2[2].SetActive(false);
                    uiObj_play_2[3].SetActive(false);
                    uiObj_play_2[4].SetActive(true);
                    uiObj_result[0].SetActive(false);
                    uiObj_result[1].SetActive(false);
                    uiObj_result[2].SetActive(false);
                    uiObj_result[3].SetActive(false);
                    uiObj_result[4].SetActive(true);
                    break;
                //rankA
                case 1:
                    uiObj_play_2[0].SetActive(false);
                    uiObj_play_2[1].SetActive(false);
                    uiObj_play_2[2].SetActive(false);
                    uiObj_play_2[3].SetActive(true);
                    uiObj_play_2[4].SetActive(false);
                    uiObj_result[0].SetActive(false);
                    uiObj_result[1].SetActive(false);
                    uiObj_result[2].SetActive(false);
                    uiObj_result[3].SetActive(true);
                    uiObj_result[4].SetActive(false);
                    break;
                //rankB
                case 2:
                    uiObj_play_2[0].SetActive(false);
                    uiObj_play_2[1].SetActive(false);
                    uiObj_play_2[2].SetActive(true);
                    uiObj_play_2[3].SetActive(false);
                    uiObj_play_2[4].SetActive(false);
                    uiObj_result[0].SetActive(false);
                    uiObj_result[1].SetActive(false);
                    uiObj_result[2].SetActive(true);
                    uiObj_result[3].SetActive(false);
                    uiObj_result[4].SetActive(false);
                    break;
                //rankC
                case 3:
                    uiObj_play_2[0].SetActive(false);
                    uiObj_play_2[1].SetActive(true);
                    uiObj_play_2[2].SetActive(false);
                    uiObj_play_2[3].SetActive(false);
                    uiObj_play_2[4].SetActive(false);
                    uiObj_result[0].SetActive(false);
                    uiObj_result[1].SetActive(true);
                    uiObj_result[2].SetActive(false);
                    uiObj_result[3].SetActive(false);
                    uiObj_result[4].SetActive(false);
                    break;
                //rankD
                case 4:
                    uiObj_play_2[0].SetActive(true);
                    uiObj_play_2[1].SetActive(false);
                    uiObj_play_2[2].SetActive(false);
                    uiObj_play_2[3].SetActive(false);
                    uiObj_play_2[4].SetActive(false);
                    uiObj_result[0].SetActive(true);
                    uiObj_result[1].SetActive(false);
                    uiObj_result[2].SetActive(false);
                    uiObj_result[3].SetActive(false);
                    uiObj_result[4].SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// チェインUIのアクティブ切替
        /// </summary>
        /// <param name="i">フラグ</param>
        public void UIActive_chain(int i)
        {
            //Debug.Log("アクティブ:UIActive_chain");
            switch (i)
            {
                //AH
                case 0:
                    uiObj_play_2[5].SetActive(false);
                    uiObj_play_2[6].SetActive(false);
                    uiObj_play_2[7].SetActive(true);
                    uiObj_result[5].SetActive(false);
                    uiObj_result[6].SetActive(false);
                    //Debug.Log("AH");
                    uiObj_result[7].SetActive(true);
                    break;
                //FC
                case 1:
                    uiObj_play_2[5].SetActive(false);
                    uiObj_play_2[6].SetActive(true);
                    uiObj_play_2[7].SetActive(false);
                    uiObj_result[5].SetActive(false);
                    uiObj_result[6].SetActive(true);
                    //Debug.Log("FC");
                    uiObj_result[7].SetActive(false);
                    break;
                //フラグなし
                case 2:
                    uiObj_play_2[5].SetActive(true);
                    uiObj_play_2[6].SetActive(false);
                    uiObj_play_2[7].SetActive(false);
                    uiObj_result[5].SetActive(true);
                    uiObj_result[6].SetActive(false);
                    //Debug.Log("none");
                    uiObj_result[7].SetActive(false);
                    break;
            }
        }

        /// <summary>
        /// 値加算,リセット処理
        /// </summary>
        /// <param name="b">フラグ</param>
        public void UIAnim_value(bool b)
        {
            if (settingsMng.windowFlag != 3)
            {
                Debug.Log("アクティブ:UIAnim_value");
            }
            if (b)
            {
                uiObj_play_2[8].SetActive(true);
                fixedText_play_2.Play("addvalue", 0, 0);
            }
            else
            {
                uiObj_play_2[8].SetActive(false);
            }
        }

        //public void SetBPMGuide(int bpm)
        //{
        //    bpmGuideUI[0].speed = bpm / 60;
        //    bpmGuideUI[1].speed = bpm / 60;
        //    bpmGuideUI[0].Play("bpmGuide", 0, 0);
        //    bpmGuideUI[1].Play("bpmGuide", 0, 0);
        //}

        /// <summary>
        /// アニメーション終了後処理
        /// </summary>
        public void AnimEnd(int i)
        {
            switch (i)
            {
                case 0://アクティブ状態切替
                    Debug.Log("アニメーション0");
                    gameMng.DrumActive();
                    break;
                case 1://選曲画面オープン
                    Debug.Log("アニメーション1");
                    gameMng.SetUIData();
                    Open_select1();
                    break;
                case 2://レベル画面オープン
                    Debug.Log("アニメーション2");
                    gameMng.SetUIData();
                    Open_select2();
                    break;
                case 3://プレイ画面オープン
                    Debug.Log("アニメーション3");
                    optionUI[0].SetActive(false);
                    optionUI[1].SetActive(true);
                    notesLineUI.SetActive(true);
                    gameMng.SetUIData();
                    gameMng.GenerateNotes();
                    Open_play();
                    break;
                case 4://リザルト画面オープン,又はゲームスタート処理
                    if (settingsMng.windowFlag == 4)
                    {
                        Debug.Log("アニメーション4en");
                        optionUI[0].SetActive(true);
                        optionUI[1].SetActive(false);
                        notesLineUI.SetActive(false);
                        Open_result();
                    }
                    else
                    {
                        Debug.Log("アニメーション4st");
                        gameMng.DrumActive();
                        gameMng.PlayMusic();
                    }
                    break;
                default:
                    Debug.LogError("[<color=red>E104</color>]アニメ後処理値が不正です");
                    break;
            }
        }
    }
}