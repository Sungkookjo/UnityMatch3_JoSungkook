using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // public properties
    public GameObject QuitMenuObj = null;

    // Use this for initialization
    void Start() {
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
}
