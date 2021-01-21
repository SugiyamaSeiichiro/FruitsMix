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
        public GameObject fruitsTypeOrderParent;
        public GameObject frutisCompMapPrefab;
        public GameObject frutisCompMapParent;
        public GameObject gameClearUI;
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

        // 初期処理
        private void Start()
        {
            // 方向条件テキスト設定
            this.setDirectionConditionText();
            
            // 種類順番UI
            foreach(var type in this.gameManagerScript.fruitsTypeList.Select((v, i) => new {Value = v, Index = i })){
                Vector2 position = this.fruitsTypeOrderPositionList[this.gameManagerScript.fruitsTypeList.Count][type.Index];
                float size = 20.0f;
                this.createFruitsUI(position, size, type.Value, this.fruitsTypeOrderParent);
            }
            List<float> fruitsPosList = this.getFruitsPosList(this.gameManagerScript.squareNum);
            // 完成配置UI
            for(int i = 0; i < this.gameManagerScript.columnNum; i++){
                for(int j = 0; j < this.gameManagerScript.rowNum; j++){
                    float size = this.fruitsSizeList[this.gameManagerScript.squareNum];
                    Vector2 position = new Vector2( fruitsPosList[j], -fruitsPosList[i]);
                    this.createFruitsUI(position, size, this.gameManagerScript.compMap[i,j], this.frutisCompMapParent);
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

        // フルーツUI取得
        private void createFruitsUI(Vector2 position, float size, int type, GameObject parent){
            GameObject prefab = (GameObject)frutisCompMapPrefab;
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<Image>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.transform.SetParent(parent.transform, false);
            RectTransform rectTransform = obj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(size, size);
            rectTransform.anchoredPosition = position;
        }

        // ゲームクリアUI表示
        public void showGameClearUI(){
            this.gameClearUI.SetActive(true);
        }

        // セレクトシーン遷移
        public void onClickToSelectScene(){
            SceneManager.LoadScene("SelectScene");
        }
    }
}
