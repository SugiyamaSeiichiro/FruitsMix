using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Common;

public class SettingUI : MonoBehaviour
{
    public GameObject furitsMap;
    public GameObject shadowImage;
    public GameObject menuButtonParent;
    public Slider seSlider;
    public Slider bgmSlider;
    private bool pauseFlg = false;
    private bool audioFlg;
    private AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        this.audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        this.seSlider.value = Convert.ToInt32(this.audioManager.isSeFlg());
        this.bgmSlider.value = Convert.ToInt32(this.audioManager.isBgmFlg());
    }

    public void onClickPause(){
        this.pauseFlg = !this.pauseFlg;
        if(this.pauseFlg){
            Time.timeScale = 0f;
        }else{
            Time.timeScale = 1f;
        }
        this.furitsMap.GetComponent<PlayScene.FruitsMap>().touchFlag = !this.pauseFlg;
        this.shadowImage.SetActive(this.pauseFlg);
        this.menuButtonParent.SetActive(this.pauseFlg);
        this.audioManager.playSE(SE_TYPE.BUTTON);
    }

    public void onClickToSelectScene(){
        this.onClickPause();
        SceneManager.LoadScene(Define.SelectScene);
        this.audioManager.playSE(SE_TYPE.BUTTON);
    }

    public void onClickSE(){
        int value = (int)this.seSlider.value;
        // 音楽再生
        if(value == 1){
            this.audioManager.startSE();
        // 音楽停止
        }else{
            this.audioManager.stopSE();
        }
    }

    public void onClickBGM(){
        int value = (int)this.bgmSlider.value;
        // 音楽再生
        if(value == 1){
            this.audioManager.startBGM();
        // 音楽停止
        }else{
            this.audioManager.stopBGM();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
