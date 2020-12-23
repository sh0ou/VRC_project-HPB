using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ノーツジェネレータ
/// </summary>
public class NotesGenerator : UdonSharpBehaviour
{
    #region 引用:https://www.youtube.com/watch?v=5fQhqfdc5eg
    ////ノーツのLifeはノーツを射出する時間を格納すればOKだと思う
    //static float fMoveValue = 0.01f;　//ハイスピ
    //public int nLifeNote = 300; //ノーツの命
    //private void Update()
    //{
    //    //ハイスピの変更

    //    transform.position = new Vector3(0, nLifeNote * fMoveValue, 0);
    //    //ノーツの命がなくなったら削除
    //    if (nLifeNote < 0)
    //    {
    //        Destroy(this.gameObject);
    //        return;
    //    }
    //    else
    //    {
    //        --nLifeNote;
    //    }
    //}
    #endregion
    #region 変数
    [SerializeField]
    private HPB_GameManager hpb_GM;

    [SerializeField]
    private SoundManager soundManager;

    [SerializeField]
    private TextFileConverter textFileConverter;

    [SerializeField, Tooltip("ノーツオブジェクト")]
    private GameObject notesObj;

    [SerializeField, Tooltip("終点の位置")]
    private Transform endPosition;

    [SerializeField, Tooltip("ノーツのインスタンス郡")]
    private GameObject[] notesObjInstance;
    #endregion
    void Start()
    {
        textFileConverter.SetTextFile(1);
        SetNotes();
    }
    /// <summary>
    /// ノーツを生成
    /// </summary>
    public void SetNotes()
    {
        float notesPos;
        Debug.Log("ノーツ位置生成を開始します...");
        //for = 条件が続く限り続行するやつ
        for (int i = 0; i <= textFileConverter.textDB[1].Length; i++)
        {
            Debug.Log("ノーツNo." + i);

            //https://www.slideshare.net/tenonnotenonno/ss-63731377
            //ノーツ位置を計算
            Debug.Log(int.Parse("1"));
            Debug.Log("終点位置:" + endPosition.position.z);
            Debug.Log("DB数値:" + int.Parse(textFileConverter.textDB[1][i])); //ここでエラる
            Debug.Log("再生時間:" + soundManager.playTime);
            Debug.Log("スピード:" + hpb_GM.notesSpeed);

            notesPos = endPosition.position.z + ((int.Parse(textFileConverter.textDB[1][i]) - soundManager.playTime) * 100 * hpb_GM.notesSpeed);
            Debug.Log("生成位置:" + notesPos);

            //ノーツ生成
            notesObjInstance.SetValue(VRCInstantiate(notesObj), i);
            Debug.Log("ノーツObj:" + notesObjInstance[i]);

            //ノーツ位置を設定
            notesObjInstance[i].gameObject.transform.position = new Vector3(-0.25f, 1, notesPos);
            Debug.Log("ノーツ位置:" + notesObjInstance[i].transform.position);

            //ノーツ生存時間を設定
            notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime = int.Parse(textFileConverter.textDB[1][i]) + (0.1f * hpb_GM.notesSpeed);
            Debug.Log("ノーツ生存時間:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
        }
        Debug.LogWarning("ノーツ生成が完了しました");
    }
}