using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    GameObject me; // 自分のオブジェクト取得用変数
    public float fadeStart = 1f; // フェード開始時間
    public bool fadeFlg = true; // trueの場合はフェードイン
    public float fadeSpeed = 1f; // フェード速度指定


    // Start is called before the first frame update
    void Start()
    {
        me = this.gameObject; // 自分のオブジェクト取得
    }

    // Update is called once per frame
    void Update()
    {
        if(this.fadeFlg){
            this.fadeIn();
        }else{
            this.fadeOut();
        }
    }

    void fadeIn()
    {
        if(me.GetComponent<Text>().color.a < 255f/255f)
        {
            UnityEngine.Color tmp = me.GetComponent<Text>().color;
            tmp.a = tmp.a + (Time.deltaTime * fadeSpeed);
            me.GetComponent<Text>().color = tmp;
        }else{
            this.fadeFlg = false;
        }
    }

    void fadeOut()
    {
        if(me.GetComponent<Text>().color.a > 0f/255f)
        {
            UnityEngine.Color tmp = me.GetComponent<Text>().color;
            tmp.a = tmp.a - (Time.deltaTime * fadeSpeed);
            me.GetComponent<Text>().color = tmp;
        }else{
            this.fadeFlg = true;
        }
    }
}
