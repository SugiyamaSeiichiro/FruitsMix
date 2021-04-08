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

        public static List<string> getMissionTextList(int typeNum, int squareNum){
            List<string> missionTextList = new List<string>();
            // クリア判定タイム数
            float clearTimeNum = Utility.getClearTimeNum(typeNum, squareNum);
            missionTextList.Add(clearTimeNum.ToString("f0"));
            // クリア判定タップ数
            int clearTapNum = Utility.getClearTapNum(typeNum, squareNum);
            missionTextList.Add(clearTapNum.ToString());
            // 両方判定
            missionTextList.Add(clearTimeNum.ToString("f0") + "&" + clearTapNum.ToString());

            return missionTextList;
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

        public static Color getLevelColor(int level){
            Color color;
            switch(level){
                // 難易度：簡単
                case 0:
                    color =  new Color(  0f/255f, 200f/255f, 255f/255f);
                    break;
                // 難易度：普通
                case 1:
                    color =  new Color( 40f/255f, 255f/255f,   0f/255f);
                    break;
                // 難易度：難しい
                default:
                    color =  new Color(255f/255f, 80/255f,   80f/255f);
                    break;
            }
            return color;
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