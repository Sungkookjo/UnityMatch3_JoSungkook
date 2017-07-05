using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// code from http://studentguru.gr/b/dt008/archive/2015/02/25/building-a-match-3-game-like-candy-crush-in-unity
// graphics from http://opengameart.org/content/candy-pack-1

public enum EGameState
{
    Idle,
    SelectionStarted,
    Animating,
}

public enum EGameResult
{
    Playing,
    Clear,
    Over,
    Result,
}

public class GameManager : MonoBehaviour {
    public EGameResult GameResult;
    public GameClearCondition gameClearCondi = null;
    public GameOverCondition gameOverCondi = null;

    public bool bIsCheat;
    // {{ 
    protected int TileKindNum = 0;
    protected int ScoreGrade = 0;
    // }} 

    public static GameManager Instance;

    protected Grid GameGrid;

    private int score;
    private int combo;
    private int stage;

    private EGameState state = EGameState.Idle;
    private GameObject hitGo = null;
    private Vector2[] SpawnPositions;
    public GameObject[] TilePrefabs;

    private IEnumerator CheckPotentialMatchesCoroutine;
    private IEnumerator AnimatePotentialMatchesCoroutine;

    IEnumerable<GameObject> potentialMatches;

    // Use this for initialization
    void Start()
    {
        Instance = this;

        InitializeGameSettings();

        InitializePrefab();

        InitializeGridAndStartPosition();

        StartCheckForPotentialMatches();

        GameResult = EGameResult.Playing;
    }

    public void AfterUIMgrInstanced()
    {
        if( gameOverCondi != null )
        {
            gameOverCondi.NotiUpdateUI();
        }

        if (gameClearCondi != null)
        {
            gameClearCondi.NotiUpdateUI();
        }

        ShowScore();

        if (UIManager_InGame.instance != null)
            UIManager_InGame.instance.UpdateStageTitle(stage);
    }

    private void InitializeGameSettings()
    {
        if( TileKindNum <= 0 )
        {
            TileKindNum = TilePrefabs.Length;
        }

        hitGo = null;
        state = EGameState.Idle;

        stage = StagePrefs.GetStage();

        // {{ set Over Conditions
        gameOverCondi = new GameOverCondition();
        gameOverCondi.InitCond(
            StagePrefs.GetValue(EStageColumn.OverCondType, stage),
            StagePrefs.GetValue(EStageColumn.OverCondValue, stage)
            );
        // }} 

        // {{ set Clear Conditions
        gameClearCondi = new GameClearCondition();
        gameClearCondi.InitCond(
            StagePrefs.GetValue(EStageColumn.ClearCondType, stage),
            StagePrefs.GetValue(EStageColumn.ClearCondValue, stage)
            );
        // }}

        AfterUIMgrInstanced();
}

    private void InitializePrefab()
    {
        //just assign the name of the prefab
        foreach (var item in TilePrefabs)
        {
            item.GetComponent<Tile>().Type = item.name;
        }
    }

    // {{ about Score
    private void InitializeVariables()
    {
        score = 0;
        ShowScore();
    }

    private void IncreaseScore(int amount)
    {
        score += amount;

        if (gameClearCondi != null)
            gameClearCondi.UpdateScore(score);
        
        ShowScore();
    }

    private void ShowScore()
    {
        if (UIManager_InGame.instance != null)
            UIManager_InGame.instance.UpdateScore(score);
    }
    // }} about Score

    public void NotifyGameOver()
    {
        if (GameResult == EGameResult.Playing)
            GameResult = EGameResult.Over;
    }

    public void NotifyGameClear()
    {
        if(GameResult == EGameResult.Playing)
            GameResult = EGameResult.Clear;
    }

    public void ShowResult( bool bClear )
    {
        if (UIManager_InGame.instance != null)
        {
            UIManager_InGame.instance.ShowGameResult(bClear);
        }

        gameOverCondi = null;
        gameClearCondi = null;

        GameResult = EGameResult.Result;

        if(bClear)
        {
            var BestScore = StagePrefs.GetValue(EStageColumn.BestScore, stage);
            if (BestScore < score)
            {
                StagePrefs.SetValue(EStageColumn.BestScore, stage, score);
            }
        }

    }

    private GameObject GetRandomTile()
    {
        return TilePrefabs[ UnityEngine.Random.Range(0, TileKindNum )];
    }

