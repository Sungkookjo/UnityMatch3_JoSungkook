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

    public void OnClick_CustomStageButton()
    {
        StagePrefs.SetStage(0);

        if (MainMenu.instance != null && MainMenu.instance.CustomStageMenu != null)
        {
            MainMenu.instance.ShowCustomStageMenu();
        }
        else
        {
            LoadLevel(1);
        }
    }

    public void OnClick_StageButton( int StageIndex )
    {
        StagePrefs.SetStage(StageIndex);

        if (MainMenu.instance != null && MainMenu.instance.StageMenu != null)
        {
            MainMenu.instance.ShowStageMenu();
        }
        else
        {
            LoadLevel(1);
        }
    }

    public void LoadStage()
    {
        var StageIndex = StagePrefs.GetStage();
        var StageLevel = StagePrefs.GetValue(EStageColumn.StageLevel, StageIndex);

        LoadLevel(StageLevel);
    }

    public void LoadLevel( int LevelIndex )
    {
        SceneManager.LoadScene(LevelIndex);
    }

    public void ReplayLevel()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
