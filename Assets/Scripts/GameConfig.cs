using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    public Dictionary<int, float> fruitsScaleList = new Dictionary<int, float>(){
        {3, 5.0f},
        {4, 4.4f},
        {5, 3.4f},
    };
    public Dictionary<int, float> fruitsIntervalList = new Dictionary<int, float>(){
        {3, 3.0f},
        {4, 2.2f},
        {5, 1.8f},
    };
    public Dictionary<int, Vector2> fruitsPositionList = new Dictionary<int, Vector2>(){
        {3, new Vector2(-6.0f, 3.0f)},
        {4, new Vector2(-6.2f, 3.2f)},
        {5, new Vector2(-6.4f, 3.6f)},
    };
    public Dictionary<int, float> fruitsCompMapUIScaleList = new Dictionary<int, float>(){
        {3, 40.0f},
        {4, 30.0f},
        {5, 25.0f},
    };
    public Dictionary<int, float> fruitsCompMapUIIntervalList = new Dictionary<int, float>(){
        {3, 50.0f},
        {4, 35.0f},
        {5, 28.0f},
    };
    public Dictionary<int, Vector2> fruitsCompMapUIPositionList = new Dictionary<int, Vector2>(){
        {3, new Vector2(585.0f, 190.0f)},
        {4, new Vector2(580.0f, 195.0f)},
        {5, new Vector2(578.0f, 198.0f)},
    }; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
