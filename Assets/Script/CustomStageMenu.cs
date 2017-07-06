using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomStageMenu : MonoBehaviour {

    private const int customStageIndex = 0;

    public Text stageHighScoreTxt;

    protected bool bOverTypeInitilized = false;
    public Dropdown overTypeBox;

    protected bool bOverValInitilized = false;
    public Dropdown overValBox;

    protected bool bClearTypeInitilized = false;
    public Dropdown clearTypeBox;

    protected bool bClearValInitilized = false;
    public Dropdown clearValBox;

    protected bool bTileNumInitilized = false;
    public Dropdown tileNumBox;
    
    protected List<string> GetOverTypes()
    {
        List<string> retval = new List<string>();

        for (EGameOverCondition val = (EGameOverCondition)0; val < EGameOverCondition.Max; ++val)
        {
            retval.Add(val.ToString());
        }

        return retval;
    }

    protected List<string> GetOverValues()
    {
        List<string> retval = new List<string>();

        switch ((EGameOverCondition)StagePrefs.GetValue(EStageColumn.OverCondType, customStageIndex))
        {
            case EGameOverCondition.Time:
                retval.Add("30");
                retval.Add("60");
                retval.Add("120");
                retval.Add("150");
                retval.Add("180");
                break;
            case EGameOverCondition.Turn:
                retval.Add("15");
                retval.Add("45");
                retval.Add("60");
                retval.Add("80");
                break;
        }

        return retval;
    }

    protected List<string> GetClearTypes()
    {
        List<string> retval = new List<string>();

        for (EGameClearCondition val = (EGameClearCondition)0; val < EGameClearCondition.Max; ++val)
        {
            retval.Add(val.ToString());
        }

        return retval;
    }

    protected List<string> GetClearValues()
    {
        List<string> retval = new List<string>();

        switch( (EGameClearCondition)StagePrefs.GetValue(EStageColumn.ClearCondType,customStageIndex) )
        {
            case EGameClearCondition.Score:
                retval.Add("1100");
                retval.Add("1500");
                retval.Add("2000");
                retval.Add("3000");
                break;
            case EGameClearCondition.Explode:
                retval.Add("15");
                retval.Add("45");
                retval.Add("60");
                retval.Add("80");
                break;
        }

        return retval;
    }

    protected List<string> GetTilesNum()
    {
        List<string> retval = new List<string>();

        // 4~8
        for (int val = 4; val < 9; ++val)
        {
            retval.Add(val.ToString());
        }

        return retval;
    }

    public void OnChange_overType( Dropdown box)
    {
        int value = box.value;
        StagePrefs.SetValue(EStageColumn.OverCondType, customStageIndex, value);

        if( overValBox != null )
        {
            overValBox.ClearOptions();
            overValBox.AddOptions(GetOverValues());
            overValBox.value = 0;
        }
    }

    public void OnChange_overVal( Dropdown box )
    {
        int value = int.Parse(box.options[box.value].text);
        StagePrefs.SetValue(EStageColumn.OverCondValue, customStageIndex, value);
    }

    public void OnChange_clearType(Dropdown box)
    {
        int value = box.value;
        StagePrefs.SetValue(EStageColumn.ClearCondType, customStageIndex, value);

        if (clearValBox != null)
        {
            clearValBox.ClearOptions();
            clearValBox.AddOptions(GetClearValues());
            clearValBox.value = 0;
        }
    }

    public void OnChange_clearVal(Dropdown box)
    {
        int value = int.Parse(box.options[box.value].text);
        StagePrefs.SetValue(EStageColumn.ClearCondValue, customStageIndex, value);
    }

    public void OnChange_tileNum(Dropdown box)
    {
        int value = int.Parse( box.options[box.value].text );
        StagePrefs.SetValue(EStageColumn.TileKindNum, customStageIndex, value);
    }

    public void InitStageInfo()
    {
        StagePrefs.SetStage(customStageIndex);

        // over type
        if ( overTypeBox != null && !bOverTypeInitilized )
        {
            overTypeBox.ClearOptions();
            overTypeBox.AddOptions(GetOverTypes());
            overTypeBox.value = StagePrefs.GetValue(EStageColumn.OverCondType, customStageIndex);
            overTypeBox.onValueChanged.AddListener(delegate { OnChange_overType(overTypeBox); });

            bOverTypeInitilized = true;
        }

        // over val
        if (overValBox != null && !bOverValInitilized)
        {
            overValBox.ClearOptions();
            overValBox.AddOptions(GetOverValues());
            overValBox.value = StagePrefs.GetValue(EStageColumn.TileKindNum, customStageIndex);
            overValBox.onValueChanged.AddListener(delegate { OnChange_overVal(overValBox); });

            bOverValInitilized = true;
        }

        // clear type
        if (clearTypeBox != null && !bClearTypeInitilized )
        {
            clearTypeBox.ClearOptions();
            clearTypeBox.AddOptions(GetClearTypes());
            clearTypeBox.value = StagePrefs.GetValue(EStageColumn.OverCondType, customStageIndex);
            clearTypeBox.onValueChanged.AddListener(delegate { OnChange_clearType(clearTypeBox); });
            bClearTypeInitilized = true;
        }

        // clear val
        if (clearValBox != null && !bClearValInitilized )
        {
            clearValBox.ClearOptions();
            clearValBox.AddOptions(GetClearValues());
            clearValBox.value = StagePrefs.GetValue(EStageColumn.OverCondType, customStageIndex);
            clearValBox.onValueChanged.AddListener(delegate { OnChange_clearVal(clearValBox); });
            bClearValInitilized = true;
        }

        // Tiles num
        if (tileNumBox != null && !bTileNumInitilized )
        {
            tileNumBox.ClearOptions();
            tileNumBox.AddOptions(GetTilesNum());
            tileNumBox.value = StagePrefs.GetValue(EStageColumn.TileKindNum, customStageIndex);
            tileNumBox.onValueChanged.AddListener(delegate { OnChange_tileNum(tileNumBox); });
            bTileNumInitilized = true;
        }

        // High score
        if (stageHighScoreTxt != null)
        {
            stageHighScoreTxt.text = "Best : " + StagePrefs.GetValue(EStageColumn.BestScore, customStageIndex);
        }
    }
}
