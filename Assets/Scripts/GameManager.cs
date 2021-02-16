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

        [SerializeField, Tooltip("パーティクルジェネレータ")]
        private ParticleGenerator particleGenerator;

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
            if (settingsMng.gamePlay)
            {
                playMng.playTime = soundMng.audioSources[0].time;
                //楽曲時間がプレイ時間を超えると終了
                if (playMng.playTime >= playMng.endTime)
                {
                    EndMusic();
                }
            }
            if (settingsMng.keyboardFlag)
            {
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
            }
        }

        /// <summary>
        /// ドラム処理
        /// </summary>
        /// <param name="i">叩かれたドラム</param>
        public void DrumAction(int i)
        {
            //処理待ち対策
            if (drumActive)
            {
                switch (i)
                {
                    case 0:
                        particleGenerator.GenerateParticle(10);
                        break;
                    case 5:
                        particleGenerator.GenerateParticle(15);
                        break;
                    case 1:
                        particleGenerator.GenerateParticle(11);
                        break;
                    case 2:
                        particleGenerator.GenerateParticle(12);
                        break;
                    case 3:
                        particleGenerator.GenerateParticle(13);
                        break;
                    case 4:
                        particleGenerator.GenerateParticle(14);
                        break;
                }
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
            notesGen.GenerateNotes();
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
            playMng.endTime = float.Parse(txtConverter.textDB[0][4]);
            uiMng.StartBPMGuide(float.Parse(txtConverter.textDB[0][2]) / 60);
            settingsMng.gamePlay = true;
        }

        /// <summary>
        /// UI情報をセット
        /// </summary>
        public void SetUIData()
        {
            txtConverter.SetTextFile(selectMusicNum, selectLevelNum);
            //開いてる窓によって処理を変更
            switch (settingsMng.windowFlag)
            {
                case 0:
                    break;
                case 1:
                    #region 選曲画面処理
                    //値をセット
                    uiMng.tmp_select_1[0].text = txtConverter.textDB[0][0];
                    uiMng.tmp_select_1[1].text = txtConverter.textDB[0][1];

                    //中央画像セット
                    uiMng.jacketImage_select_1[0].GetComponent<Renderer>().material =
                        uiMng.jacketList[selectMusicNum][0];

                    //左画像セット

                    uiMng.jacketImage_select_1[1].GetComponent<Renderer>().material =
                        (selectMusicNum - 1) == -1
                        ? uiMng.jacketList[txtConverter.SendMusicLength()][0]
                        : uiMng.jacketList[selectMusicNum - 1][0];

                    //右画像セット

                    uiMng.jacketImage_select_1[2].GetComponent<Renderer>().material =
                        selectMusicNum == txtConverter.SendMusicLength()
                        ? uiMng.jacketList[0][0]
                        : uiMng.jacketList[selectMusicNum + 1][0];

                    #endregion
                    break;
                case 2:
                    #region レベル画面処理
                    //値をセット
                    uiMng.tmp_select_2[0].text = txtConverter.textDB[0][0];
                    uiMng.tmp_select_2[1].text = txtConverter.textDB[0][1];
                    uiMng.jacketImage_select_2.GetComponent<Renderer>().material =
                        uiMng.jacketList[selectMusicNum][selectLevelNum];
                    //レベル値のセット
                    txtConverter.SetTextFile(selectMusicNum, 0);
                    uiMng.tmp_select_2[2].text = txtConverter.textDB[0][3];
                    txtConverter.SetTextFile(selectMusicNum, 1);
                    uiMng.tmp_select_2[3].text = txtConverter.textDB[0][3];
                    txtConverter.SetTextFile(selectMusicNum, 2);
                    uiMng.tmp_select_2[3].text = txtConverter.textDB[0][3];
                    #endregion
                    break;
                case 3:
                    #region プレイ画面処理
                    //値をセット
                    uiMng.tmp_play_1[0].text = txtConverter.textDB[0][0];
                    uiMng.tmp_play_1[1].text = txtConverter.textDB[0][1];
                    uiMng.tmp_play_1[2].text = txtConverter.textDB[0][3];
                    uiMng.tmp_play_1[3].text = txtConverter.textDB[0][3];
                    uiMng.tmp_play_1[4].text = txtConverter.textDB[0][3];
                    uiMng.tmp_play_2[0].text = playMng.score_now.ToString();
                    uiMng.tmp_play_2[1].text = playMng.score_now.ToString();
                    uiMng.tmp_play_2[2].text = playMng.score_now.ToString();
                    uiMng.tmp_play_2[3].text = playMng.score_now.ToString();
                    uiMng.tmp_play_2[4].text = playMng.score_now.ToString();
                    uiMng.tmp_play_2[5].text = playMng.chain.ToString();
                    uiMng.tmp_play_2[6].text = playMng.chain.ToString();
                    uiMng.tmp_play_2[7].text = playMng.chain.ToString();
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
                    uiMng.text_playerName.text = Networking.GetOwner(playMng.drumStick).displayName;
                    uiMng.text_playerName.text = "ななしの楽団員";
                    uiMng.tmp_result[0].text = playMng.score_now.ToString();
                    uiMng.tmp_result[1].text = playMng.score_now.ToString();
                    uiMng.tmp_result[2].text = playMng.score_now.ToString();
                    uiMng.tmp_result[3].text = playMng.judgedValue[0].ToString();
                    uiMng.tmp_result[4].text = playMng.judgedValue[1].ToString();
                    uiMng.tmp_result[5].text = playMng.judgedValue[2].ToString();
                    uiMng.tmp_result[6].text = playMng.judgedValue[3].ToString();
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
            //if (settingsMng.windowFlag != 3)
            //{
            //    Debug.Log("アクティブ:SetUI_score");
            //}
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
            //Debug.Log("アクティブ:SetCalcValue");
            playMng.score_now = 0;
            playMng.playTime = 0;
            playMng.chain = 0;
            playMng.judgedValue = new int[4] { 0, 0, 0, 0 };
            playMng.scoreCalcValue[0] = playMng.notesValue;
            playMng.scoreCalcValue[1] = (100000 * 1) / playMng.scoreCalcValue[0];
            Debug.Log("Calc:" + playMng.scoreCalcValue[1]);
        }

        /// <summary>
        /// ノーツ判定処理
        /// </summary>
        /// <param name="i">レーン</param>
        public void JudgeNotes(int i)
        {
            //Debug.Log("[<color=yellow>GameManager</color>]レーン" + i);
            int judge_v = notesJudger.Judge(i);
            if (notesJudger.IsAllNotesJudged())
            {
                drumActive = false;
            }
            if (judge_v == 1)//Happy
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

                //効果再生
                if (i == 0)
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[1]);
                    if (settingsMng.effectFlag)
                    {
                        particleGenerator.GenerateParticle(i);
                    }
                }
                else
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[0]);
                    if (settingsMng.effectFlag)
                    {
                        particleGenerator.GenerateParticle(i);
                    }
                }
            }
            else if (judge_v == 2)//Good
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
                    if (settingsMng.effectFlag)
                    {
                        particleGenerator.GenerateParticle(i);
                    }
                }
                else
                {
                    soundMng.audioSources[1].PlayOneShot(soundMng.seLists[0]);
                    if (settingsMng.effectFlag)
                    {
                        particleGenerator.GenerateParticle(i);
                    }
                }
            }
            else if (judge_v == 3)//Sad
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
            uiMng.StopBPMGuide();
            uiMng.Close_play();
        }
    }
}