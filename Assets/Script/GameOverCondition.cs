using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameOverCondition
{
    Time,
    Turn,
}

public class GameOverCondition
{
    public EGameOverCondition type;
    public float fCond;
    public int iCond;
    public GameObject oCond;

    public void UpdateTime( float DeltaTime )
    {
        if( type == EGameOverCondition.Time )
        {
            fCond -= DeltaTime;

            if( fCond <= 0.0f )
            {
                fCond = 0.0f;
                NotiGameOver();
            }

            NotiUpdateUI();
        }
    }

    public void DecreaseTurn()
    {
        if( type == EGameOverCondition.Turn )
        {
            iCond -= 1;

            if( iCond <= 0 )
            {
                NotiGameOver();
            }

            NotiUpdateUI();
        }
    }

    protected void NotiGameOver()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.NotifyGameOver();
        }
    }

    public string GetTypeStr()
    {
        return type.ToString();
    }

    public string GetCondStr()
    {
        switch( type )
        {
            case EGameOverCondition.Time:
                return fCond.ToString();
            case EGameOverCondition.Turn:
                return iCond.ToString();
        }

        return "";
    }

    public void NotiUpdateUI()
    {
        if( UIManager_InGame.instance != null )
        {
            UIManager_InGame.instance.UpdateGameOver(GetTypeStr() + " : " + GetCondStr());
        }
    }
}
