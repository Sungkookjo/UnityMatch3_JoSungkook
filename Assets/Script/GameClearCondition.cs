using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameClearCondition
{
    Score,
    Explode,
}

public class GameClearCondition
{
    public EGameClearCondition type;
    public float fCond;
    public int iCond;
    public GameObject oCond;

    public void UpdateScore( int score )
    {
        if( type == EGameClearCondition.Score )
        {
            if( score >= iCond )
            {
                NotiGameClear();
            }
            NotiUpdateUI();
        }
    }

    public void OnRemoveTile( GameObject item)
    {

    }

    protected void NotiGameClear()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyGameClear();
        }
    }

    public string GetTypeStr()
    {
        return type.ToString();
    }

    public string GetCondStr()
    {
        switch (type)
        {
            case EGameClearCondition.Score:
                return iCond.ToString();
            case EGameClearCondition.Explode:
                return iCond.ToString();
        }

        return "";
    }

    public void NotiUpdateUI()
    {
        if (UIManager_InGame.instance != null)
        {
            UIManager_InGame.instance.UpdateGameClear(GetTypeStr() + " : " + GetCondStr());
        }
    }
}