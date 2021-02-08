using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class TestUnityAds : MonoBehaviour
{
const string ANDROID_ID = "4006167";
const string IOS_ID = "4006166";

    void Awake()
    {
//        if(Advertisement.isSurpprted())
//        {
        // 初期化
#if UNITY_ANDROID
        Advertisement.Initialize(ANDROID_ID, false);
#elif UNITY_IOS
        Advertisement.Initialize(IOS_ID, false);
#else
//        Advertisement.Initialize("");
        Advertisement.Initialize(ANDROID_ID);
#endif
//        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // 準備ができたら広告再生
        //if (Advertisement.IsReady("rewardedVideo"))
        {
            //Advertisement.Show();
            Advertisement.Show("rewardedVideo");   // スキップ不可の動画
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
