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
            {3, 45.0f},
            {4, 30.0f},
            {5, 25.0f},
        };
        private readonly Dictionary<int, float> fruitsTypeOrderScaleList = new Dictionary<int, float>(){
            {3, 22.0f},
            {4, 22.0f},
            {5, 18.0f},
        };
        private readonly Vector2 fruitsTypeOrderParentPosition = new Vector2(670.0f, 285.0f);
        private readonly Dictionary<int, List<Vector2>> fruitsTypeOrderPositionList = new Dictionary<int, List<Vector2>>(){
            {3, new List<Vector2>(){new Vector2(15.0f, 30.0f), new Vector2(30.0f,  0.0f), new Vector2(0.0f, 0.0f)}},
            {4, new List<Vector2>(){new Vector2( 0.0f, 30.0f), new Vector2(30.0f, 30.0f), new Vector2(30.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
            {5, new List<Vector2>(){new Vector2( 5.0f, 30.0f), new Vector2(30.0f, 30.0f), new Vector2(40.0f, 0.0f), new Vector2(20.0f, 0.0f), new Vector2(0.0f, 0.0f)}},
        };
        private readonly Dictionary<int, float> fruitsCompMapScaleList = new Dictionary<int, float>(){
            {3, 44.0f},
            {4, 35.0f},
            {5, 28.0f},
        };
        private readonly Dictionary<int, float> fruitsCompMapIntervalList = new Dictionary<int, float>(){
            {3, 50.0f},
            {4, 38.0f},
            {5, 30.0f},
        };
        private readonly Dictionary<int, Vector2> fruitsCompMapPositionList = new Dictionary<int, Vector2>(){
            {3, new Vector2(587.0f, 190.0f)},
            {4, new Vector2(580.0f, 195.0f)},
            {5, new Vector2(576.0f, 198.0f)},
        };

        // 初期処理
        private void Start()
        {
            // 方向条件テキスト設定
            this.setDirectionConditionText();
            // 種類順番UI
            foreach(var type in this.gameManagerScript.fruitsTypeList.Select((v, i) => new {Value = v, Index = i })){
                Vector2 position = this.fruitsTypeOrderPositionList[this.gameManagerScript.fruitsTypeList.Count][type.Index];
                float scale = this.fruitsTypeOrderScaleList[this.gameManagerScript.fruitsTypeList.Count];
                GameObject obj = this.getFruitsUI(position, scale, type.Value);
                obj.transform.parent = this.fruitsTypeOrderParent.transform;
            }
            this.fruitsTypeOrderParent.transform.localPosition = fruitsTypeOrderParentPosition;
            // 完成配置UI
            for(int i = 0; i < this.gameManagerScript.columnNum; i++){
                for(int j = 0; j < this.gameManagerScript.rowNum; j++){
                    float x = j * this.fruitsCompMapIntervalList[this.gameManagerScript.squareNum];
                    float y = i * this.fruitsCompMapIntervalList[this.gameManagerScript.squareNum];
                    Vector2 position = new Vector2( x, -y);
                    float scale = this.fruitsCompMapScaleList[this.gameManagerScript.squareNum];
                    GameObject obj = this.getFruitsUI(position, scale, this.gameManagerScript.compMap[i,j]);
                    obj.transform.parent = this.frutisCompMapParent.transform;
                }
            }
            this.frutisCompMapParent.transform.localPosition = this.fruitsCompMapPositionList[this.gameManagerScript.squareNum];
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
        private GameObject getFruitsUI(Vector2 position, float scale, int type){
            GameObject prefab = (GameObject)frutisCompMapPrefab;
            GameObject obj = Instantiate(prefab);
            obj.GetComponent<Image>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.GetComponent<RectTransform>().anchoredPosition = position;
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(scale, scale);
            return obj;
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
