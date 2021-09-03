using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace PlayScene
{
    public class UIManager : MonoBehaviour
    {
        public GameManager gameManagerScript;
        public Text timeText;
        public Text tapText;
        public List<GameObject> directionTextList;
        public GameObject frutisImagePrefab;
        public GameObject nextFruitsUIPrefab;
        public GameObject nextFruitsUIParent;
        public GameObject fruitsTypeOrderParent;
        public GameObject frutisCompMapParent;
        public GameObject gameClearUI;
        public GameObject nextButton;

        private List<GameObject> nextFruitsUIList = new List<GameObject>();
        private readonly Dictionary<int, float> fruitsSizeList = new Dictionary<int, float>(){
            {3, 100.0f},
            {4, 80.0f},
            {5, 60.0f},
        };
        private readonly Dictionary<int, List<Vector2>> fruitsTypeOrderPositionList = new Dictionary<int, List<Vector2>>(){
            {3, new List<Vector2>(){new Vector2(45.0f, 90.0f), new Vector2(90.0f,  0.0f), new Vector2(0.0f, 0.0f)}},
            {4, new List<Vector2>(){new Vector2( 0.0f, 90.0f), new Vector2(90.0f, 30.0f), new Vector2(90.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
            {5, new List<Vector2>(){new Vector2(15.0f, 90.0f), new Vector2(90.0f, 30.0f), new Vector2(120.0f, 0.0f), new Vector2(60.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
        };
        private readonly float fruitsTypeOrderSize = 60.0f;

        // 初期処理
        private void Start()
        {
            // 方向条件テキスト設定
            this.setDirectionConditionText();
            // 種類順番UI
            foreach(var value in this.gameManagerScript.fruitsTypeList.Select((v, i) => new {Value = v, Index = i })){
                Vector2 position = this.fruitsTypeOrderPositionList[this.gameManagerScript.fruitsTypeList.Count][value.Index];
                int type = value.Value;
                GameObject parent = this.fruitsTypeOrderParent;
                this.createFruitsUI(type, fruitsTypeOrderSize, position, parent);
            }
            // フルーツ配置箇所取得
            float fruitsSize = this.fruitsSizeList[this.gameManagerScript.squareNum];
            GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            List<float> fruitsPosList = gameCommonScript.getFruitsPosList(this.gameManagerScript.squareNum, fruitsSize);
            // 完成配置UI
            for(int i = 0; i < this.gameManagerScript.columnNum; i++){
                for(int j = 0; j < this.gameManagerScript.rowNum; j++){
                    Vector2 position = new Vector2( fruitsPosList[j], -fruitsPosList[i]);
                    int type = this.gameManagerScript.compMap[i,j];
                    GameObject parent = this.frutisCompMapParent;
                    this.createFruitsUI(type, fruitsSize, position, parent);
                }
            }
        }

        // 方向条件テキスト設定
        private void setDirectionConditionText(){
            // 縦方向テキスト
            if(!this.gameManagerScript.verticalFlg){
                this.directionTextList[0].SetActive(false);
            }
            // 横方向テキスト
            if(!this.gameManagerScript.horizontalFlg){
                this.directionTextList[1].SetActive(false);
            }
            // 斜め方向テキスト
            if(!this.gameManagerScript.diagonalFlg){
                this.directionTextList[2].SetActive(false);
            }
        }

        // フルーツUI取得
        private void createFruitsUI(int type, float size, Vector2 position, GameObject parent){
            GameObject obj = Instantiate(frutisImagePrefab);
            obj.GetComponent<Image>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.transform.SetParent(parent.transform, false);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(size, size);
            rectTransform.anchoredPosition = position;
        }

        // ゲームクリアUI表示
        public void showGameClearUI(){
            this.gameClearUI.GetComponent<GameClear>().Show(
                this.gameManagerScript.stageNum,
                this.gameManagerScript.timeNum,
                this.gameManagerScript.tapNum
            );
        }

        // 次フルーツUI作成
        public void createNextFruitsUI(int type, float size, Vector2 position){
            // スクリーン座標取得
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            // 次フルーツUI作成
            GameObject obj = Instantiate(nextFruitsUIPrefab);
            obj.GetComponent<Image>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.transform.SetParent(this.nextFruitsUIParent.transform, false);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(size, size);
            obj.transform.position = screenPos;
            // 作成したオブジェクトの記録
            this.nextFruitsUIList.Add(obj);
        }

        // 次フルーツUI削除
        public void deleteNextFruitsUI(){
            foreach(var value in this.nextFruitsUIList)
            {
                Destroy(value);
            }
            this.nextFruitsUIList.Clear();
        }
    }
}
