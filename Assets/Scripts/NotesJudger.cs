using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HPB
{

    public class NotesJudger : UdonSharpBehaviour
    {
        [SerializeField]
        private TextFileConverter textFileConverter;

        [SerializeField]
        private PlayManager playManager;

        [SerializeField, Tooltip("レーン,Time")]
        public float[][] judgeTimings;

        [SerializeField, Tooltip("レーン,フラグ")]
        public int[][] judgeFlag;

        void Start()
        {

        }

        public void Setup()
        {
            for (int lane = 0; lane < 3; lane++)
            {
                //要素数カウント用
                int value = 0;
                for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
                {
                    //レーンが一致した場合
                    if (int.Parse(textFileConverter.textDB[3][i]) == lane)
                    {
                        //判定時間を代入
                        judgeTimings[lane][value] =
                            int.Parse(textFileConverter.textDB[1][i]);
                        value++;
                    }
                }
                //フラグ初期化
                for (int i = 0; i < judgeTimings[lane].Length; i++)
                {
                    judgeFlag[lane][i] = 1;
                }
            }
            #region Archive
            //for (int i = 0; i < textFileConverter.textDB[1].Length; i++)
            //{
            //    if (textFileConverter.textDB[1][i] == "-" || textFileConverter.textDB[1][i] == "0")
            //    {

            //    }
            //    else
            //    {

            //    }
            //}
            #endregion
        }

        //public (int, bool) Judge(int lane)
        public int Judge(int lane)
        {
            for (int i = 0; i < judgeFlag[lane].Length; i++)
            {
                bool b = false;
                //判定済フラグチェック
                if (judgeFlag[lane][i] == 0)
                {
                    //判定時間チェック
                    float calcTime = Mathf.Abs(judgeTimings[lane][i] - playManager.playTime);
                    if (calcTime < 0.04f / 2)
                    {
                        Debug.Log("Happy");
                        //if (EndCheck())
                        //{
                        //    return (0, true);
                        //}
                        //else
                        //{
                        //    return (0, false);
                        //}
                        return 0;
                    }
                    else if (calcTime <= 0.1f / 2)
                    {
                        Debug.Log("Good");
                        //if (EndCheck())
                        //{
                        //    return (1, true);
                        //}
                        //else
                        //{
                        //    return (1, false);
                        //}
                        return 1;
                    }
                    else if (calcTime <= 0.15f / 2)
                    {
                        Debug.Log("Sad");
                        //if (EndCheck())
                        //{
                        //    return (2, true);
                        //}
                        //else
                        //{
                        //    return (2, false);
                        //}
                        return 2;
                    }
                    else
                    {
                        Debug.Log("?");
                        break;
                    }
                }
            }
            Debug.Log("判定対象外");
            //return (-1, false);
            return -1;
        }

        private bool EndCheck()
        {
            for (int lane = 0; lane < 3; lane++)
            {
                for (int i = 0; i < judgeFlag[lane].Length; i++)
                {
                    if (judgeFlag[lane][i] == 1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}