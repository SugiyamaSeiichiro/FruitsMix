using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class UnityAds : MonoBehaviour, IUnityAdsListener
{
const string ANDROID_ID = "4276417";    // AndoridのゲームID
const string IOS_ID = "4276416";    // iOSのゲームID


    /// 広告再生スキップフラグ
    public static bool adsSkip = false;

    /// 広告再生終了フラグ
    public static bool adsEnd = false;


#region unity lifecycle
    void Start()
    {
        DontDestroyOnLoad(this);


//        if(Advertisement.isSurpprted())
//        {
        // 初期化
#if UNITY_ANDROID
        Advertisement.Initialize(ANDROID_ID, false);
        Debug.Log("Android");
#elif UNITY_IOS
        Advertisement.Initialize(IOS_ID, /*テストモードならtrue*/true);
        Debug.Log("IOS");
#else
//        Advertisement.Initialize("");
        Advertisement.Initialize(ANDROID_ID, /*テストモードならtrue*/true);
        Debug.Log("ELSE");
#endif
    Advertisement.Initialize(ANDROID_ID, /*テストモードならtrue*/true);
//        }
    }
#endregion

#region public method
    public void ShowAd()
    {
        Debug.Log("rewardedVideo");
        // 準備ができたら広告再生
        if (Advertisement.IsReady("rewardedVideo"))
        {
 //           ShowOptions options = new ShowOptions();
 //           options.resultCallback = OnUnityAdsDidFinish;

            //Advertisement.Show();
            Advertisement.Show("rewardedVideo");   // スキップ不可の動画広告
            Debug.Log("rewardedVideo");
        }
        //if (Advertisement.IsReady("Non-rewarded"))
        {
            //Advertisement.Show("Non-rewarded");   // スキップ可能の動画広告
            //Debug.Log("Non-rewarded");
        }
        //if (Advertisement.IsReady("Banner"))
        {
            //Advertisement.Show("Banner");   // バナー広告
            //Debug.Log("Banner");
        }
    }
#endregion

#region private method
#endregion

#region event
    //広告の準備完了
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log($"{placementId}の準備が完了");
        // 広告を初期化しても準備ができるまで時間がかかるので、イベントとして
    }

    //広告でエラーが発生
    public void OnUnityAdsDidError(string message)
    {
        Debug.Log($"広告でエラー発生 : {message}");
        adsEnd = true;
    }

    //広告開始
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log($"{placementId}の広告が開始");
    }

    //広告の表示終了
    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        Debug.Log($"{placementId}の表示終了");
        adsEnd = true;
        switch (showResult)
        {
            //最後まで視聴完了(リワード広告ならここでリワード付与する感じ)
            case ShowResult.Finished:
                Debug.Log("広告の表示成功");
                break;
            //スキップされた
            case ShowResult.Skipped:
                Debug.Log("広告スキップ");
                adsSkip = true;
                break;
            //表示自体が失敗した
            case ShowResult.Failed:
                Debug.LogWarning("広告の表示失敗");
                break;
        }
    }
#endregion
}
