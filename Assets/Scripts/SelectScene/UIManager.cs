using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;

namespace SelectScene
{
    public class UIManager : MonoBehaviour
    {

        public GameObject stageButtonPrefab;
        public GameObject stageInfoModalPrefab;
        public GameObject stageLockImagePrefab;
        public GameObject stageButtonParent;
        public GameObject backPageButton;
        public GameObject nextPageButton;

        private int stageAllNum = 0;
        private int pageNum = 1;
        private AudioManager audioManager;

        // Start is called before the first frame update
        void Start()
        {
            this.audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
            this.audioManager.playBGM(BGM_TYPE.SELECT_SCENE);
            this.createPage(this.pageNum);
        }

        // ページ生成
        private void createPage(int pageNum){
            this.pageNum = pageNum;
            GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            stageAllNum = gameCommonScript.getStageAllNum();
            int width = Define.SELECT_STAGE_WIDTH_NUM;
            int height = Define.SELECT_STAGE_HEIGHT_NUM;
            float intervalNum = Define.SELECT_STAGE_INTERVAL_NUM;
            int pageStageNum = width * height;
            int startNum = pageStageNum * (pageNum - 1);
            int endNum = pageStageNum * pageNum;
            float size = this.stageButtonPrefab.GetComponent<RectTransform>().sizeDelta.x;
            var centerPosXList = Utility.getCenterPosList(width, size, intervalNum);
            var centerPosYList = Utility.getCenterPosList(height, size, intervalNum);
            // ステージボタン生成
            for(int i = startNum; i < endNum; i++){
                int stageNum = i + 1;
                Vector2 position = new Vector2(centerPosXList[i%width], -centerPosYList[(i-startNum)/width]);
                this.createStageButton(stageNum, position);
            }
            // 前ページボタン
            backPageButton.gameObject.SetActive(startNum > 0);
            // 次ページボタン
            nextPageButton.gameObject.SetActive(endNum < this.stageAllNum);
            // 背景色の変更
            int level = gameCommonScript.getSquareNum(startNum + 1) - 3;
            Camera.main.backgroundColor = Utility.getLevelColor(level);
        }

        // ステージボタン生成
        private void createStageButton(int stageNum, Vector2 position)
        {
            GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            StageInfo stageInfo = gameCommonScript.getStageInfo(stageNum);
            GameObject obj;
            // 解放ステージの場合
            if(stageInfo != null){
                obj = Instantiate(this.stageButtonPrefab);
                Text stageNumText = obj.transform.GetChild(0).gameObject.GetComponent<Text>();
                stageNumText.text = stageNum.ToString();
                obj.GetComponent<Button>().onClick.AddListener(() => {this.showStageInfoModal(stageNum);});
                for(int i = 0; i < obj.transform.GetChild(1).childCount; i++){
                    GameObject starObj = obj.transform.GetChild(1).transform.GetChild(i).gameObject;
                    if(stageInfo.clearList[i] == false){
                        starObj.GetComponent<Image>().color = Color.gray;
                    }
                }
            // 未解放ステージの場合
            }else if( stageNum <= this.stageAllNum ){
                obj = Instantiate(this.stageLockImagePrefab);
            // 未実装ステージの場合
            }else{
                obj = Instantiate(this.stageLockImagePrefab);
            }
            obj.transform.SetParent(this.stageButtonParent.transform, false);
            obj.GetComponent<RectTransform>().anchoredPosition = position;
        }

        // ステージ情報モーダル表示
        private void showStageInfoModal(int stageNum){
            GameObject obj = Instantiate(this.stageInfoModalPrefab);
            obj.transform.SetParent(this.gameObject.transform, false);
            obj.GetComponent<StageInfoModal>().setStageButtonInfo(stageNum);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }

        // ステージボタン削除
        private void deleteStageButtons(){
            foreach(Transform child in stageButtonParent.transform){
                GameObject.Destroy(child.gameObject);
            }
        }

        // 次ページに移動
        public void onClickToNextPage(){
            this.deleteStageButtons();
            this.createPage(this.pageNum + 1);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }

        // 前ページに移動
        public void onClickToBackPage(){
            this.deleteStageButtons();
            this.createPage(this.pageNum - 1);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }

        // 難易度別の最初ページに移動
        public void onClickToPageJump(int pageNum){
            this.deleteStageButtons();
            this.createPage(pageNum);
            this.audioManager.playSE(SE_TYPE.BUTTON);
        }
    }
}
