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
    private HPB_SettingsManager hpb_GM;

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
        //textFileConverter.SetTextFile(1);
        //SetNotes();
    }
    /// <summary>
    /// ノーツを生成
    /// </summary>
    public void SetNotes()
    {
        float notesPosz;
        Debug.Log("ノーツ位置生成を開始します...");
        notesObjInstance = new GameObject[textFileConverter.textDB[1].Length];
        Debug.Log("生成数;" + notesObjInstance.Length);
        //for = 条件が続く限り続行するやつ
        for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
        {
            //Debug.Log("ノーツNo." + i);
            //https://www.slideshare.net/tenonnotenonno/ss-63731377
            //ノーツ位置を計算
            #region デバッグ用
            //Debug.Log("終点位置:" + endPosition.position.z);
            //Debug.Log("テスト:" + textFileConverter.textDB[1][i]);
            //Debug.Log("DB数値:" + float.Parse(textFileConverter.textDB[1][i]));
            //Debug.Log("再生時間:" + soundManager.playTime);
            //Debug.Log("スピード:" + hpb_GM.notesSpeed);
            #endregion
            notesPosz = endPosition.position.z + 
                ((float.Parse(textFileConverter.textDB[1][i]) - soundManager.playTime) * 
                (3 * hpb_GM.notesSpeed));
            //Debug.Log("生成位置:" + notesPos);

            //ノーツ生成
            notesObjInstance.SetValue(VRCInstantiate(notesObj), i);
            //Debug.Log("ノーツObj:" + notesObjInstance[i]);

            int notesLane = int.Parse(textFileConverter.textDB[3][i]);
            float notesPosx = 0;
            switch (notesLane)
            {
                case 1:
                    notesPosx = -0.75f;
                    break;
                case 2:
                    notesPosx = -0.25f;
                    break;
                case 3:
                    notesPosx = 0.25f;
                    break;
                case 4:
                    notesPosx = 0.75f;
                    break;
            }
            //ノーツ位置を設定
            notesObjInstance[i].gameObject.transform.position = new Vector3(notesPosx, 1, notesPosz);
            //Debug.Log("ノーツ位置:" + notesObjInstance[i].transform.position);

            //ノーツ生存時間を設定
            #region デバッグ用
            //Debug.Log("ゲームマネージャ:" + hpb_GM);
            //Debug.Log("ノーツスピード:" + hpb_GM.notesSpeed);
            //Debug.Log("ノーツ移動速度計算:" + 0.1f * hpb_GM.notesSpeed);
            //Debug.Log("オブジェクトチェック:" + notesObjInstance[i]);
            //Debug.Log("コンポーネント:" + notesObjInstance[i].GetComponent<NotesObjScr>());
            //Debug.Log("lifetime取得チェック:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
            //Debug.Log("Parseチェック" + float.Parse(textFileConverter.textDB[1][i]));
            #endregion
            notesObjInstance[i].GetComponent<NotesObj>().lifeTime = 
                float.Parse(textFileConverter.textDB[1][i]) + (0.1f * hpb_GM.notesSpeed);
            //Debug.Log("ノーツ生存時間:" + notesObjInstance[i].GetComponent<NotesObjScr>().lifeTime);
        }
        Debug.Log("<color=green>ノーツ生成が完了しました</color>");
    }
}