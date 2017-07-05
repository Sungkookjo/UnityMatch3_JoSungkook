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
    protected EGameClearCondition type;
    protected float fCond;
    protected int iCond;
    protected GameObject oCond;

    public void InitCond(int condType, int condVal)
    {
        type = (EGameClearCondition)condType;

        fCond = (float)condVal;
        iCond = condVal;
    }

    public void UpdateScore( int score )
    {
        if( type == EGameClearCondition.Score )
        {
            if ( score >= iCond )
            {
                NotiGameClear();
            }
            // NotiUpdateUI();
        }
    }

    public void OnRemoveTile( GameObject item)
    {
        if( type == EGameClearCondition.Explode )
        {
            --iCond;

            NotiUpdateUI();

            if (iCond <= 0)
            {
                NotiGameClear();
            }
        }
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