using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Common{
    // 
    public static readonly Vector2 BottomRight = new Vector2(-2.5f, -3.5f);
    public static readonly Vector2 TileSize = new Vector2(0.62f, 0.62f);

    public static readonly int Rows = 10;
    public static readonly int Columns = 9;

    public static readonly float AnimationDuration = 0.2f;
    public static readonly float MoveAnimationMinDuration = 0.08f;
    public static readonly float ExplosionDuration = 0.3f;
    public static readonly float ComboTurm = 0.1f;

    public static readonly float WaitBeforePotentialMatchesCheck = 2f;
    public static readonly float OpacityAnimationFrameDelay = 0.05f;

    public static readonly float SameTileBombMotionDelay = 0.1f;

    public static readonly int PerTileScore = 10;
    public static readonly int ComboScore = 20;
    
    public static readonly int MinimumMatches = 3;
    public static readonly int HorizonBombMatches = 4;
    public static readonly int VerticalBombMatches = 4;
    public static readonly int AreaBombMatches = 3;
    public static readonly int SameTileBombMatches = 5;

    public static readonly int ClearScore = 1500;
}