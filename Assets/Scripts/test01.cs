using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using UnityEngine.UI;

public class test01 : UdonSharpBehaviour
{
    [SerializeField]
    private TextAsset textfile;
    [SerializeField]
    private Text titleText;
    private string textData;

    void Start()
    {
        textData = textfile.text;
        titleText.text = textData;
    }
}
