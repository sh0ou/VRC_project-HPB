using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{
    /// <summary>
    /// 音源データベース
    /// </summary>
    public class SoundManager : UdonSharpBehaviour
    {
        [SerializeField, Tooltip("BGMリスト\n0=選曲BGM\n1=リザルトBGM\n2-9=空き\n10...=楽曲BGM")]
        public AudioClip[] bgmLists;

        [SerializeField, Tooltip("効果音リスト\n0=無音\n1=決定音\n2=キャンセル音\n3-10=空き\n11...=楽曲エフェクト")]
        public AudioClip[] seLists;

        [SerializeField, Tooltip("出力オブジェクト")]
        public AudioSource[] audioSources;
    }
}