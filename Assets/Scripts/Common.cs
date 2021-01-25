using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public static class Define
    {
        public const float CLEAR_TIME = 20;
    }
    // ステージ情報
    class StageInfo
    {
        public float timeNum = 0.0f;
        public int tapNum = 0;
        public bool[] clearList = {true, true, true};
    }

}