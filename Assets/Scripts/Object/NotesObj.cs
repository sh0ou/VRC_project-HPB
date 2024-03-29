﻿using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// ノーツオブジェクト
    /// </summary>
    public class NotesObj : UdonSharpBehaviour
    {
        private NotesJudger_V2 notesJudger;
        private NotesGenerator notesGenerator;
        private SettingsManager settingsMng;
        private PlayManager playMng;
        private MeshRenderer meshRenderer;
        private SyncManager syncManager;
        private Vector2 vec2; //位置更新用Vector2

        [SerializeField, Tooltip("ノーツの参照番号\n0=レーン\n1=要素番号(レーン内)\n2=要素番号(単体)")]
        public int[] notesReferenceNo = new int[3];

        void Start()
        {
            notesJudger = GameObject.Find("NotesJudger").GetComponent<NotesJudger_V2>();
            notesGenerator = GameObject.Find("NotesGenerator").GetComponent<NotesGenerator>();
            settingsMng = GameObject.Find("GameManager").GetComponent<SettingsManager>();
            playMng = GameObject.Find("GameManager").GetComponent<PlayManager>();
            syncManager = GameObject.Find("GameManager").GetComponent<SyncManager>();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
        }
        void Update()
        {
            if (settingsMng.gamePlay)
            {
                //位置を更新
                if (meshRenderer.enabled)
                {
                    //Debug.Log("[<color=yellow>NotesObj</color>]位置更新を開始:" + notesReferenceNo[0] + "/" + notesReferenceNo[1]);
                    vec2 = notesGenerator.GetNotesPosValue_xy(notesReferenceNo[0]);
                    float f = notesGenerator.GetNotesPosValue_z(notesReferenceNo[0], notesReferenceNo[1]);
                    transform.position = new Vector3(vec2.x, vec2.y, f);
                }
                //sad判定の範囲外に出たらミスとして判定
                if (syncManager.isActivePlayer)
                {
                    if (playMng.playTime > notesJudger.noteTimeList[notesReferenceNo[0]][notesReferenceNo[1]] +
                        notesJudger.noteJudgeTimeSpanList[3])
                    {
                        Debug.Log("[<color=yellow>NotesObj</color>]Miss判定:" + gameObject.name);
                        notesJudger.Judge_miss(notesReferenceNo[0], notesReferenceNo[1]);
                    }
                    //判定済みの場合は消去
                    if (notesJudger.noteJudgeResultsList[notesReferenceNo[0]][notesReferenceNo[1]] != 0)
                    {
                        //Debug.Log("[<color=yellow>NotesObj</color>]開始:Mesh消去処理");

                        //MeshRenderer表示,非表示処理（10以上を非表示化）
                        //Debug.Log("EnableCheck : " + (notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes) + "/to : " + gameObject.name);
                        if ((notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes) > 9)
                        {
                            if (notesGenerator.notesObjInstance[notesReferenceNo[2] + 10] == null)
                            {
                                Debug.Log("[<color=red>NotesObj</color>]対象Meshがnullのため、処理が実行できませんでした");
                            }
                            else
                            {
                                notesGenerator.notesObjInstance[notesReferenceNo[2] + 10].GetComponent<MeshRenderer>().enabled = true;
                                Debug.Log("[<color=yellow>NotesObj</color>]完了:Mesh消去処理");
                            }
                            //Debug.Log("ReferenceCheck : " + notesGenerator.notesObjInstance[notesReferenceNo[2] + 10].name);
                        }
                        //else if (notesJudger.totalPlacedNotes != notesJudger.totalJudgedNotes)
                        //{
                        //Debug.Log("10ノーツ以下の処理");
                        //Debug.Log("ReferenceCheck : " + notesGenerator.notesObjInstance[notesReferenceNo[2] + (notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes)].name);
                        //notesGenerator.notesObjInstance[notesReferenceNo[2] + (notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes)]
                        //    .GetComponent<MeshRenderer>().enabled = true;
                        //}
                        gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (notesReferenceNo[2] < notesJudger.totalJudgedNotes)
                    {
                        //Debug.Log("[<color=yellow>NotesObj</color>]開始:Mesh消去処理_Guest");
                        if ((notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes) > 9)
                        {
                            if (notesGenerator.notesObjInstance[notesReferenceNo[2] + 10] == null)
                            {
                                Debug.Log("[<color=red>NotesObj</color>]対象Meshがnullのため、処理が実行できませんでした");
                            }
                            else
                            {
                                notesGenerator.notesObjInstance[notesReferenceNo[2] + 10].GetComponent<MeshRenderer>().enabled = true;
                                Debug.Log("[<color=yellow>NotesObj</color>]完了:Mesh消去処理_Guest");
                            }
                            //Debug.Log("ReferenceCheck : " + notesGenerator.notesObjInstance[notesReferenceNo[2] + 10].name);
                        }
                        //else if (notesJudger.totalPlacedNotes != notesJudger.totalJudgedNotes)
                        //{
                        //Debug.Log("10ノーツ以下の処理");
                        //Debug.Log("ReferenceCheck : " + notesGenerator.notesObjInstance[notesReferenceNo[2] + (notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes)].name);
                        //notesGenerator.notesObjInstance[notesReferenceNo[2] + (notesJudger.totalPlacedNotes - notesJudger.totalJudgedNotes)]
                        //    .GetComponent<MeshRenderer>().enabled = true;
                        //}
                        gameObject.SetActive(false);
                    }
                }

            }
        }
    }
}