using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AlteredTileInfo
{
    private List<GameObject> newTile { get; set; }
    public int MaxDistance { get; set; }

    /// <summary>
    /// Returns distinct list of altered candy
    /// </summary>
    public IEnumerable<GameObject> AlteredTile
    {
        get
        {
            return newTile.Distinct();
        }
    }

    public void AddTile(GameObject go)
    {
        if (!newTile.Contains(go))
            newTile.Add(go);
    }

    public AlteredTileInfo()
    {
        newTile = new List<GameObject>();
    }
}
