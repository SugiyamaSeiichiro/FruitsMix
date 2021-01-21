using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SelectScene
{
    public class StageButton : MonoBehaviour
    {
        private GameCommon gameCommonScript;
        public Text titleText;
        public Text squareNumText;
        public Text typeNumText;
        public Button startButton;
        public GameObject fruitsImagePrefab;
        public GameObject fruitsInitMapParent;
        public GameObject fruitsCompMapParent;
        private int rowNum;
        private int columnNum;
        private int stageNum;
        private int squareNum;
        private readonly Dictionary<int, float> fruitsSizeList = new Dictionary<int, float>(){
            {3, 45.0f},
            {4, 30.0f},
            {5, 25.0f},
        };

        // ステージボタン設定
        public void setStageButton(int stageNum){
            this.gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            this.stageNum = stageNum;
            this.rowNum = this.gameCommonScript.getRowNum(stageNum);
            this.columnNum = this.gameCommonScript.getColumnNum(stageNum);
            this.squareNum = this.gameCommonScript.getSquareNum(stageNum);
            this.titleText.text = "ステージ " + stageNum.ToString();
            this.squareNumText.text = rowNum.ToString() + "×" + columnNum.ToString() + "マス";
            int typeNum = this.gameCommonScript.getFruitsTypeNum(stageNum);
            this.typeNumText.text = typeNum.ToString() + "種類";
            startButton.onClick.AddListener(() => onClickToMainScene());
            // フルーツUI作成
            int[,] initMap = this.gameCommonScript.getFruitsInitMap(stageNum);
            int[,] compMap = this.gameCommonScript.getFruitsCompMap(stageNum);
            this.createFruitsMapUI(initMap, this.fruitsInitMapParent);
            this.createFruitsMapUI(compMap, this.fruitsCompMapParent);
        }

        private List<float> getFruitsPosList(int squareNum){
            List<float> fruitsPosList = new List<float>();
            float distance = this.fruitsSizeList[squareNum] * 1.2f;
            float leftPos = 0;
            // 偶数配置
            if(squareNum % 2 == 0){
                int num = squareNum / 2;
                leftPos = -num * distance + distance/2;
            // 奇数配置
            }else{
                int num = (squareNum - 1) / 2;
                leftPos = -num * distance;
            }
            // 位置格納
            for(int i = 0; i < squareNum; i++){
                float pos = leftPos + (i * distance);
                fruitsPosList.Add(pos);
            }
            return fruitsPosList;
        }

        private void createFruitsMapUI(int[,] map, GameObject parent){
            int squareNum = this.gameCommonScript.getSquareNum(this.stageNum);
            float size = this.fruitsSizeList[squareNum];
            List<float> fruitsPosList = this.getFruitsPosList(squareNum);
            for(int i = 0; i < this.columnNum; i++){
                for(int j = 0; j < this.rowNum; j++){
                    GameObject obj = Instantiate(this.fruitsImagePrefab);
                    obj.GetComponent<Image>().sprite = this.gameCommonScript.getFrutisSprite(map[i,j]);
                    obj.transform.SetParent(parent.transform, false);
                    RectTransform rectTransform = obj.GetComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(size, size);
                    rectTransform.anchoredPosition = new Vector2(fruitsPosList[j], -fruitsPosList[i]);
                }
            }
        }

        private void onClickToMainScene(){
            SceneManager.sceneLoaded += GameSceneLoaded;
            SceneManager.LoadScene("MainScene");
        }

        private void GameSceneLoaded(Scene next, LoadSceneMode mode)
        {
            //シーン切り替え後のスクリプトを取得
            var gameManager = GameObject.FindWithTag("GameManager").GetComponent<PlayScene.GameManager>();
            //データを渡す処理
            gameManager.stageNum = this.stageNum;
            //イベントから削除
            SceneManager.sceneLoaded -= GameSceneLoaded;
        }
    }
}
