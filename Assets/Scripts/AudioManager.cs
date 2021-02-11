using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip fruitsTapSE;
    public AudioClip fruitsMatchSE;
    public AudioClip fruitsAllMatchSE;
    public AudioClip gameClearSE;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playFruitsTapSE(){
        this.audioSource.PlayOneShot(fruitsTapSE);
    }

    public void playFruitsMatchSE(){
        this.audioSource.PlayOneShot(fruitsMatchSE);
    }

    public void playGameClearSE(){
        this.audioSource.PlayOneShot(fruitsAllMatchSE);
        StartCoroutine(this.playGameClearSE(1.5f));
    }

    private IEnumerator playGameClearSE(float delay) {
        yield return new WaitForSeconds(delay);
        this.audioSource.PlayOneShot(gameClearSE);
    }
}
