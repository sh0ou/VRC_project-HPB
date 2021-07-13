using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace HPB
{
    /// <summary>
    /// デバッグウインドウ管理スクリプト
    /// </summary>
    public class DebugManager : UdonSharpBehaviour
    {
        [SerializeField] GameManager gameManager;
        [SerializeField] PlayManager playManager;
        [SerializeField] SettingsManager settingsManager;
        [SerializeField] SyncManager syncManager;
        [SerializeField] TextFileConverter textFileConverter;
        [SerializeField] NotesGenerator notesGenerator;
        [SerializeField] NotesJudger_V2 notesJudger;
        [SerializeField] NotesObj notesObj_0;
        [SerializeField] NotesObj notesObj_1;

        [SerializeField] Animation_Drum animation_Drum;
        [SerializeField] Animation_Chain animation_Chain;
        [SerializeField] GenerateJudgeText generateJudgeText;
        [SerializeField] ChangePlayer changePlayer;
        [SerializeField] SoundPlayer soundPlayer;
        [SerializeField] MusicAnimator musicAnimator;

        [SerializeField] GameObject debugWindow;
        [SerializeField] GameObject debugWindow_b;
        [SerializeField] TextMeshProUGUI[] tmp_Debug;
        [SerializeField] TextMeshProUGUI[] tmp_Debug_b;

        void Start()
        {
            //gameManager = GetComponent<GameManager>();
            //playManager = GetComponent<PlayManager>();
            //settingsManager = GetComponent<SettingsManager>();
            //syncManager = GetComponent<SyncManager>();
            //textFileConverter = GameObject.Find("TextFileConverter").GetComponent<TextFileConverter>();
            //notesGenerator = GameObject.Find("NotesGenerator").GetComponent<NotesGenerator>();
            //notesJudger = GameObject.Find("NotesJudger").GetComponent<NotesJudger_V2>();
        }

        void Update()
        {
            if (settingsManager.debugFlag)
            {
                //Sync変数の表示
                tmp_Debug[0].text = "selectMusicNum: " + gameManager.GetMusicNum();
                tmp_Debug[1].text = "selectLevelNum: " + gameManager.GetLevelNum();
                tmp_Debug[2].text = "score_now: " + playManager.score_now;
                tmp_Debug[3].text = "chain: " + playManager.chain;
                tmp_Debug[4].text = "judgedValue_H: " + playManager.judgedValue[0];
                tmp_Debug[5].text = "judgedValue_G: " + playManager.judgedValue[1];
                tmp_Debug[6].text = "judgedValue_S: " + playManager.judgedValue[2];
                tmp_Debug[7].text = "judgedValue_M: " + playManager.judgedValue[3];
                tmp_Debug[8].text = "fcFlag: " + playManager.fcFlag;
                tmp_Debug[9].text = "ahFlag: " + playManager.ahFlag;
                tmp_Debug[10].text = "<color=yellow>windowFlag: " + settingsManager.windowFlag + "</yellow>";
                tmp_Debug[11].text = "targetLane: " + syncManager.targetlane;
                tmp_Debug[12].text = "targetid_a: " + syncManager.targetid_a;
                tmp_Debug[13].text = "targetid_b: " + syncManager.targetid_b;
                tmp_Debug[14].text = "targetTextid_a: " + syncManager.targetTextid_a;
                tmp_Debug[15].text = "targetTextid_b: " + syncManager.targetTextid_b;
                tmp_Debug[16].text = "targetBool: " + syncManager.targetBool;
                tmp_Debug[17].text = "totalJudgedNotes: " + notesJudger.totalJudgedNotes;
                //Local変数の表示
                tmp_Debug[18].text = "isActivePlayer: " + syncManager.isActivePlayer;
                tmp_Debug[19].text = "gameplay: " + settingsManager.gamePlay;
                //tmp_Debug[20].text = "musicAnimTime: " + musicAnimator.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                //tmp_Debug[20].text = "musicAnimTime: " + musicAnimator.animator.GetCurrentAnimatorStateInfo(0).
                //tmp_Debug[20].text = "playtime: " + playManager.playTime;
                //tmp_Debug[21].text = "endTime: " + playManager.endTime;

                //Nullチェック
                tmp_Debug_b[0].text = "Animation_Drum: " + (animation_Drum.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[1].text = "Animation_Chain: " + (animation_Chain.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[2].text = "GenerateJudgeText: " + (generateJudgeText.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[3].text = "ChangePlayer: " + (changePlayer.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[4].text = "SoundPlayer: " + (soundPlayer.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[5].text = "SyncManager: " + (syncManager.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[6].text = "NotesJudger: " + (notesJudger.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[7].text = "NotesObj_0: " + (notesObj_0.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                tmp_Debug_b[8].text = "NotesObj_1: " + (notesObj_1.enabled ? "<color=green>OK</color>" : "<color=red>null<color=green>");
                debugWindow.SetActive(true);
                debugWindow_b.SetActive(true);
            }
            else
            {
                debugWindow.SetActive(false);
                debugWindow_b.SetActive(false);
            }
        }
    }
}