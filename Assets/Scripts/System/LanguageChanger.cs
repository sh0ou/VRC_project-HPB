using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

/// <summary>
/// テキストの表示言語を変更します。
/// </summary>
public class LanguageChanger : UdonSharpBehaviour
{
    //対象のオブジェクト
    [SerializeField] GameObject[] targetObj_jpn;
    [SerializeField] GameObject[] targetObj_eng;

    bool isEng = true;

    void Start()
    {
        ChangeLanguage();
    }

    /// <summary>
    /// 言語を切り替えます
    /// </summary>
    public void ChangeLanguage()
    {
        if (isEng)
        {
            //日本語に変更
            isEng = false;
            for (int i = 0; i < targetObj_jpn.Length; i++)
            {
                targetObj_jpn[i].SetActive(true);
                targetObj_eng[i].SetActive(false);
            }
        }
        else
        {
            //英語に変更
            isEng = true;
            for (int i = 0; i < targetObj_eng.Length; i++)
            {
                targetObj_jpn[i].SetActive(false);
                targetObj_eng[i].SetActive(true);
            }
        }
    }
}