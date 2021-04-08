using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Common;

namespace TitleScene
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonDown(0)){
                AudioManager audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
                audioManager.playSE(SE_TYPE.BUTTON);
                SceneManager.LoadScene(Define.SelectScene);
            }
        }
    }
}
