﻿using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

/// <summary>
/// ゲームプレイ時に必要なUIを格納するスクリプト
/// </summary>
public class HPB_UIManager : UdonSharpBehaviour
{
    /*メモ
    色は0~1の小数で指定　255指定はできない
    bottomが下　topが上
    */

    #region 共通
    [Header("共通")]
    [SerializeField, Tooltip("ドラムガイドUI")]
    private Image[] guideUI;

    #endregion
    #region プレイ画面
    [Header("プレイ画面1")]
    [SerializeField, Tooltip("値更新用UI\n0=タイトル\n1=アーティスト\n2=レベル")]
    private InputField[] inputFields_play_1;

    [SerializeField, Tooltip("プレイ画面txtの色変更用UI\n0=レベル種類\n1=レベル値")]
    private Animator[] fixedText_play_1;

    [Header("プレイ画面2")]
    [SerializeField, Tooltip("値更新用UI\n0=チェイン\n1=スコア")]
    private InputField[] inputFields_play_2;

    //TMP指定できないのでAnimator
    //（プリセットどれかわからないので直接指定）
    [SerializeField, Tooltip
        ("プレイ画面txtの色変更用UI\n0=チェイン値\n1=チェインテキスト\n2=スコア値\n3=スコアテキスト")]
    private Animator[] fixedText_play_2;

    #endregion
    #region リザルト画面
    [Header("リザルト画面")]
    [SerializeField, Tooltip("リザルト画面ジャケット画像")]
    private Image jacketImage;

    [SerializeField, Tooltip
        ("リザルト画面txtの値更新用UI\n0=ランク\n1=スコア\n2=H判定\n3=G判定\n4=S判定\n5=M判定")]
    private InputField[] valueFields_result;

    [SerializeField, Tooltip
        ("リザルト画面txtの色更新用UI\n0=ランク\n1=スコア値")]
    private Animator[] fixedText_result;

    #endregion
    void Start()
    {

    }
}