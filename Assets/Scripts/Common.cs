using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class Define
    {
        public const float CLEAR_BASE_TIME = 20.0f;
        public const int CLEAR_BASE_TAP = 10;
    }

    public static class Utility
    {
        // クリア判定タイム数
        public static float getClearTimeNum(int typeNum, int squareNum)
        {
            return (typeNum - 2) * (squareNum - 2) * Define.CLEAR_BASE_TIME;
        }

        // クリア判定タップ数
        public static int getClearTapNum(int typeNum, int squareNum)
        {
            return (typeNum - 2) * (squareNum - 2) * Define.CLEAR_BASE_TAP;
        }
    }

    // ステージ情報
    class StageInfo
    {
        public float timeNum = 0.0f;
        public int tapNum = 0;
        public bool[] clearList = {true, true, true};
    }

}