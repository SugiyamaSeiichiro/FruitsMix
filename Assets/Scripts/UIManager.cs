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
        public GameObject fruitsTypeOrderParent;
        public GameObject frutisCompMapParent;
        public GameObject gameClearUI;
        public GameObject nextButton;

        private List<GameObject> nextFruitsUIList = new List<GameObject>();
        private readonly Dictionary<int, float> fruitsSizeList = new Dictionary<int, float>(){
            {3, 55.0f},
            {4, 40.0f},
            {5, 32.0f},
        };
        private readonly Dictionary<int, List<Vector2>> fruitsTypeOrderPositionList = new Dictionary<int, List<Vector2>>(){
            {3, new List<Vector2>(){new Vector2(15.0f, 30.0f), new Vector2(30.0f,  0.0f), new Vector2(0.0f, 0.0f)}},
            {4, new List<Vector2>(){new Vector2( 0.0f, 30.0f), new Vector2(30.0f, 30.0f), new Vector2(30.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
            {5, new List<Vector2>(){new Vector2( 5.0f, 30.0f), new Vector2(30.0f, 30.0f), new Vector2(40.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
        };
        private readonly float fruitsTypeOrderSize = 20.0f;

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
            this.gameClearUI.SetActive(true);
            GameCommon gameCommonScript = GameObject.Find("GameCommon").GetComponent<GameCommon>();
            int stageAllNum = gameCommonScript.getStageAllNum();
            if(this.gameManagerScript.stageNum >= stageAllNum){
                this.nextButton.SetActive(false);
            }
        }

        // 次フルーツUI作成
        public void createNextFruitsUI(int type, float size, Vector2 position){
            // スクリーン座標取得
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Camera.main, position);
            // 次フルーツUI作成
            GameObject obj = Instantiate(nextFruitsUIPrefab);
            obj.GetComponent<Image>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.transform.SetParent(this.gameObject.transform, false);
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

        // セレクトシーン遷移
        public void onClickToSelectScene(){
            SceneManager.LoadScene("SelectScene");
        }

        // 次のステージ遷移
        public void onClickToNextStageScene(){
            SceneManager.sceneLoaded += GameSceneLoaded;
            SceneManager.LoadScene("MainScene");
        }

        private void GameSceneLoaded(Scene next, LoadSceneMode mode)
        {
            //シーン切り替え後のスクリプトを取得
            var gameManager = GameObject.FindWithTag("GameManager").GetComponent<PlayScene.GameManager>();
            //データを渡す処理
            gameManager.stageNum = this.gameManagerScript.stageNum + 1;
            //イベントから削除
            SceneManager.sceneLoaded -= GameSceneLoaded;
        }
    }
}
