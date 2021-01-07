using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ドラムから音流す
/// </summary>
public class Test03 : UdonSharpBehaviour
{
    [SerializeField, Tooltip("音声出力コンポ")]
    private AudioSource audioSource;

    [SerializeField, Tooltip("音声DB")]
    private SoundManager soundList;

    [SerializeField]
    private HPB_GameManager gameMng;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 23)
        {
            //ドラム単体を再生
            audioSource.PlayOneShot(soundList.seLists[1]);
            gameMng.JudgeNotes(1);
        }
    }
}
