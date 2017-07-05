using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIUtility : MonoBehaviour
{

    public void QuitOnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 상에서 에디터 플레이 종료
#else
        Application.Quit();
#endif // UNITY_EDITOR
    }

    public void OnClick_StageButton( int StageIndex )
    {
        LoadLevel(1);
    }

    public void LoadLevel( int LevelIndex )
    {
        SceneManager.LoadScene(LevelIndex);
    }
}
