using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo
{
    public int Column { get; set; }
    public int Row { get; set; }
}

public class Grid {

    // Cached Tiles
    private GameObject[,] Tiles = new GameObject[Common.Rows, Common.Columns];
    

    // Tile Get/Set
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {
                return Tiles[row, column];
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        set
        {
            Tiles[row, column] = value;
        }
    }

    // Swap Tile
    public void Swap(GameObject g1, GameObject g2)
    {
        var g1Tile = g1.GetComponent<Tile>();
        var g2Tile = g2.GetComponent<Tile>();

        //get array indexes
        int g1Row = g1Tile.Row;
        int g1Column = g1Tile.Column;
        int g2Row = g2Tile.Row;
        int g2Column = g2Tile.Column;

        //swap them in the array
        var temp = Tiles[g1Row, g1Column];
        Tiles[g1Row, g1Column] = Tiles[g2Row, g2Column];
        Tiles[g2Row, g2Column] = temp;

        //swap their respective properties
        Tile.SwapTile(g1Tile, g2Tile);
    }

    // check horizontal tiles
    protected List<GameObject> GetMatchesHorizontally(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var tile = go.GetComponent<Tile>();
        int column;

        //check left
        for (column = tile.Column - 1; column >= 0; column--)
        {
            if (Tiles[tile.Row, column] != null &&
                Tiles[tile.Row, column].GetComponent<Tile>().IsSameType(tile))
            {
                matches.Add(Tiles[tile.Row, column]);
            }
            else
                break;
        }

        //check right
        for (column = tile.Column + 1; column < Common.Columns; column++)
        {
            if (Tiles[tile.Row, column] != null &&
                Tiles[tile.Row, column].GetComponent<Tile>().IsSameType(tile))
            {
                matches.Add(Tiles[tile.Row, column]);
            }
            else
                break;
        }

        //we want more than three matches
        if (matches.Count < Common.MinimumMatches)
            matches.Clear();

        return matches;
    }

    // check Vertical tiles
    protected List<GameObject> GetMatchesVertically(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var tile = go.GetComponent<Tile>();
        int row;

        //check bottom
        for (row = tile.Row - 1; row >= 0; row--)
        {
            if (Tiles[row, tile.Column] != null &&
                Tiles[row, tile.Column].GetComponent<Tile>().IsSameType(tile))
            {
                matches.Add(Tiles[row, tile.Column]);
            }
            else
                break;
        }

        //check top
        for (row = tile.Row + 1; row < Common.Rows; row++)
        {
            if (Tiles[row, tile.Column] != null &&
                Tiles[row, tile.Column].GetComponent<Tile>().IsSameType(tile))
            {
                matches.Add(Tiles[row, tile.Column]);
            }
            else
                break;
        }

        if (matches.Count < Common.MinimumMatches)
            matches.Clear();

        return matches;
    }

    public MatchesInfo GetMatches(GameObject go)
    {
        MatchesInfo matchesInfo = new MatchesInfo();

        var horizontalMatches = GetMatchesHorizontally(go);
        var verticalMatches = GetMatchesVertically(go);

        matchesInfo.AddObjectRange(horizontalMatches);
        matchesInfo.AddObjectRange(verticalMatches);

        matchesInfo.NewAbility = Tile.CalcAbility( verticalMatches.Count , horizontalMatches.Count );
        return matchesInfo;
    }

    public MatchesInfo GetAllSameTiles( GameObject go , bool onlySame = true )
    {
        MatchesInfo matchesInfo = new MatchesInfo();

        Tile t = go.GetComponent<Tile>();

        for(int row=0;row<Common.Rows;++row)
        {
            for(int col=0;col<Common.Columns;++col)
            {
                if( Tiles[row,col] != null )
                {
                    if( !onlySame || t.IsSameType( Tiles[row,col].GetComponent<Tile>() ))
                    {
                        matchesInfo.AddObject(Tiles[row, col]);
                    }
                }
            }
        }

        return matchesInfo;
    }

    public IEnumerable<GameObject> ExplodeAndRemove(GameObject item)
    {
        if (item == null) return null;
        Tile t = item.GetComponent<Tile>();

        if ( t == null || t.isExplode) return null;

        t.Explode();
        Tiles[t.Row, t.Column] = null;

        switch( t.Ability )
        {
            case ETileAbility.DestroyHorizontally:
                return GetEntireRow(item);
            case ETileAbility.DestroyVertically:
                return GetEntireColumn(item);
            case ETileAbility.DestroyNearArea:
                return GetNearAreaTiles(item);
        }
        return null;
    }

    // Get Entire Row / Column
    private IEnumerable<GameObject> GetEntireRow(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int row = go.GetComponent<Tile>().Row;
        for (int column = 0; column < Common.Columns; column++)
        {
            if(Tiles[row, column] != null )
                matches.Add(Tiles[row, column]);
        }
        return matches;
    }

