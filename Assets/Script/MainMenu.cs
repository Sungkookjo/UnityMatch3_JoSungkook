using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public static MainMenu instance = null;
        
    // public properties
    public GameObject QuitMenuObj = null;
    public GameObject StageMenu = null;
    public GameObject CustomStageMenu = null;

    // Use this for initialization
    void Start() {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // 백버튼 입력 시
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 종료 메뉴 토글
            if (QuitMenuObj != null)
            {
                QuitMenuObj.SetActive(!QuitMenuObj.activeInHierarchy);
            }
        }
    }

    public void ShowStageMenu()
    {
        if( StageMenu != null )
        {
            StageMenu.SetActive(true);
            if( StageMenu.GetComponent<StageMenu>() != null )
            {
                StageMenu.GetComponent<StageMenu>().InitStageInfo();
            }
        }
    }

    public void ShowCustomStageMenu()
    {
        if (CustomStageMenu != null)
        {
            CustomStageMenu.SetActive(true);

            if (CustomStageMenu.GetComponent<CustomStageMenu>() != null)
            {
                CustomStageMenu.GetComponent<CustomStageMenu>().InitStageInfo();
            }
        }
    }
}
