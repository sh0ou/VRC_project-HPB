using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using System.Linq;

/// <summary>
/// ノーツ判定用スクリプト
/// </summary>
public class NotesJudger : UdonSharpBehaviour
{
    [SerializeField, Tooltip("ノーツジェネレータ")]
    private NotesGenerator notesGen;

    [SerializeField, Tooltip("各ノーツスクリプト")]
    private GameObject[] notesObjs;

    [SerializeField, Tooltip("判定対象オブジェクト格納用")]
    private NotesObj targetNotes;

    //[SerializeField, Tooltip("時間格納用配列")]
    //private float[] timeValues;

    [SerializeField, Tooltip("最小時間格納用")]
    private float timeValue_min;

    void Start()
    {

    }

    /// <summary>
    /// 判定対象ノーツをセット
    /// </summary>
    /// <param name="i">レーン番号</param>
    public void SetNotesObjs(int i)
    {
        Debug.Log("[<color=yellow>NotesJudger</color>]レーン" + i + "のノーツセット処理を開始します...");
        notesObjs = new GameObject[notesGen.SendNotesValue()];
        for (int n = 0; n < notesGen.SendNotesValue(); n++)
        {
            Debug.Log("Round:" + n);
            Debug.Log("レーン番号チェック:" + notesGen.SendNotesLaneNum(n));
            if (notesGen.SendNotesLaneNum(n) == i)
            {
                Debug.Log("コンポーネント取得チェック:" + notesGen.SendNotesObj(n).GetComponent<NotesObj>());
                notesObjs[n] = notesGen.SendNotesObj(n);
            }
        }
    }

    /// <summary>
    /// ノーツ判定開始
    /// </summary>
    /// <param name="f">ドラム判定時間</param>
    /// <returns></returns>
    public int Judge(float f)
    {
        targetNotes = null;
        timeValue_min = 9999;
        //位置格納処理
        //for (int i = 0; i <= notesObjs.Length; i++)
        //{
        //    Debug.Log("Obj" + i + "位置:" + notesObjs[i].gameObject.transform.position.z);
        //    Debug.Log("ローカル位置:" + notesObjs[i].gameObject.transform.localPosition.z);
        //    timeValues[i] = notesObjs[i].gameObject.transform.position.z;
        //}
        //timeValue_min = Mathf.Min(timeValues);

        //時間格納処理
        for (int i = 0; i < notesObjs.Length; i++)
        {
            Debug.Log("格納処理:" + i);
            if (notesObjs[i] == null)
            {
                Debug.LogWarning("nullチェックに失敗しました。スキップします");
                return 3;
            }
            else /*(notesObjs[i] != null)*/
            {
                Debug.Log("NullCheck:OK");
                Debug.Log("ジャッジタイム:" + notesObjs[i].GetComponent<NotesObj>().judgeTime);
                //if (notesObjs[i].judgeTime < timeValue_min)
                //{
                //Debug.Log("最小値Obj:" + notesObjs[i].gameObject.name + "/" + notesObjs[i].judgeTime);
                timeValue_min = notesObjs[i].GetComponent<NotesObj>().judgeTime;
                targetNotes = notesObjs[i].GetComponent<NotesObj>();
                i = notesObjs.Length;
                //}
            }
        }

        //判定処理
        int i_r = -1;
        Debug.Log("時間差:" + Mathf.Abs(targetNotes.judgeTime - f));
        //H判定
        if (Mathf.Abs(targetNotes.judgeTime - f) <= 0.04f / 2)
        {
            Debug.Log("Happy!");
            i_r = 0;
        }
        //G判定
        else if (Mathf.Abs(targetNotes.judgeTime - f) <= 0.1f / 2)
        {
            Debug.Log("Good");
            i_r = 1;
        }
        //S判定
        else if (Mathf.Abs(targetNotes.judgeTime - f) <= 0.2f / 2)
        {
            Debug.Log("Sad...");
            i_r = 2;
        }
        else
        {
            Debug.LogWarning("判定範囲外です。スキップします");
            return 3;
        }

        //オブジェクト削除処理
        Destroy(targetNotes.gameObject);
        if (i_r == -1)
        {
            Debug.LogError("[<color=red>NotesJudger</color>]判定結果値が不正です");
        }
        return i_r;
    }
}