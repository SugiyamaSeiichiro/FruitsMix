using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelectScene
{
    public class UIManager : MonoBehaviour
    {
        public GameCommon gameCommonScript;
        public GameObject stageButtonPrefab;
        public GameObject stageButtonParent;
        private int stageNum;
        // Start is called before the first frame update
        void Start()
        {
            for(int i = 0; i < this.gameCommonScript.stageAllNum; i++){
                GameObject obj = Instantiate(this.stageButtonPrefab);
                obj.transform.SetParent(this.stageButtonParent.transform, false);
                int stageNum = i + 1;
                obj.GetComponent<StageButton>().setStageButton(stageNum);

            }
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
