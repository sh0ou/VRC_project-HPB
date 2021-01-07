using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ノーツオブジェクト
/// </summary>
public class NotesObj : UdonSharpBehaviour
{
    [SerializeField,Tooltip("ノーツの生存時間")]
    public float lifeTime;

    [SerializeField,Tooltip("判定時間")]
    public float judgeTime;

    [SerializeField]
    private HPB_SettingsManager settingsMng;
    void Start()
    {
        judgeTime = lifeTime;
        settingsMng = GameObject.Find("GameManager").GetComponent<HPB_SettingsManager>();
    }
    private void Update()
    {
        //ゲーム中のみ動かす
        if (settingsMng.gamePlay)
        {
            transform.position += transform.forward * 3.04f * settingsMng.notesSpeed * Time.deltaTime;
            //transform.position += transform.forward * 1f * Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}