    private void InstantiateAndPlaceNewTile(int row, int column, GameObject newTile)
    {
        GameObject go = Instantiate(newTile,
            Common.BottomRight + new Vector2(column * Common.TileSize.x, row * Common.TileSize.y), Quaternion.identity)
            as GameObject;
        
        //assign the specific properties
        go.GetComponent<Tile>().Assign(newTile.GetComponent<Tile>().Type, row, column);
        GameGrid[row, column] = go;
    }

    private void SetupSpawnPositions()
    {
        //create the spawn positions for the new shapes (will pop from the 'ceiling')
        for (int column = 0; column < Common.Columns; column++)
        {
            SpawnPositions[column] = GameGrid.GetSpawnAblePosition(column);
        }
    }

    private void DestroyAllTiles()
    {
        if(GameGrid == null)
        {
            return;
        }
        for (int row = 0; row < Common.Rows; row++)
        {
            for (int column = 0; column < Common.Columns; column++)
            {
                if(GameGrid[row, column] != null )
                {
                    Destroy(GameGrid[row, column]);
                }
            }
        }
    }

    public void InitializeGridAndStartPosition()
    {
        InitializeVariables();

        if (GameGrid != null)
            DestroyAllTiles();

        GameGrid = new Grid();
        SpawnPositions = new Vector2[Common.Columns];

        for (int row = 0; row < Common.Rows; row++)
        {
            for (int column = 0; column < Common.Columns; column++)
            {

                GameObject newTile = GetRandomTile();

                //check if two previous horizontal are of the same type
                while (column >= 2 && GameGrid[row, column - 1].GetComponent<Tile>()
                    .IsSameType(newTile.GetComponent<Tile>())
                    && GameGrid[row, column - 2].GetComponent<Tile>().IsSameType(newTile.GetComponent<Tile>()))
                {
                    newTile = GetRandomTile();
                }

                //check if two previous vertical are of the same type
                while (row >= 2 && GameGrid[row - 1, column].GetComponent<Tile>()
                    .IsSameType(newTile.GetComponent<Tile>())
                    && GameGrid[row - 2, column].GetComponent<Tile>().IsSameType(newTile.GetComponent<Tile>()))
                {
                    newTile = GetRandomTile();
                }

                InstantiateAndPlaceNewTile(row, column, newTile);

            }
        }

        SetupSpawnPositions();
    }

    private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
    {
        SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
        SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
        if (sp1.sortingOrder <= sp2.sortingOrder)
        {
            sp1.sortingOrder = 1;
            sp2.sortingOrder = 0;
        }
    }

    // {{ Check potential matches
    private void StartCheckForPotentialMatches()
    {
        StopCheckForPotentialMatches();
        //get a reference to stop it later
        CheckPotentialMatchesCoroutine = CheckPotentialMatches();
        StartCoroutine(CheckPotentialMatchesCoroutine);
    }

    private void StopCheckForPotentialMatches()
    {
        if (AnimatePotentialMatchesCoroutine != null)
            StopCoroutine(AnimatePotentialMatchesCoroutine);
        if (CheckPotentialMatchesCoroutine != null)
            StopCoroutine(CheckPotentialMatchesCoroutine);
        ResetOpacityOnPotentialMatches();
    }

    private IEnumerator CheckPotentialMatches()
    {
        yield return new WaitForSeconds(Common.WaitBeforePotentialMatchesCheck);
        potentialMatches = GameGrid.GetPotentialMatches();
        if (potentialMatches != null)
        {
            while (true)
            {
                AnimatePotentialMatchesCoroutine = AnimatePotentialMatches(potentialMatches);
                StartCoroutine(AnimatePotentialMatchesCoroutine);
                yield return new WaitForSeconds(Common.WaitBeforePotentialMatchesCheck);
            }
        }
    }

    private void ResetOpacityOnPotentialMatches()
    {
        if (potentialMatches != null)
        {
            foreach (var item in potentialMatches)
            {
                if (item == null) break;

                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = 1.0f;
                item.GetComponent<SpriteRenderer>().color = c;
            }
        }
    }
    // }} Check potential matches

