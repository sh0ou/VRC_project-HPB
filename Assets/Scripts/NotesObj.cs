using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ノーツオブジェクト
/// </summary>
public class NotesObj : UdonSharpBehaviour
{
    [SerializeField]
    public float lifeTime;

    [SerializeField]
    private HPB_SettingsManager test05;
    void Start()
    {
        test05 = GameObject.Find("GameManager").GetComponent<HPB_SettingsManager>();
    }
    private void Update()
    {
        transform.position += transform.forward * 3f * test05.notesSpeed * Time.deltaTime;
        //ゲーム中のみ動かす
        if (test05.gamePlay)
        {
            transform.position += transform.forward * 1f * Time.deltaTime;
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}