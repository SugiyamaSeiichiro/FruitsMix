using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AspectKeeper : MonoBehaviour
{
    void Awake() {

        // 横画面で開発している場合は以下の用に切り替えます
        float developAspect = 1920.0f / 1080.0f;
        
        // 実機のサイズを取得して、縦横比取得
        float deviceAspect = (float)Screen.width / (float)Screen.height;

        // 実機と開発画面との対比
        float scale = deviceAspect / developAspect;

        Camera mainCamera = Camera.main;

        // カメラに設定していたorthographicSizeを実機との対比でスケール
        float deviceSize = mainCamera.orthographicSize;
        // scaleの逆数
        float deviceScale = 1.0f / scale;
        // orthographicSizeを計算し直す
        mainCamera.orthographicSize = deviceSize * deviceScale;

    }
    // [SerializeField]
    // private Camera targetCamera; //対象とするカメラ

    // [SerializeField]
    // private Vector2 aspectVec; //目的解像度

    // // Update is called once per frame
    // void Update()
    // {
    //     var screenAspect = Screen.width / (float)Screen.height; //画面のアスペクト比
    //     var targetAspect = aspectVec.x / aspectVec.y; //目的のアスペクト比
    //     var magRate = targetAspect / screenAspect; //目的アスペクト比にするための倍率
    //     var viewportRect = new Rect(0, 0, 1, 1); //Viewport初期値でRectを作成

    //     if (magRate < 1)
    //     {
    //         viewportRect.width = magRate; //使用する横幅を変更
    //         viewportRect.x = 0.5f - viewportRect.width * 0.5f;//中央寄せ
    //     }
    //     else
    //     {
    //         viewportRect.height = 1 / magRate; //使用する縦幅を変更
    //         viewportRect.y = 0.5f - viewportRect.height * 0.5f;//中央余生
    //     }

    //     targetCamera.rect = viewportRect; //カメラのViewportに適用
    // }
}
