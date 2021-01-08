using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// デッドライン判定オブジェクト
/// </summary>
public class DeadLineObj : UdonSharpBehaviour
{
    [SerializeField]
    private HPB_GameManager gameMng;

    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 24)
        {
            gameMng.JudgeNotes(-1);
        }
    }
}
