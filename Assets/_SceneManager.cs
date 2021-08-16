using UnityEngine;

public class _SceneManager : MonoBehaviour
{
#if false
#region Singleton
    static public _SceneManager instance;

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
#endif
}
