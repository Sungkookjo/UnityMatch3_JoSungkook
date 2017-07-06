using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMenu : MonoBehaviour
{
    public Text stageTxt;

    public Text stageOverTxt;
    public Text stageClearTxt;
    public Text stageTileTxt;
    public Text stageHighScoreTxt;

    public void InitStageInfo()
    {
        int stage = StagePrefs.GetStage();

        if(stageTxt != null )
        {
            // Stage
            if (stage > 0)
            {
                stageTxt.text = stage + " Stage";
            }
            else
            {
                stageTxt.text = "Custom";
            }

            // Over cond
            if( stageOverTxt != null )
            {
                stageOverTxt.text = ((EGameOverCondition)StagePrefs.GetValue(EStageColumn.OverCondType, stage)).ToString()
                    + " Limit : " 
                    + StagePrefs.GetValue(EStageColumn.OverCondValue,stage);
            }

            // Clear Cond
            if( stageClearTxt != null )
            {
                stageClearTxt.text = ((EGameClearCondition)StagePrefs.GetValue(EStageColumn.ClearCondType, stage)).ToString()
                    + " : " 
                    + StagePrefs.GetValue(EStageColumn.ClearCondValue, stage);
            }

            // Tiles num
            if(stageTileTxt != null )
            {
                stageTileTxt.text = "Tiles : " + StagePrefs.GetValue(EStageColumn.TileKindNum, stage);
            }

            // High score
            if(stageHighScoreTxt != null )
            {
                stageHighScoreTxt.text = "Best : "+ StagePrefs.GetValue(EStageColumn.BestScore, stage);
            }
        }
    }
}
