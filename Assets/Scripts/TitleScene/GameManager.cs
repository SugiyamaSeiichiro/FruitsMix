using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleScene
{
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.LoadScene("SelectScene"); 
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
