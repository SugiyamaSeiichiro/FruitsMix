using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SE_TYPE
{
    FRUITS_TAP,
    FRUITS_MATCH,
    FRUITS_ALL_MATCH,
    GAME_CLEAR,
    BUTTON,
}

public enum BGM_TYPE
{
    TITLE_SCENE,
    SELECT_SCENE,
    PLAY_SCENE,
    RESULT_SCENE,
}

public class AudioManager : MonoBehaviour
{
    public AudioSource seAudioSource;
    public AudioSource bgmAudioSource;
    public List<AudioClip> seAudioClipList;
    public List<AudioClip> bgmAudioClipList;

    private string audioInfoKey = "audioInfo";
    private AudioInfo audioInfo = new AudioInfo();

    public class AudioInfo
    {
        public bool seFlg = true;
        public bool bgmFlg = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        AudioInfo audioInfo = PlayerPrefsUtils.GetObject<AudioInfo>(this.audioInfoKey);
        if(audioInfo != null){
            this.audioInfo.seFlg = audioInfo.seFlg;
            this.audioInfo.bgmFlg = audioInfo.bgmFlg;
        }
    }

    public void playSE(SE_TYPE type){
        if(this.isSeFlg() == false){
            return;
        }
        AudioClip audioClip = this.seAudioClipList[(int)type];
        this.seAudioSource.PlayOneShot(audioClip);
    }

    public void playBGM(BGM_TYPE type){
        AudioClip audioClip = this.bgmAudioClipList[(int)type];
        // 同じ音楽の場合、続けて再生
        bool audioClipFlg = this.bgmAudioSource.clip != null &&
                            this.bgmAudioSource.clip.samples != audioClip.samples;
        if(this.bgmAudioSource.clip == null || audioClipFlg){
            this.bgmAudioSource.clip = audioClip;
        }else{
            return;
        }
        if(this.isBgmFlg() == false){
            this.bgmAudioSource.Stop();
            return;
        }
        this.bgmAudioSource.Play();
    }

    public void startSE(){
        this.audioInfo.seFlg = true;
        PlayerPrefsUtils.SetObject(this.audioInfoKey, this.audioInfo);
    }

    public void stopSE(){
        this.audioInfo.seFlg = false;
        PlayerPrefsUtils.SetObject(this.audioInfoKey, this.audioInfo);
        this.seAudioSource.Stop();
    }

    public void startBGM(){
        this.audioInfo.bgmFlg = true;
        PlayerPrefsUtils.SetObject(this.audioInfoKey, this.audioInfo);
        this.bgmAudioSource.Play();
    }

    public void stopBGM(){
        this.audioInfo.bgmFlg = false;
        PlayerPrefsUtils.SetObject(this.audioInfoKey, this.audioInfo);
        this.bgmAudioSource.Stop();
    }

    public bool isSeFlg(){
        return this.audioInfo.seFlg;
    }

    public bool isBgmFlg(){
        return this.audioInfo.bgmFlg;
    }
}