    public static IEnumerator AnimatePotentialMatches(IEnumerable<GameObject> potentialMatches)
    {
        for (float i = 1f; i >= 0.5f; i -= 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }
            yield return new WaitForSeconds(Common.OpacityAnimationFrameDelay);
        }
        for (float i = 0.5f; i <= 1f; i += 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }
            yield return new WaitForSeconds(Common.OpacityAnimationFrameDelay);
        }
    }

    void UpdateIdle()
    {
        switch(GameResult)
        {
            case EGameResult.Clear:
                ShowResult( true );
                return;
            case EGameResult.Over:
                ShowResult(false);                
                return;
            case EGameResult.Result:
                return;
        }

        //user has clicked or touched
        if (Input.GetMouseButtonDown(0))
        {
            //get the hit position
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null) //we have a hit!!!
            {
                hitGo = hit.collider.gameObject;
                state = EGameState.SelectionStarted;
            }
        }
    }

    void UpdateSelectionStarted()
    {
        //user dragged
        if (Input.GetMouseButton(0))
        {
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            //we have a hit
            if (hit.collider != null && hitGo != hit.collider.gameObject)
            {
                //user did a hit, no need to show him hints 
                StopCheckForPotentialMatches();

                //if the two shapes are diagonally aligned (different row and column), just return
                if (!hitGo.GetComponent<Tile>().IsNeighborTile( hit.collider.gameObject.GetComponent<Tile>()))
                {
                    state = EGameState.Idle;
                }
                else
                {
                    state = EGameState.Animating;
                    StartCoroutine(FindMatchesAndCollapse(hit));
                }
            }
        }
    }

    void Update()
    {
        switch( state )
        {
            case EGameState.Idle:
                UpdateIdle();
                break;
            case EGameState.SelectionStarted:
                UpdateSelectionStarted();                
                break;
        }

        if (gameOverCondi != null)
            gameOverCondi.UpdateTime(Time.deltaTime);
    }

    private void RemoveFromScene(GameObject item)
    {
        if (item != null)
        {
            if (gameClearCondi != null)
                gameClearCondi.OnRemoveTile(item);

            Tile t = item.GetComponent<Tile>();
            if (t != null)
            {
                IncreaseScore(t.GetScore());
            }
            Destroy(item);
        }
    }

    private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    {
        //get the second item that was part of the swipe
        var hitGo2 = hit2.collider.gameObject;
        var t1 = hitGo.GetComponent<Tile>();
        var t2 = hitGo2.GetComponent<Tile>();
        List<GameObject> totalMatches;

        totalMatches = new List<GameObject>();
        
        if ( !bIsCheat && (t1.Ability == ETileAbility.DestroySameTile || t2.Ability == ETileAbility.DestroySameTile) )
        {
            var targetGo = hitGo;
            var tragetTile = t1;

            if (t1.Ability == ETileAbility.DestroySameTile)
            {
                targetGo = hitGo2;
                tragetTile = t2;
            }

            yield return new WaitForSeconds(Common.AnimationDuration);

            MatchesInfo MatchInfo = null;

            totalMatches.Add(hitGo);
            totalMatches.Add(hitGo2);

            MatchInfo = GameGrid.GetAllSameTiles(targetGo, tragetTile.Ability != ETileAbility.DestroySameTile);

            totalMatches.AddRange(MatchInfo.MatchedTiles);

            if(tragetTile.Ability != ETileAbility.None )
            {
                foreach (var item in totalMatches)
                {
                    yield return new WaitForSeconds(Common.SameTileBombMotionDelay);

                    if (item == null) continue;

                    Tile t = item.GetComponent<Tile>();

                    if (t == null) continue;

                    t.SetSpecailAbility(tragetTile.Ability);
                }
            }
        }
        else
        {
            GameGrid.Swap(hitGo, hitGo2);

            //move the swapped ones
            t1.MoveTo(hitGo2.transform.position, Common.AnimationDuration);
            t2.MoveTo(hitGo.transform.position, Common.AnimationDuration);
            yield return new WaitForSeconds(Common.AnimationDuration);

            if (bIsCheat)
            {
                state = EGameState.Idle;
                StartCheckForPotentialMatches();
                yield break;
            }

            MatchesInfo MatchInfo = null;
            // Calc first hit obj bonus
            MatchInfo = GameGrid.GetMatches(hitGo);
            totalMatches.AddRange(MatchInfo.MatchedTiles);
            if (t1.Ability == ETileAbility.None && MatchInfo.NewAbility != ETileAbility.None)
            {
                totalMatches.Remove(hitGo);
                t1.SetSpecailAbility(MatchInfo.NewAbility);
            }

            // clac second hit obj bonus
            MatchInfo = GameGrid.GetMatches(hitGo2);

            totalMatches.AddRange(MatchInfo.MatchedTiles);
            if (t2.Ability == ETileAbility.None && MatchInfo.NewAbility != ETileAbility.None)
            {
                totalMatches.Remove(hitGo2);
                t2.SetSpecailAbility(MatchInfo.NewAbility);
            }

            // if can't find matches.
            if (totalMatches.Count < Common.MinimumMatches)
            {
                t1.MoveTo(hitGo2.transform.position, Common.AnimationDuration);
                t2.MoveTo(hitGo.transform.position, Common.AnimationDuration);
                yield return new WaitForSeconds(Common.AnimationDuration);

                // undo swap
                GameGrid.Swap(hitGo, hitGo2);
            }
        }

        if (gameOverCondi != null && totalMatches.Count >= Common.MinimumMatches )
            gameOverCondi.DecreaseTurn();
        
        combo = 1;
        List<int> columns = new List<int>();
        while (totalMatches.Count >= Common.MinimumMatches)
        {
            List<GameObject> pendingDestroy = new List<GameObject>();

            if(SoundManager.instance != null )
                SoundManager.instance.PlayEffectSound();

            foreach (var item in totalMatches)
            {
                if (item == null) continue;

                Tile t = item.GetComponent<Tile>();

                if (t == null) continue;

                IEnumerable<GameObject> NewDestroyTarget;
                NewDestroyTarget = GameGrid.ExplodeAndRemove(item);
                RemoveFromScene(item);

                if(NewDestroyTarget != null && NewDestroyTarget.Count() > 0 )
                {
                    IncreaseCombo();
                    pendingDestroy.AddRange(NewDestroyTarget);
                }
                columns.Add(t.Column);
            }

            if(pendingDestroy.Count > 0)
            {
                pendingDestroy.Distinct();
                totalMatches = pendingDestroy;
                yield return new WaitForSeconds(Common.ComboTurm);
            }
            else
            {
                columns.Distinct();

                //the order the 2 methods below get called is important!!!
                //collapse the ones gone
                var collapsedTileInfo = GameGrid.Collapse(columns);
                //create new ones
                var newTileInfo = CreateNewTileInSpecificColumns(columns);
                
                int maxDistance = Mathf.Max(collapsedTileInfo.MaxDistance, newTileInfo.MaxDistance);
                
                MoveAndAnimate(newTileInfo.AlteredTile, maxDistance);
                MoveAndAnimate(collapsedTileInfo.AlteredTile, maxDistance);
                
                //will wait for both of the above animations
                yield return new WaitForSeconds(Common.MoveAnimationMinDuration * maxDistance);

                //search if there are matches with the new/collapsed items
                totalMatches.Clear();
                totalMatches.AddRange( GameGrid.GetMatches(collapsedTileInfo.AlteredTile).
                    Union(GameGrid.GetMatches(newTileInfo.AlteredTile)).Distinct() );

                if (totalMatches.Count >= Common.MinimumMatches)
                {
                    IncreaseCombo();
                }
            }
        }

        state = EGameState.Idle;
        StartCheckForPotentialMatches();
    }

    protected void IncreaseCombo()
    {
        IncreaseScore(Common.ComboScore * combo);
        ++combo;
    }

    /// <summary>
    /// Spawns new candy in columns that have missing ones
    /// </summary>
    /// <param name="columnsWithMissingTile"></param>
    /// <returns>Info about new candies created</returns>
    private AlteredTileInfo CreateNewTileInSpecificColumns(IEnumerable<int> columnsWithMissingTile)
    {
        AlteredTileInfo newTileInfo = new AlteredTileInfo();

        //find how many null values the column has
        foreach (int column in columnsWithMissingTile)
        {
            var emptyItems = GameGrid.GetEmptyItemsOnColumn(column);
            foreach (var item in emptyItems)
            {
                var go = GetRandomTile();
                GameObject newTile = Instantiate(go, SpawnPositions[column], Quaternion.identity)
                    as GameObject;

                newTile.GetComponent<Tile>().Assign(go.GetComponent<Tile>().Type, item.Row, item.Column);

                if (Common.Rows - item.Row > newTileInfo.MaxDistance)
                    newTileInfo.MaxDistance = Common.Rows - item.Row;

                GameGrid[item.Row, item.Column] = newTile;
                newTileInfo.AddTile(newTile);
            }
        }
        return newTileInfo;
    }

    /// <summary>
    /// Animates gameobjects to their new position
    /// </summary>
    /// <param name="movedGameObjects"></param>
    private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
    {
        foreach (var item in movedGameObjects)
        {
            item.GetComponent<Tile>().MoveTo( Common.BottomRight +
                new Vector2(item.GetComponent<Tile>().Column * Common.TileSize.x, item.GetComponent<Tile>().Row * Common.TileSize.y) , Common.MoveAnimationMinDuration * distance );
        }
    }
}