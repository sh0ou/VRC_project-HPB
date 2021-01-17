using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ゲーム処理用スクリプト
    /// </summary>
    public class GameManager : UdonSharpBehaviour
    {
        #region 変数
        [SerializeField, Tooltip("プレイマネージャ")]
        private PlayManager playMng;

        [SerializeField, Tooltip("設定マネージャ")]
        private SettingsManager settingsMng;

        [SerializeField, Tooltip("UIマネージャ")]
        private UIManager uiMng;

        [SerializeField, Tooltip("テキスト変換スクリプト")]
        private TextFileConverter txtConverter;

        [SerializeField, Tooltip("ノーツジェネレータ")]
        private NotesGenerator notesGen;

        [SerializeField, Tooltip("ノーツ判定スクリプト")]
        private NotesJudger_V2 notesJudger;

        [SerializeField, Tooltip("サウンドマネージャ")]
        private SoundManager soundMng;

        [SerializeField, Tooltip("ドラム処理フラグ（falseだと判定が無視される）")]
        private bool drumActive;

        [SerializeField, Tooltip("選択中の曲番号")]
        private int selectMusicNum;

        [SerializeField, Tooltip("選択中の曲レベル")]
        private int selectLevelNum;

        #endregion
        void Start()
        {
            drumActive = true;
        }

        private void Update()
        {
            #region キーボード入力
            if (Input.GetKeyDown(KeyCode.F))
            {
                DrumAction(1);
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                DrumAction(2);
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                DrumAction(3);
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                DrumAction(4);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                DrumAction(0);
            }
            #endregion

            if (settingsMng.gamePlay)
            {
                playMng.playTime += Time.deltaTime;
                //楽曲時間がプレイ時間を超えると終了
                if (playMng.playTime >= playMng.endTime)
                {
                    EndMusic();
                }
            }
        }

        /// <summary>
        /// ドラム処理
        /// </summary>
        /// <param name="i">叩かれたドラム</param>
        public void DrumAction(int i)
        {
            Debug.Log("[<color=yellow>GameManager</color>]ドラム判定" + i);
            #region 処理メモ
            /*
            ドラム番号取得
            ゲーム状態取得
            タイトル画面の場合
                タイトルアニメ再生
            選曲画面の場合
                曲移動処理
                    基本情報更新
                    ジャケット更新（+1-1のジャケットも更新）
            レベル選択画面の場合
                レベル移動処理、譜面生成処理
            プレイ画面の場合
                判定処理
            リザルト画面の場合
                選曲画面遷移処理
            */
            #endregion
            //処理待ち対策
            if (drumActive)
            {
                //if (settingsMng.windowFlag != 3)
                //{
                //    Debug.Log("アクティブ:DrumAction");
                //}
                switch (settingsMng.windowFlag)
                {
                    case 0:
                        //ゲーム開始
                        if (i == 0)
                        {
                            drumActive = false;
                            settingsMng.windowFlag = 1;
                            uiMng.Close_title();
                        }
                        break;
                    case 1:
                        #region 処理
                        //左に移動
                        if (i == 1 || i == 2)
                        {
                            if (selectMusicNum == 0)
                            {
                                selectMusicNum = txtConverter.SendMusicLength();
                            }
                            else
                            {
                                selectMusicNum -= 1;
                            }
                            SetUIData();
                        }
                        //右に移動
                        else if (i == 3 || i == 4)
                        {
                            if (selectMusicNum == txtConverter.SendMusicLength())
                            {
                                selectMusicNum = 0;
                            }
                            else
                            {
                                selectMusicNum += 1;
                            }
                            SetUIData();
                        }
                        //楽曲決定
                        else if (i == 0)
                        {
                            drumActive = false;
                            settingsMng.windowFlag = 2;
                            uiMng.Close_select1();
                        }
                        break;
                    #endregion
                    case 2:
                        #region 処理
                        //楽曲選択に戻る
                        if (i == 1 || i == 4)
                        {
                            drumActive = false;
                            settingsMng.windowFlag = 1;
                            uiMng.Close_select2_return();
                        }
                        //レベルを下げる
                        else if (i == 2)
                        {
                            if (selectLevelNum != 0)
                            {
                                selectLevelNum -= 1;
                                SetUIData();
                            }
                        }
                        //レベルを上げる
                        else if (i == 3)
                        {
                            if (selectLevelNum != 2)
                            {
                                selectLevelNum += 1;
                                SetUIData();
                            }
                        }
                        //プレイ開始
                        else if (i == 0)
                        {
                            drumActive = false;
                            playMng.fcFlag = true;
                            playMng.ahFlag = true;
                            settingsMng.windowFlag = 3;
                            uiMng.Close_select2();
                        }
                        break;
                    #endregion
                    case 3:
                        //判定処理
                        JudgeNotes(i);
                        break;
                    case 4:
                        //楽曲選択に戻る
                        drumActive = false;
                        settingsMng.windowFlag = 1;
                        uiMng.Close_result();
                        break;
                    default:
                        Debug.LogError("[<color=red>HPB_GameManager</color>]画面番号が不正です");
                        break;
                }
            }
        }

        /// <summary>
        /// ドラム処理を可能にする
        /// </summary>
        public void DrumActive()
        {
            //Debug.Log("アクティブ:DrumActive");
            drumActive = true;
        }

        /// <summary>
        /// ノーツ生成処理
        /// </summary>
        public void GenerateNotes()
        {
            //Debug.Log("アクティブ:GenerateNotes");
            SetUI_level();
            SetUIData();
            notesJudger.Setup();
            notesGen.SetNotes();

            SetCalcValue();
        }

        /// <summary>
        /// 楽曲再生処理
        /// </summary>
        public void PlayMusic()
        {
            //Debug.Log("アクティブ:PlayMusic");
            soundMng.audioSources[0].clip = soundMng.bgmLists[0];
            soundMng.audioSources[0].Play();
            //uiMng.SetBPMGuide(int.Parse(txtConverter.textDB[0][2]));
            playMng.endTime = float.Parse(txtConverter.textDB[0][4]);
            settingsMng.gamePlay = true;
        }

        /// <summary>
        /// UI情報をセット
        /// </summary>
        public void SetUIData()
        {
            //if (settingsMng.windowFlag != 3)
            //{
            //    Debug.Log("アクティブ:SetUIData");
            //}
            txtConverter.SetTextFile(selectMusicNum, selectLevelNum);
            Debug.Log("窓処理:" + settingsMng.windowFlag);
            //開いてる窓によって処理を変更
            switch (settingsMng.windowFlag)
            {
                case 0:
                    break;
                case 1:
                    #region 選曲画面処理
                    //値をセット
                    uiMng.inputFields_select_1[0].text = txtConverter.textDB[0][0];
                    uiMng.inputFields_select_1[1].text = txtConverter.textDB[0][1];

                    //中央画像セット
                    uiMng.jacketImage_select_1[0].GetComponent<Renderer>().material =
                        uiMng.jacketList[selectMusicNum][0];

                    //左画像セット

                    if ((selectMusicNum - 1) == -1)
                    {
                        uiMng.jacketImage_select_1[1].GetComponent<Renderer>().material =
                            uiMng.jacketList[txtConverter.SendMusicLength()][0];
                    }
                    else
                    {
                        uiMng.jacketImage_select_1[1].GetComponent<Renderer>().material =
                            uiMng.jacketList[selectMusicNum - 1][0];
                    }

                    //右画像セット

                    if (selectMusicNum == txtConverter.SendMusicLength())
                    {
                        uiMng.jacketImage_select_1[2].GetComponent<Renderer>().material =
                            uiMng.jacketList[0][0];
                    }
                    else
                    {
                        uiMng.jacketImage_select_1[2].GetComponent<Renderer>().material =
                            uiMng.jacketList[selectMusicNum + 1][0];
                    }


                    #endregion
                    break;
                case 2:
                    #region レベル画面処理
                    //値をセット
                    uiMng.inputFields_select_2[0].text = txtConverter.textDB[0][0];
                    uiMng.inputFields_select_2[1].text = txtConverter.textDB[0][1];
                    uiMng.jacketImage_select_2.GetComponent<Renderer>().material =
                        uiMng.jacketList[selectMusicNum][selectLevelNum];
                    //レベル値のセット
                    txtConverter.SetTextFile(selectMusicNum, 0);
                    uiMng.inputFields_select_2[2].text = txtConverter.textDB[0][3];
                    txtConverter.SetTextFile(selectMusicNum, 1);
                    uiMng.inputFields_select_2[3].text = txtConverter.textDB[0][3];
                    txtConverter.SetTextFile(selectMusicNum, 2);
                    uiMng.inputFields_select_2[3].text = txtConverter.textDB[0][3];
                    #endregion
                    break;
                case 3:
                    #region プレイ画面処理
                    //値をセット
                    uiMng.inputFields_play_1[0].text = txtConverter.textDB[0][0];
                    uiMng.inputFields_play_1[1].text = txtConverter.textDB[0][1];
                    uiMng.inputFields_play_1[2].text = txtConverter.textDB[0][3];
                    uiMng.inputFields_play_1[3].text = txtConverter.textDB[0][3];
                    uiMng.inputFields_play_1[4].text = txtConverter.textDB[0][3];
                    uiMng.inputFields_play_2[0].text = playMng.score_now.ToString();
                    uiMng.inputFields_play_2[1].text = playMng.score_now.ToString();
                    uiMng.inputFields_play_2[2].text = playMng.score_now.ToString();
                    uiMng.inputFields_play_2[3].text = playMng.score_now.ToString();
                    uiMng.inputFields_play_2[4].text = playMng.score_now.ToString();
                    uiMng.inputFields_play_2[5].text = playMng.chain.ToString();
                    uiMng.inputFields_play_2[6].text = playMng.chain.ToString();
                    uiMng.inputFields_play_2[7].text = playMng.chain.ToString();
                    //レベル種類のセット
                    SetUI_score();
                    SetUI_chainFlag();
                    #endregion
                    break;
                case 4:
                    #region リザルト画面処理
                    //値をセット
                    SetUI_score();
                    SetUI_chainFlag();
                    //Debug.LogError("[<color=red>HPB_GameManager</color>]DisplayNameがコメントアウトされてます\nアップロード時は解除してください");
                    //uiMng.text_playerName.text = Networking.GetOwner(playMng.drumStick).displayName;
                    uiMng.text_playerName.text = "ななしの楽団員";
                    uiMng.valueFields_result[0].text = playMng.score_now.ToString();
                    uiMng.valueFields_result[1].text = playMng.score_now.ToString();
                    uiMng.valueFields_result[2].text = playMng.score_now.ToString();
                    uiMng.valueFields_result[3].text = playMng.judgedValue[0].ToString();
                    uiMng.valueFields_result[4].text = playMng.judgedValue[1].ToString();
                    uiMng.valueFields_result[5].text = playMng.judgedValue[2].ToString();
                    uiMng.valueFields_result[6].text = playMng.judgedValue[3].ToString();
                    uiMng.jacketImage_result.GetComponent<Renderer>().material =
                        uiMng.jacketList[selectMusicNum][selectLevelNum];
                    #endregion
                    break;
                default:
                    Debug.LogError("[<color=red>HPB_GameManager</color>]窓番号が不正です");
                    break;
            }
        }

        /// <summary>
        /// レベルUIをセット
        /// </summary>
        private void SetUI_level()
        {
            //Debug.Log("アクティブ:SetUI_level");
            uiMng.UIActive_level(selectLevelNum);
        }

        /// <summary>
        /// ランク,スコアUIをセット
        /// </summary>
        private void SetUI_score()
        {
            if (settingsMng.windowFlag != 3)
            {
                //Debug.Log("アクティブ:SetUI_score");
            }
            //rankS
            if (playMng.score_now >= 95000)
            {
                uiMng.UIActive_rank(0);
            }
            //rankA
            else if (playMng.score_now >= 90000)
            {
                uiMng.UIActive_rank(1);
            }
            //rankB
            else if (playMng.score_now >= 80000)
            {
                uiMng.UIActive_rank(2);
            }
            //rankC
            else if (playMng.score_now >= 70000)
            {
                uiMng.UIActive_rank(3);
            }
            //rankD
            else
            {
                uiMng.UIActive_rank(4);
            }
        }

        /// <summary>
        /// AH,FCフラグをセット
        /// </summary>
        private void SetUI_chainFlag()
        {
            //if (settingsMng.windowFlag != 3)
            //{
            //    Debug.Log("アクティブ:SetUI_chainFlag");
            //}
            //5チェイン以上のみ表示
            if (playMng.chain >= 5)
            {
                if (playMng.ahFlag)
                {
                    uiMng.UIActive_chain(0);
                }
                else if (playMng.fcFlag)
                {
                    uiMng.UIActive_chain(1);
                }
                else
                {
                    uiMng.UIActive_chain(2);
                }
            }
        }

        /// <summary>
        /// スコア計算用値をセット
        /// </summary>
        private void SetCalcValue()
        {
            Debug.Log("アクティブ:SetCalcValue");
            playMng.score_now = 0;
            playMng.playTime = 0;
            playMng.chain = 0;
            playMng.judgedValue = new int[4] { 0, 0, 0, 0 };
            playMng.scoreCalcValue[0] = playMng.notesValue;
            playMng.scoreCalcValue[1] = (100000 * 1) / playMng.scoreCalcValue[0];
        }

        /// <summary>
        /// ノーツ判定処理
        /// </summary>
        /// <param name="i">レーン</param>
        public void JudgeNotes(int i)
        {
            //Debug.Log("[<color=yellow>GameManager</color>]レーン" + i);
            int judge_v = notesJudger.Judge(i);
            //int judge_i = judge_v;
            //bool judge_b = judge_v.Item2;
            //if (judge_v)
            //{
            //    drumActive = false;
            //}
            //Debug.Log("[<color=yellow>GameManager</color>]判定結果:" + judge_v);
            if (notesJudger.IsAllNotesJudged())
            {
                drumActive = false;
            }
            if (judge_v == 1)
            {
                playMng.judgedValue[0] += 1;
                playMng.chain += 1;
                //理論値処理
                if (playMng.judgedValue[0] != playMng.notesValue)
                {
                    playMng.score_now += playMng.scoreCalcValue[1];
                }
                else
                {
                    playMng.score_now = 100000;
                }
                uiMng.UIAnim_value(true);
                GameObject g = VRCInstantiate(uiMng.uiObj_judge[0]);
                g.GetComponent<JudgeTextObj>().judgeValue = i;
                if (i == 0)
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
                }
                else
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[0]);
                }
            }
            else if (judge_v == 2)
            {
                playMng.judgedValue[1] += 1;
                playMng.chain += 1;
                playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.8f);
                playMng.ahFlag = false;
                uiMng.UIAnim_value(true);
                GameObject g = VRCInstantiate(uiMng.uiObj_judge[1]);
                g.GetComponent<JudgeTextObj>().judgeValue = i;
                if (i == 0)
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
                }
                else
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[0]);
                }
            }
            else if (judge_v == 3)
            {
                playMng.judgedValue[2] += 1;
                playMng.chain = 0;
                playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.5f);
                playMng.ahFlag = false;
                playMng.fcFlag = false;
                uiMng.UIAnim_value(false);
                GameObject g = VRCInstantiate(uiMng.uiObj_judge[2]);
                g.GetComponent<JudgeTextObj>().judgeValue = i;
            }
            Update_UIData();
            #region Archive
            //if (settingsMng.windowFlag != 3)
            //{
            //    Debug.Log("アクティブ:JudgeNotes");
            //}
            ///*
            //範囲外は無視
            //判定時間より極端に早い場合は無視
            //（スピード調整による誤動作防止）
            //Sad判定の場合はフラグ全オフ
            //    とChainリセット
            //Good判定の場合はAHフラグをオフ
            //Happy,Good判定の場合はChain加算

            //レーン内のノーツを探して差を計算？
            //*/
            //if (i == -1)
            //{
            //    //M判定
            //    playMng.judgedValue[3] += 1;
            //    playMng.chain = 0;
            //    playMng.ahFlag = false;
            //    playMng.fcFlag = false;
            //    uiMng.UIAnim_value(false);
            //}
            //else
            //{
            //    int judgeValue = notesJudgers[i].Judge(i);
            //    //H判定
            //    if (judgeValue == 0)
            //    {
            //        playMng.judgedValue[0] += 1;
            //        playMng.chain += 1;
            //        //理論値処理
            //        if (playMng.score_now < (100000 - playMng.scoreCalcValue[1]))
            //        {
            //            playMng.score_now += playMng.scoreCalcValue[1];
            //        }
            //        else
            //        {
            //            playMng.score_now = 100000;
            //        }
            //        uiMng.UIAnim_value(true);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[0]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }
            //    //G判定
            //    else if (judgeValue == 1)
            //    {
            //        playMng.judgedValue[1] += 1;
            //        playMng.chain += 1;
            //        playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.6f);
            //        playMng.ahFlag = false;
            //        uiMng.UIAnim_value(true);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[1]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }
            //    //S判定
            //    else if(judgeValue == 2)
            //    {
            //        playMng.judgedValue[2] += 1;
            //        playMng.chain = 0;
            //        playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.2f);
            //        playMng.ahFlag = false;
            //        playMng.fcFlag = false;
            //        uiMng.UIAnim_value(false);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[2]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }

            //    //H判定
            //    if (Mathf.Abs(playMng.playTime - 0) <= 1f)
            //    {
            //        playMng.judgedValue[0] += 1;
            //        playMng.chain += 1;
            //        //理論値処理
            //        if (playMng.score_now < (100000 - playMng.scoreCalcValue[1]))
            //        {
            //            playMng.score_now += playMng.scoreCalcValue[1];
            //        }
            //        else
            //        {
            //            playMng.score_now = 100000;
            //        }
            //        uiMng.UIAnim_value(true);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[0]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }
            //    //G判定
            //    else if (Mathf.Abs(playMng.playTime - 0) <= 1f)
            //    {
            //        playMng.judgedValue[1] += 1;
            //        playMng.chain += 1;
            //        playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.6f);
            //        playMng.ahFlag = false;
            //        uiMng.UIAnim_value(true);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[1]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }
            //    //S判定
            //    else
            //    {
            //        playMng.judgedValue[2] += 1;
            //        playMng.chain = 0;
            //        playMng.score_now += Mathf.RoundToInt(playMng.scoreCalcValue[1] * 0.2f);
            //        playMng.ahFlag = false;
            //        playMng.fcFlag = false;
            //        uiMng.UIAnim_value(false);
            //        GameObject g = VRCInstantiate(uiMng.uiObj_judge[2]);
            //        g.GetComponent<JudgeTextObj>().judgeValue = i;
            //    }
            //}
            //SetUIData();
            //SetUI_score();
            //SetUI_chainFlag();
            #endregion
        }

        public void Judge_miss(int laneIndex)
        {
            playMng.judgedValue[3] += 1;
            playMng.chain = 0;
            playMng.ahFlag = false;
            playMng.fcFlag = false;
            uiMng.UIAnim_value(false);
            GameObject g = VRCInstantiate(uiMng.uiObj_judge[3]);
            g.GetComponent<JudgeTextObj>().judgeValue = laneIndex;
            //データ更新
            Update_UIData();
        }

        private void Update_UIData()
        {
            SetUIData();
            SetUI_score();
            SetUI_chainFlag();
        }

        /// <summary>
        /// 楽曲終了時処理
        /// </summary>
        public void EndMusic()
        {
            Debug.Log("アクティブ:EndMusic");
            //SoundManagerで再生されている楽曲が終了すると発火
            settingsMng.gamePlay = false;
            soundMng.audioSources[0].Stop();
            settingsMng.windowFlag = 4;
            notesGen.DisableRootObj();
            playMng.playTime = 0;
            //全ミス状態の場合フラグを外す
            if (playMng.score_now == 0)
            {
                playMng.ahFlag = false;
                playMng.fcFlag = false;
            }
            int missNotes = playMng.notesValue - notesJudger.totalJudgedNotes;
            playMng.judgedValue[3] += missNotes;
            Update_UIData();
            uiMng.Close_play();
        }
    }
}