using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// 音源データベース
/// </summary>
public class SoundManager : UdonSharpBehaviour
{
    [SerializeField, Tooltip("BGMリスト\n0=通常BGM 1...=楽曲BGM")]
    public AudioClip[] bgmLists;

    [SerializeField, Tooltip("効果音リスト\n0=決定音")]
    public AudioClip[] seLists;

    void Start()
    {

    }
}
