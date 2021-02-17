using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class Define
    {
        public static readonly int[] RELEASE_STAGE_LIST = {1,16};
        public const float fruitsIntervalIndex = 1.2f;
        public const float CLEAR_BASE_TIME = 20.0f;
        public const int CLEAR_BASE_TAP = 10;

        public const int SELECT_STAGE_WIDTH_NUM = 5;
        public const int SELECT_STAGE_HEIGHT_NUM = 3;
        public const float SELECT_STAGE_INTERVAL_NUM = 1.5f;

        public const string initMapPath = "Conf/InitMap";
        public const string compMapPath = "Conf/CompMap";

        public const string TitleScene = "TitleScene";
        public const string PlayScene = "PlayScene";
        public const string SelectScene = "SelectScene";

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

        public static List<float> getCenterPosList(int num, float size, float intervalNum){
            List<float> fruitsPosList = new List<float>();
            float distance = size * intervalNum;
            float leftPos = 0;
            // 偶数配置
            if(num % 2 == 0){
                leftPos = -(num/2) * distance + distance/2;
            // 奇数配置
            }else{
                leftPos = -((num - 1) / 2) * distance;
            }
            // 位置格納
            for(int i = 0; i < num; i++){
                float pos = leftPos + (i * distance);
                fruitsPosList.Add(pos);
            }
            return fruitsPosList;
        }
    }

    // ステージ情報
    public class StageInfo
    {
        public bool clearFlg = false;
        public float timeNum = 0.0f;
        public int tapNum = 0;
        public bool[] clearList = {false, false, false};
    }



}