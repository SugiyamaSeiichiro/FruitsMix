using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameManager gameManagerScript;
    public GameConfig gameConfigScript;
    //public FruitsMap fruitsMapScript;
    public Text timeText;
    public Text tapText;
    public List<GameObject> directionTextList;
    public List<GameObject> frutisCompMapImageList;
    public GameObject frutisCompMapParent;

    // Start is called before the first frame update
    void Start()
    {
        // 縦方向テキスト
        if(!this.gameManagerScript.verticalFlg){
            directionTextList[0].SetActive(false);
        }
        // 横方向テキスト
        if(!this.gameManagerScript.horizontalFlg){
            directionTextList[1].SetActive(false);
        }
        // 斜め方向テキスト
        if(!this.gameManagerScript.diagonalFlg){
            directionTextList[2].SetActive(false);
        }
        // 完成配置UI
        for(int i = 0; i < this.gameManagerScript.columnNum; i++){
            for(int j = 0; j < this.gameManagerScript.rowNum; j++){
                float x = j * this.gameConfigScript.fruitsCompMapUIIntervalList[this.gameManagerScript.squareNum];
                float y = i * this.gameConfigScript.fruitsCompMapUIIntervalList[this.gameManagerScript.squareNum];
                Vector3 position = new Vector3( x, -y, 0.0f);
                float scale = this.gameConfigScript.fruitsCompMapUIScaleList[this.gameManagerScript.squareNum];
                GameObject prefab = (GameObject)frutisCompMapImageList[this.gameManagerScript.compMap[i,j]];
                GameObject obj = Instantiate (prefab, position, prefab.transform.rotation);
                obj.GetComponent<RectTransform> ().sizeDelta = new Vector2(scale, scale);
                obj.transform.parent = this.frutisCompMapParent.transform;
            }
        }
        this.frutisCompMapParent.transform.localPosition = this.gameConfigScript.fruitsCompMapUIPositionList[this.gameManagerScript.squareNum];
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "タイム：" + this.gameManagerScript.timeNum.ToString().PadLeft(4);
        tapText.text = "タップ：" + this.gameManagerScript.tapNum.ToString().PadLeft(4);
    }
}
