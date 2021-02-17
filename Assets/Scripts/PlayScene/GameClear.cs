using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class GameClear : MonoBehaviour
{
    public Text stageNumText;
    public Text bestTimeText;
    public Text bestTapText;
    public GameObject starImagesParent;
    public GameObject nextStageButton;

    private int stageNum;
    private int stageAllNum;
    private int typeNum;
    private int squareNum;

    // ゲームクリア表示
    public void Show(int stageNum, float timeNum, int tapNum){
        // 初期処理
        this.init(stageNum);
        // クリア情報保存、取得
        Dictionary<string, bool> newRecordList = new Dictionary<string, bool>();
        StageInfo stageInfo = this.getSaveData(stageNum, timeNum, tapNum, ref newRecordList);
        // 次のステージを解放
        if(stageNum < stageAllNum){
            this.setNextStage(stageNum);
        }
        // ステージ数、時間、タップ数表示
        this.stageNumText.text = "STAGE " + stageNum.ToString();
        this.bestTimeText.text = "BestTime : " + stageInfo.timeNum.ToString("f0");
        this.bestTapText.text  = "BestTap   : " + stageInfo.tapNum.ToString();
        // 新記録テキスト表示
        this.bestTimeText.gameObject.transform.GetChild(0).gameObject.SetActive(newRecordList["time"]);
        this.bestTapText.gameObject.transform.GetChild(0).gameObject.SetActive(newRecordList["tap"]);
        // 星画像表示
        for(int i = 0; i < this.starImagesParent.transform.childCount; i++){
            GameObject obj = this.starImagesParent.transform.GetChild(i).gameObject;
            if(stageInfo == null || !stageInfo.clearList[i]){
                obj.GetComponent<Image>().color = Color.gray;
            }
        }
        // 次のステージボタン表示
        if(stageNum >= this.stageAllNum){
            this.nextStageButton.SetActive(false);
        }
        // クリア画面表示
        this.gameObject.SetActive(true);
        // 広告表示
        GameObject.Find("UnityAds").GetComponent<UnityAds>().ShowAd();
    }

    // 初期処理
    private void init(int stageNum){
        this.stageNum = stageNum;
        GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
        this.typeNum = gameCommonScript.getFruitsTypeNum(stageNum);
        this.squareNum = gameCommonScript.getSquareNum(stageNum);
        this.stageAllNum = gameCommonScript.getStageAllNum();
    }

    // クリア情報を保存、取得
    private StageInfo getSaveData(int stageNum, float timeNum, int tapNum, ref Dictionary<string, bool> newRecordList){
        StageInfo stageInfo = new StageInfo();
        // 新記録判定の初期化
        newRecordList = new Dictionary<string, bool>(){{"time", false}, {"tap", false}};
        // クリア判定タイム数
        float clearTimeNum = Utility.getClearTimeNum(this.typeNum, this.squareNum);
        // クリア判定タップ数
        int clearTapNum = Utility.getClearTapNum(this.typeNum, this.squareNum);
        // 記録するKey作成
        string key = "stage" + stageNum.ToString();
        // 前回の情報取得
        StageInfo beforeStageInfo = PlayerPrefsUtils.GetObject<StageInfo>(key);
        // 前回の情報がある場合
        if(beforeStageInfo != null && beforeStageInfo.clearFlg){
            // クリア
            stageInfo.clearFlg = true;
            // 時間
            stageInfo.timeNum = (timeNum < beforeStageInfo.timeNum) ? timeNum : beforeStageInfo.timeNum;
            // タップ数
            stageInfo.tapNum = (tapNum < beforeStageInfo.tapNum) ? tapNum : beforeStageInfo.tapNum;
            // 指定時間以内のクリア判定
            stageInfo.clearList[0] = (!beforeStageInfo.clearList[0]) ? timeNum < clearTimeNum : beforeStageInfo.clearList[0];
            // 指定タップ数以内のクリア判定
            stageInfo.clearList[1] = (!beforeStageInfo.clearList[1]) ? tapNum < clearTapNum : beforeStageInfo.clearList[1];
            // 時間とタップ数が両方指定数以内のクリア判定
            stageInfo.clearList[2] = (!beforeStageInfo.clearList[2]) ? timeNum < clearTimeNum && tapNum < clearTapNum : beforeStageInfo.clearList[2];
            // 時間が新記録かの判定
            newRecordList["time"] = ((int)beforeStageInfo.timeNum != (int)stageInfo.timeNum);
            // タップ数が新記録かの判定
            newRecordList["tap"] = (beforeStageInfo.tapNum != stageInfo.tapNum);
        // 新規情報追加の場合
        }else{
            // クリア
            stageInfo.clearFlg = true;
            // 時間
            stageInfo.timeNum = timeNum;
            // タップ数
            stageInfo.tapNum = tapNum;
            // 指定時間以内のクリア判定
            stageInfo.clearList[0] = timeNum < clearTimeNum;
            // 指定タップ数以内のクリア判定
            stageInfo.clearList[1] = tapNum < clearTapNum;
            // 時間とタップ数が両方指定数以内のクリア判定
            stageInfo.clearList[2] = timeNum < clearTimeNum && tapNum < clearTapNum;
            // 時間が新記録かの判定
            newRecordList["time"] = true;
            // タップ数が新記録かの判定
            newRecordList["tap"] = true;
        }
        // データを保存する
        PlayerPrefsUtils.SetObject(key, stageInfo);

        return stageInfo;
    }

    private void setNextStage(int stageNum){
        int nextStageNum = stageNum + 1;
        string key = "stage" + nextStageNum.ToString();
        StageInfo beforeStageInfo = PlayerPrefsUtils.GetObject<StageInfo>(key);
        if(beforeStageInfo == null){
            StageInfo stageInfo = new StageInfo();
            PlayerPrefsUtils.SetObject(key, stageInfo);
        }
    }

    // セレクトシーン遷移
    public void onClickToSelectScene(){
        SceneManager.LoadScene(Define.SelectScene);
    }

    // 次のステージ遷移
    public void onClickToNextStageScene(){
        SceneManager.sceneLoaded += GameSceneLoaded;
        SceneManager.LoadScene(Define.PlayScene);
    }

    // シーンに情報受け渡し
    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        //シーン切り替え後のスクリプトを取得
        var gameManager = GameObject.FindWithTag("GameManager").GetComponent<PlayScene.GameManager>();
        //データを渡す処理
        gameManager.stageNum = this.stageNum + 1;
        //イベントから削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
