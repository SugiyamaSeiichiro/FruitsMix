using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayScene
{
    public class FruitsMap : MonoBehaviour
    {
        public GameObject fruitsPrefab;
        public GameManager gameManagerScript;
        public AudioManager audioManagerScript;

        private int rowNum;
        private int columnNum;
        private int squareNum;
        private int txtCheckNum;
        private int[,] initMap;
        private int[,] curMap;
        private int[,] compMap;
        private GameObject[,] fruitsObjectMap;
        private List<int> fruitsTypeList = new List<int>();
        private int fruitsTypeNum;

        private readonly Dictionary<int, float> fruitsScaleList = new Dictionary<int, float>(){
            {3, 2.4f},
            {4, 1.8f},
            {5, 1.4f},
        };
        private readonly Dictionary<int, float> fruitsIntervalList = new Dictionary<int, float>(){
            {3, 3.0f},
            {4, 2.2f},
            {5, 1.7f},
        };
        private readonly Dictionary<int, Vector2> fruitsPositionList = new Dictionary<int, Vector2>(){
            {3, new Vector2(-5.8f, 3.0f)},
            {4, new Vector2(-6.1f, 3.3f)},
            {5, new Vector2(-6.2f, 3.4f)},
        };

        // Start is called before the first frame update
        void Start()
        {
            // 行数取得
            this.rowNum = this.gameManagerScript.rowNum;
            // 烈数取得
            this.columnNum = this.gameManagerScript.columnNum;
            // マス数取得
            this.squareNum = this.gameManagerScript.squareNum;
            // 初期配置取得
            this.initMap = this.gameManagerScript.initMap;
            // 現在配置取得
            this.curMap = new int[this.columnNum, this.rowNum];
            Array.Copy(this.initMap, 0, this.curMap, 0, this.initMap.Length);
            // 完成配置所得
            this.compMap = this.gameManagerScript.compMap;
            // 種類順番取得
            this.fruitsTypeList = this.gameManagerScript.fruitsTypeList;
            // 種類数取得
            this.fruitsTypeNum = this.gameManagerScript.fruitsTypeNum;
            // フルーツオブジェクト配置
            this.fruitsObjectMap = new GameObject[this.columnNum, this.rowNum];

            // 左上から右に生成していく
            for(int i = 0; i < this.columnNum; i++){
                for(int j = 0; j < this.rowNum; j++){
                    // フルーツ生成
                    float x = j * this.fruitsIntervalList[this.squareNum];
                    float y = i * this.fruitsIntervalList[this.squareNum];
                    Vector3 position = new Vector3( x, -y, 0.0f);
                    float scale = this.fruitsScaleList[this.squareNum];
                    this.fruitsObjectMap[i,j] = this.getFruitsObject(this.initMap[i,j], position, scale, this.transform);
                    this.playMatchedBlink(j, i, false);
                }
            }
            // 全体のフルーツ位置を設定
            this.gameObject.transform.position = this.fruitsPositionList[this.squareNum];
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0) && !this.gameManagerScript.isClearFlg){
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Rayとオブジェクトの接触を調べる
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
                if(hit){
                    audioManagerScript.playFruitsTapSE();
                    float x = hit.collider.gameObject.transform.position.x;
                    float y = hit.collider.gameObject.transform.position.y;
                    int mapX = (int)Mathf.Round((x - this.fruitsPositionList[this.squareNum].x)/this.fruitsIntervalList[this.squareNum]);
                    int mapY = (int)Mathf.Round((-y + this.fruitsPositionList[this.squareNum].y)/this.fruitsIntervalList[this.squareNum]);
                    int curType = this.curMap[mapY,mapX];
                    // 触れたフルーツを交換
                    this.changeNextFruits(mapX, mapY);
                    this.playMatchedBlink(mapX, mapY, true);
                    // 他フルーツの交換
                    this.changeOtherFruitsObject(curType, mapX, mapY);
                    // 手数を記録
                    this.gameManagerScript.tapNum++;
                    var a = this.gameManagerScript.initMap;
                }
            }
        }

        // 他フルーツオブジェクト交換
        void changeOtherFruitsObject(int curType, int mapX, int mapY){
            List<(int,int)> hit = new List<(int,int)>();
            // 縦方向探索
            if(this.gameManagerScript.verticalFlg){
                hit.AddRange(this.searchVertical(mapX, mapY));
            }
            // 横方向探索
            if(this.gameManagerScript.horizontalFlg){
                hit.AddRange(this.searchHorizontal(mapX, mapY));
            }
            // 斜め方向探索
            if(this.gameManagerScript.diagonalFlg){
                hit.AddRange(this.searchDiagonal(mapX, mapY));
            }
            // hitしたフルーツを交換
            foreach((int x,int y) value in hit){
                this.changeNextFruits(value.x, value.y);
                this.playMatchedBlink(value.x, value.y, true);
            }
            Debug.Log("交換数：" + hit.Count);
            // 完成図と一致した場合
            if(this.isCurMapAndCompMap()){
                this.gameManagerScript.isClearFlg = true;
            }
        }

        // 次のフルーツ
        void changeNextFruits(int x, int y){
            int type = this.curMap[y,x];
            int nextType = this.getNextType(type);
            this.curMap[y,x] = nextType;
            this.fruitsObjectMap[y,x].GetComponent<SpriteRenderer>().sprite = this.gameManagerScript.fruitsSpriteList[nextType];
        }

        // フルーツオブジェクト生成（初期生成）
        GameObject getFruitsObject(int type, Vector3 position, float scale, Transform transform){
            GameObject prefab = (GameObject)this.fruitsPrefab;
            GameObject obj = Instantiate (prefab, position, prefab.transform.rotation);
            obj.GetComponent<SpriteRenderer>().sprite = this.gameManagerScript.fruitsSpriteList[type];
            obj.transform.localScale = new Vector3(scale, scale, scale);
            obj.transform.parent = transform;
            return obj;
        }

        // 次の種類取得
        int getNextType(int curType)
        {
            if(curType < this.fruitsTypeList[this.fruitsTypeNum - 1]){
                int nextNum = this.fruitsTypeList.IndexOf(curType) + 1;
                return this.fruitsTypeList[nextNum];
            }else{
                return this.fruitsTypeList[0];
            }
        }

        // 現在配置と完成図が一致している場合、点滅する
        void playMatchedBlink(int x, int y, bool audioFlg){
            Blinker blinker = this.fruitsObjectMap[y,x].GetComponent<Blinker>();
            // 現在配置と完成図が一致している場合
            if(this.curMap[y,x] == this.compMap[y,x]){
                if(audioFlg){
                    this.audioManagerScript.playFruitsMatchSE();
                }
                blinker.setMatchedBlink(true);
            }else{
                blinker.setMatchedBlink(false);
            }
        }

        public void playAllMatchedBlink(){
            for(int i = 0; i < this.rowNum; i++){
                for(int j = 0; j < this.columnNum; j++){
                    Blinker blinker = this.fruitsObjectMap[i,j].GetComponent<Blinker>();
                    blinker.setAllMatchedBlink();
                }
            }
        }

        // 現在配置と完成配置が一致しているか
        bool isCurMapAndCompMap()
        {
            for(int i = 0; i < this.rowNum; i++){
                for(int j = 0; j < this.columnNum; j++){
                    if(this.curMap[i,j] != this.compMap[i,j]){
                        return false;
                    }
                }
            }
            return true;
        }

        // 縦方向の探索
        List<(int,int)> searchVertical(int x, int y)
        {
            List<(int,int)> hit = new List<(int,int)>();
            // 上の探索
            for(int i = 1; i < y + 1; i++){
                int nextY = y - i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,x]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x, nextY + j));
                        }
                    }
                    break;
                }
            }
            // 下の探索
            for(int i = 1; i < (this.columnNum - y); i++){
                int nextY = y + i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,x]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        //changeNum += i - 1;
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x, nextY - j));
                        }
                    }
                    break;
                }
            }
            return hit;
        }

        // 横方向の探索
        List<(int,int)> searchHorizontal(int x, int y){
            List<(int,int)> hit = new List<(int,int)>();
            // 左の探索
            for(int i = 1; i < x + 1; i++){
                int nextX = x - i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[y,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((nextX + j, y));
                        }
                    }
                    break;
                }
            }
            // 右の探索
            for(int i = 1; i < (this.rowNum - x); i++){
                int nextX = x + i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[y,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((nextX - j, y));
                        }
                    }
                    break;
                }
            }
            return hit;
        }

        // 斜め方向の探索
        List<(int,int)> searchDiagonal(int x, int y){
            List<(int,int)> hit = new List<(int,int)>();
            // 右斜め上の探索
            for (int i = 1; i < Math.Min(this.rowNum - x, y + 1); i++)
            {
                int nextX = x + i;
                int nextY = y - i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x + j, y - j));
                        }
                    }
                    break;
                }
            }
            // 右斜め下の探索
            for (int i = 1; i < Math.Min(this.rowNum - x, this.columnNum - y); i++)
            {
                int nextX = x + i;
                int nextY = y + i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x + j, y + j));
                        }
                    }
                    break;
                }
            }
            //  左斜め上の探索
            for (int i = 1; i < Math.Min(x, y) + 1; i++)
            {
                int nextX = x - i;
                int nextY = y - i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x - j, y - j));
                        }
                    }
                    break;
                }
            }
            // 左斜め下の探索
            for (int i = 1; i < Math.Min(x + 1, this.columnNum - y); i++)
            {
                int nextX = x - i;
                int nextY = y + i;
                // 同じ種類があった場合
                if(this.curMap[y,x] == this.curMap[nextY,nextX]){
                    // 隣の場合、何もせずに終了
                    if(i != 1){
                        // 二つ以上離れている場合、間の種類変える
                        for(int j = 1; j < i; j++){
                            hit.Add((x - j, y + j));
                        }
                    }
                    break;
                }
            }
            return hit;
        }
    }
}