    private IEnumerable<GameObject> GetEntireColumn(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int column = go.GetComponent<Tile>().Column;
        for (int row = 0; row < Common.Rows; row++)
        {
            if(Tiles[row, column] != null )
                matches.Add(Tiles[row, column]);
        }
        return matches;
    }

    private IEnumerable<GameObject> GetNearAreaTiles(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        if (go == null) return matches;

        Tile t = go.GetComponent<Tile>();
        if (t == null) return matches;
        
        for (int row = Mathf.Max(t.Row-2,0); row < Mathf.Max(t.Row+3,Common.Rows); ++row)
        {
            for( int col = Mathf.Max(t.Column - 2, 0); col < Mathf.Max(t.Column + 3, Common.Columns); ++col )
            {
                if (Tiles[row, col] != null)
                {
                    matches.Add(Tiles[row, col]);
                }
            }
        }
        return matches;
    }

    public Vector2 GetSpawnAblePosition( int Column )
    {
        return Common.BottomRight + new Vector2( Column * Common.TileSize.x, Common.Rows * Common.TileSize.y);
    }

    public IEnumerable<GameObject> GetPotentialMatches()
    {
        //list that will contain all the matches we find
        List<List<GameObject>> matches = new List<List<GameObject>>();

        for (int row = 0; row < Common.Rows; row++)
        {
            for (int column = 0; column < Common.Columns; column++)
            {
                Tile t = Tiles[row, column].GetComponent<Tile>();

                var matches1 = CheckHorizontalPotential1(row, column , t);
                var matches2 = CheckHorizontalPotential2(row, column , t);
                var matches3 = CheckHorizontalPotential3(row, column , t);
                var matches4 = CheckVerticalPotential1(row, column , t);
                var matches5 = CheckVerticalPotential2(row, column , t);
                var matches6 = CheckVerticalPotential3(row, column , t);

                if (matches1 != null) matches.Add(matches1);
                if (matches2 != null) matches.Add(matches2);
                if (matches3 != null) matches.Add(matches3);
                if (matches4 != null) matches.Add(matches4);
                if (matches5 != null) matches.Add(matches5);
                if (matches6 != null) matches.Add(matches6);

                //if we have >= 3 matches, return a random one
                if (matches.Count >= 3)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];

                //if we are in the middle of the calculations/loops
                //and we have less than 3 matches, return a random one
                if (row >= Common.Rows / 2 && matches.Count > 0 && matches.Count <= 2)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];
            }
        }
        return null;
    }

    // {{ check Horizontal Potential
    private List<GameObject> CheckHorizontalPotential1( int row , int column , Tile t )
    {
        List < GameObject > matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if ( column - 3 >= 0
            && t.IsSameType(Tiles[row, column - 2].GetComponent<Tile>())
            && t.IsSameType(Tiles[row, column - 3].GetComponent<Tile>())
            )
        {
            matches.Add(Tiles[row, column - 2]);
            matches.Add(Tiles[row, column - 3]);
            return matches;
        }

        if( column +3 < Common.Columns
            && t.IsSameType(Tiles[row, column + 2].GetComponent<Tile>())
            && t.IsSameType(Tiles[row, column + 3].GetComponent<Tile>())
            )
        {
            matches.Add(Tiles[row, column + 2]);
            matches.Add(Tiles[row, column + 3]);
            return matches;
        }

        return null;
    }

    private List<GameObject> CheckHorizontalPotential2(int row, int column, Tile t)
    {
        List<GameObject> matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if (column - 2 >= 0)
        {
            if( row - 1 >= 0
                && t.IsSameType(Tiles[row-1, column - 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row-1, column - 2].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row-1, column - 1]);
                matches.Add(Tiles[row-1, column - 2]);
                return matches;
            }

            if( row + 1 < Common.Rows
                && t.IsSameType(Tiles[row + 1, column - 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row + 1, column - 2].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row + 1, column - 1]);
                matches.Add(Tiles[row + 1, column - 2]);
                return matches;
            }
        }
        return null;
    }

    private List<GameObject> CheckHorizontalPotential3(int row, int column, Tile t)
    {
        List<GameObject> matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if (column + 2 < Common.Columns )
        {
            if (row - 1 >= 0
                && t.IsSameType(Tiles[row - 1, column + 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row - 1, column + 2].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row - 1, column + 1]);
                matches.Add(Tiles[row - 1, column + 2]);
                return matches;
            }

            if (row + 1 < Common.Rows
                && t.IsSameType(Tiles[row + 1, column + 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row + 1, column + 2].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row + 1, column + 1]);
                matches.Add(Tiles[row + 1, column + 2]);
                return matches;
            }
        }
        return null;
    }
    // }} check Horizontal Potential

    // {{ check Vertical Potential
    private List<GameObject> CheckVerticalPotential1(int row, int column, Tile t)
    {
        List<GameObject> matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if (row - 3 >= 0
            && t.IsSameType(Tiles[row - 2, column].GetComponent<Tile>())
            && t.IsSameType(Tiles[row - 3, column].GetComponent<Tile>())
            )
        {
            matches.Add(Tiles[row - 2, column]);
            matches.Add(Tiles[row - 3, column]);
            return matches;
        }

        if (row + 3 < Common.Rows
            && t.IsSameType(Tiles[row + 2, column].GetComponent<Tile>())
            && t.IsSameType(Tiles[row + 3, column].GetComponent<Tile>())
            )
        {
            matches.Add(Tiles[row + 2, column]);
            matches.Add(Tiles[row + 3, column]);
            return matches;
        }

        return null;
    }

    private List<GameObject> CheckVerticalPotential2(int row, int column, Tile t)
    {
        List<GameObject> matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if (row - 2 >= 0)
        {
            if (column - 1 >= 0
                && t.IsSameType(Tiles[row - 1, column - 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row - 2, column - 1].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row - 1, column - 1]);
                matches.Add(Tiles[row - 2, column - 1]);
                return matches;
            }

            if (column + 1 < Common.Columns
                && t.IsSameType(Tiles[row - 1, column + 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row - 2, column + 1].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row - 1, column + 1]);
                matches.Add(Tiles[row - 2, column + 1]);
                return matches;
            }
        }
        return null;
    }

    private List<GameObject> CheckVerticalPotential3(int row, int column, Tile t)
    {
        List<GameObject> matches = new List<GameObject>();

        matches.Add(t.gameObject);
        if (row + 2 < Common.Rows)
        {
            if (column - 1 >= 0
                && t.IsSameType(Tiles[row + 1, column - 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row + 2, column - 1].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row + 1, column - 1]);
                matches.Add(Tiles[row + 2, column - 1]);
                return matches;
            }

            if (column + 1 < Common.Columns
                && t.IsSameType(Tiles[row + 1, column + 1].GetComponent<Tile>())
                && t.IsSameType(Tiles[row + 2, column + 1].GetComponent<Tile>())
                )
            {
                matches.Add(Tiles[row + 1, column + 1]);
                matches.Add(Tiles[row + 2, column + 1]);
                return matches;
            }
        }
        return null;
    }
    // }} check Vertical Potential

    /// <summary>
    /// Collapses the array on the specific columns, after checking for empty items on them
    /// </summary>
    /// <param name="columns"></param>
    /// <returns>Info about the GameObjects that were moved</returns>
    public AlteredTileInfo Collapse(IEnumerable<int> columns)
    {
        AlteredTileInfo collapseInfo = new AlteredTileInfo();


        ///search in every column
        foreach (var column in columns)
        {
            //begin from bottom row
            for (int row = 0; row < Common.Rows - 1; row++)
            {
                //if you find a null item
                if (Tiles[row, column] == null)
                {
                    //start searching for the first non-null
                    for (int row2 = row + 1; row2 < Common.Rows; row2++)
                    {
                        //if you find one, bring it down (i.e. replace it with the null you found)
                        if (Tiles[row2, column] != null)
                        {
                            Tiles[row, column] = Tiles[row2, column];
                            Tiles[row2, column] = null;

                            //calculate the biggest distance
                            if (row2 - row > collapseInfo.MaxDistance)
                                collapseInfo.MaxDistance = row2 - row;

                            //assign new row and column (name does not change)
                            Tiles[row, column].GetComponent<Tile>().Row = row;
                            Tiles[row, column].GetComponent<Tile>().Column = column;

                            collapseInfo.AddTile(Tiles[row, column]);
                            break;
                        }
                    }
                }
            }
        }

        return collapseInfo;
    }

    /// <summary>
    /// Returns the matches found for a list of GameObjects
    /// MatchesInfo class is not used as this method is called on subsequent collapses/checks, 
    /// not the one inflicted by user's drag
    /// </summary>
    /// <param name="gos"></param>
    /// <returns></returns>
    public IEnumerable<GameObject> GetMatches(IEnumerable<GameObject> gos)
    {
        List<GameObject> matches = new List<GameObject>();
        foreach (var go in gos)
        {
            matches.AddRange(GetMatches(go).MatchedTiles);
        }
        return matches.Distinct();
    }

    public IEnumerable<TileInfo> GetEmptyItemsOnColumn(int column)
    {
        List<TileInfo> emptyItems = new List<TileInfo>();
        for (int row = 0; row < Common.Rows; row++)
        {
            if (Tiles[row, column] == null)
                emptyItems.Add(new TileInfo() { Row = row, Column = column });
        }
        return emptyItems;
    }
}