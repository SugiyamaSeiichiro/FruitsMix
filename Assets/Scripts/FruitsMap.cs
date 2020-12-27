using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsMap : MonoBehaviour
{
    public List<GameObject> fruitsPrefabList;
    public Material markMaterial;
    public GameManager gameManagerScript;
    public GameConfig gameConfigScript;

    private int rowNum;
    private int columnNum;
    private int squareNum;
    private int txtCheckNum;
    private int[,] initMap;
    private int[,] curMap;
    private int[,] compMap;
    private GameObject[,] fruitsObjectMap;
    private List<int> fruitsTypeList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        // 初期配置取得
        this.initMap = GetConfMap("Conf/InitMap/Stage" + this.gameManagerScript.stageNum);
        // 現在配置取得
        this.curMap = this.initMap;
        // 完成配置所得
        this.compMap = GetConfMap("Conf/CompMap/Stage" + this.gameManagerScript.stageNum);
        this.gameManagerScript.compMap = this.compMap;
        // 種類ソート
        this.fruitsTypeList.Sort();
        this.gameManagerScript.fruitsTypeList = this.fruitsTypeList;
        // 種類数
        this.gameManagerScript.fruitsTypeNum = this.fruitsTypeList.Count;
        // フルーツオブジェクト配置
        this.fruitsObjectMap = new GameObject[this.columnNum, this.rowNum];
        // マス数格納
        this.squareNum = this.rowNum;
        this.gameManagerScript.squareNum = this.squareNum;

        // 左上から右に生成していく
        for(int i = 0; i < this.columnNum; i++){
            for(int j = 0; j < this.rowNum; j++){
                // フルーツ生成
                float x = j * this.gameConfigScript.fruitsIntervalList[this.squareNum];
                float y = i * this.gameConfigScript.fruitsIntervalList[this.squareNum];
                Vector3 position = new Vector3( x, -y, 0.0f);
                float scale = this.gameConfigScript.fruitsScaleList[this.squareNum];
                this.fruitsObjectMap[i,j] = this.getFruitsObject(this.initMap[i,j], position, scale, this.transform);
                this.setCurMapAndCompMapMark(j, i);
            }
        }
        // 全体のフルーツ位置を設定
        this.gameObject.transform.position = this.gameConfigScript.fruitsPositionList[this.squareNum];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !this.gameManagerScript.isClearFlg){
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //Rayとオブジェクトの接触を調べる
            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if(hit){
                float x = hit.collider.gameObject.transform.position.x;
                float y = hit.collider.gameObject.transform.position.y;
                int mapX = (int)Mathf.Ceil((x - this.gameConfigScript.fruitsPositionList[this.squareNum].x)/this.gameConfigScript.fruitsIntervalList[this.squareNum]);
                int mapY = (int)Mathf.Ceil((-y + this.gameConfigScript.fruitsPositionList[this.squareNum].y)/this.gameConfigScript.fruitsIntervalList[this.squareNum]);
                int curType = this.curMap[mapY,mapX];
                // 触れたフルーツを交換
                this.changeNextFruits(mapX, mapY);
                this.setCurMapAndCompMapMark(mapX, mapY);
                // 他フルーツの交換
                this.changeOtherFruitsObject(curType, mapX, mapY);
                // 手数を記録
                this.gameManagerScript.tapNum++;
            }
        }
    }

    // 設定ファイルの二次元配列を取得
    int[,] GetConfMap(string txtPath)
    {
        // 設定ファイル読み込み
        TextAsset textFile = (TextAsset)Resources.Load(txtPath);
        int txtCheckNum = textFile.text.Count(c => c == ',');
        if(this.txtCheckNum == 0){
            this.txtCheckNum = txtCheckNum;
        // カンマ数で初期配置と完成配置の大きさを比較
        }else if(this.txtCheckNum != txtCheckNum){
            Debug.Log("エラー：初期配置と完成配置の大きさが一致しません");
        }
        // 設定ファイルを配列変換
        string[] textData = textFile.text.Split('\n');
        int rowNum = textData[0].Split(',').Length;
        int columnNum = textData.Length;
        if(this.rowNum == 0 && this.columnNum == 0){
            this.rowNum = rowNum;
            this.columnNum = columnNum;
            this.gameManagerScript.rowNum = this.rowNum;
            this.gameManagerScript.columnNum = this.columnNum;
        }else if(this.rowNum != this.columnNum){
            Debug.Log("エラー：縦と横の数が一致しません");
        }
        // 二次元配列に格納
        int[,] textWords = new int[this.columnNum, this.rowNum];
        for(int i = 0; i < this.columnNum; i++){
            string[] tempWords = textData[i].Split(',');
            for(int j = 0; j < this.rowNum; j++){
                int type = int.Parse(tempWords[j]);
                textWords[i,j] = type;
                // フルーツの種類格納
                if(!this.fruitsTypeList.Contains(type)){
                    this.fruitsTypeList.Add(type);
                }
            }
        }
        return textWords;
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
            this.setCurMapAndCompMapMark(value.x, value.y);
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
        this.changeFruitsObject(ref this.fruitsObjectMap[y,x], nextType);
    }

    // フルーツオブジェクト生成（初期生成）
    GameObject getFruitsObject(int type, Vector3 position, float scale, Transform transform){
        GameObject prefab = (GameObject)this.fruitsPrefabList[type];
        GameObject obj = Instantiate (prefab, position, prefab.transform.rotation);
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.transform.parent = transform;
        return obj;
    }

    // 次のフルーツオブジェクト生成
    void changeFruitsObject(ref GameObject obj, int type){
        Vector3 position = obj.transform.position;
        Destroy(obj);
        GameObject prefab = (GameObject)this.fruitsPrefabList[type];
        obj = Instantiate (prefab, position, prefab.transform.rotation);
        float scale = this.gameConfigScript.fruitsScaleList[this.squareNum];
        obj.transform.localScale = new Vector3(scale, scale, scale);
        obj.transform.parent = this.transform;
    }

    // 次の種類取得
    int getNextType(int curType)
    {
        if(curType < this.fruitsTypeList[this.gameManagerScript.fruitsTypeNum - 1]){
            int nextNum = this.fruitsTypeList.IndexOf(curType) + 1;
            return this.fruitsTypeList[nextNum];
        }else{
            return this.fruitsTypeList[0];
        }
    }

    // 現在配置と完成図が一致している場合、マークする
    void setCurMapAndCompMapMark(int x, int y){
        // 現在配置と完成図が一致している場合
        if(this.curMap[y,x] == this.compMap[y,x]){
            MeshRenderer renderer = this.fruitsObjectMap[y,x].GetComponent<MeshRenderer>();
            Material[] materials = new Material[2];
            materials[0] = renderer.material;
            materials[1] = markMaterial;
            renderer.materials = materials;
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
