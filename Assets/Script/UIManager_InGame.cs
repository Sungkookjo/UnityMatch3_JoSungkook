using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_InGame : MonoBehaviour {

    // public properties
    public GameObject QuitMenuObj = null;

    public GameObject ResultMenuObj = null;
    public Text ResultTxt = null;

    public Text ScoreTxt = null;
    public Text OverCondiTxt = null;
    public Text ClearCondiTxt = null;

    public static UIManager_InGame instance;

    // Use this for initialization
    void Start () {
        instance = this;		
	}
	
	// Update is called once per frame
	void Update () {
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

    public void UpdateScore(int score)
    {
        if( ScoreTxt != null )
        {
            ScoreTxt.text = "Score: " + score.ToString();
        }
    }

    public void UpdateGameClear( string txt )
    {
        if( ClearCondiTxt != null )
        {
            ClearCondiTxt.text = txt;
        }
    }

    public void UpdateGameOver( string txt )
    {
        if (OverCondiTxt != null)
        {
            OverCondiTxt.text = txt;
        }
    }

    public void ShowGameResult(bool bClear)
    {
        if (ResultMenuObj != null)
        {
            ResultMenuObj.SetActive(true);
        }

        if(ResultTxt != null )
        {
            ResultTxt.text = bClear ? "Clear" : "Over";
        }
    }
}
