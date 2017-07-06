using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStageColumn
{
    StageLevel,
    TileKindNum,
    OverCondType,
    OverCondValue,
    ClearCondType,
    ClearCondValue,
    BestScore,
}
public class StagePrefs : MonoBehaviour {

    protected const float PrefsVer = 1.1f;
    protected const string StageStr = "Stage";

    #region GetprefName
    protected static string GetPrefName(EStageColumn col, int idx)
    {
        var str = col.ToString();
        str += "_" + idx;

        return str;
    }
    #endregion

    #region ValueGetSet
    public static int GetValue(EStageColumn col, int idx)
    {
        return PlayerPrefs.GetInt(GetPrefName(col, idx));
    }

    public static void SetValue(EStageColumn col, int idx, int value)
    {
        PlayerPrefs.SetInt(GetPrefName(col, idx),value);
    }
    #endregion

    #region Stage
    public static int GetStage()
    {
        return PlayerPrefs.GetInt(StageStr);
    }

    public static void SetStage(int newStage)
    {
        PlayerPrefs.SetInt(StageStr, newStage);
    }
    #endregion

    // Use this for initialization
    void Start () {
        
        if( !PlayerPrefs.HasKey("InitStagePrefs") )
        {
            #region BestScore
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 0), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 1), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 2), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 3), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 4), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 5), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 6), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 7), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 8), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.BestScore, 9), 0);
            #endregion
        }

        if (!PlayerPrefs.HasKey("InitStagePrefs") || PlayerPrefs.GetFloat("StagePrefsVer") < PrefsVer)
        {
            PlayerPrefs.SetInt("InitStagePrefs", 1);
            PlayerPrefs.SetFloat("StagePrefsVer", PrefsVer);

            SetStage(0);

            #region StageLevel
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 0), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 1), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 2), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 3), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 4), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 5), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 6), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 7), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 8), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.StageLevel, 9), 1);
            #endregion

            #region OverCondType
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,0), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,1), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,2), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,3), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,4), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,5), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,6), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,7), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,8), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondType,9), 0);
            #endregion

            #region OverCondValue
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 0), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 1), 30);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 2), 25);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 3), 20);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 4), 15);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 5), 30);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 6), 35);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 7), 30);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 8), 45);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.OverCondValue, 9), 60);
            #endregion

            #region ClearCondType
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 0), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 1), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 2), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 3), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 4), 0);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 5), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 6), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 7), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 8), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondType, 9), 1);
            #endregion

            #region ClearCondValue
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 0), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 1), 1500);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 2), 1100);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 3), 1100);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 4), 1100);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 5), 15);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 6), 35);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 7), 51);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 8), 67);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.ClearCondValue, 9), 75);
            #endregion

            #region TileKindNum
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 0), 1);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 1), 6);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 2), 7);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 3), 8);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 4), 9);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 5), 6);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 6), 6);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 7), 7);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 8), 8);
            PlayerPrefs.SetInt(GetPrefName(EStageColumn.TileKindNum, 9), 9);
            #endregion

        }
    }
}
