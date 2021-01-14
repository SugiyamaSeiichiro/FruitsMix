using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinker : MonoBehaviour
{
    public float matchedBlinkHue;
    public float matchedBlinkVal;
    public float matchedBlinkTime;

    public float allMatchedBlinkHue;
    public float allMatchedBlinkVal;
    public float allMatchedBlinkTime;
    public int allMatchedBlinkCount;

    private Coroutine matchedBlinkCoroutine = null;

    public void setMatchedBlink(bool flg){
        if(flg == true && this.matchedBlinkCoroutine == null){
            this.matchedBlinkCoroutine = StartCoroutine(this.Blink(this.matchedBlinkHue, this.matchedBlinkVal, this.matchedBlinkTime));
        }else if(flg == false && this.matchedBlinkCoroutine != null){
            StopCoroutine(this.matchedBlinkCoroutine);
            this.matchedBlinkCoroutine = null;
            SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
            renderer.material.SetFloat("_Hue", 1.0f);
            renderer.material.SetFloat("_Val", 1.0f);
        }
    }

    public void setAllMatchedBlink(){
        if(this.matchedBlinkCoroutine != null){
            StopCoroutine(this.matchedBlinkCoroutine);
            this.matchedBlinkCoroutine = null;
        }
        SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
        renderer.material.SetFloat("_Hue", 1.0f);
        renderer.material.SetFloat("_Val", 1.0f);
        StartCoroutine(this.Blink(this.allMatchedBlinkHue, this.allMatchedBlinkVal, this.allMatchedBlinkTime, this.allMatchedBlinkCount));
    }

    IEnumerator Blink(float hue, float val, float waitTime, int blinkCount = 0) {
        int count = 0;
        while ( true ) {
            if(blinkCount != 0){
                if(count > blinkCount) yield break;
                count++;
            }
            SpriteRenderer renderer = this.gameObject.GetComponent<SpriteRenderer>();
            if(renderer.material.GetFloat("_Val") == 1.0f){
                renderer.material.SetFloat("_Hue", hue);
                renderer.material.SetFloat("_Val", val);
            }else{
                renderer.material.SetFloat("_Hue", 1.0f);
                renderer.material.SetFloat("_Val", 1.0f);
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
}
