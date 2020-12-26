using UnityEngine;

public class SceneManager : MonoBehaviour
{

#region Singleton
    static public SceneManager instance;

    void  Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
#endregion

    public enum SceneStateEnum
    {
        eTitle,
        eSelect,
        eMain,
    }
    SceneStateEnum eSceneState;

    void Start()
    {
        eSceneState = SceneStateEnum.eTitle;
    }
    void Update()
    {
#if false
        switch(eSceneState)
        {
            case SceneStateEnum.eTitle:
            break;
            case SceneStateEnum.eSelect:
            break;
            case SceneStateEnum.eMain:
            break;
        }
#endif
    }
}
