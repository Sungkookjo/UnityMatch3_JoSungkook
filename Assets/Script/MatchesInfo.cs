using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MatchesInfo {

    private List<GameObject> matchedTiles;

    public IEnumerable<GameObject> MatchedTiles
    {
        get
        {
            return matchedTiles.Distinct();
        }
    }

    public void AddObject(GameObject go)
    {
        if (!matchedTiles.Contains(go))
            matchedTiles.Add(go);
    }

    public void AddObjectRange(IEnumerable<GameObject> gos)
    {
        foreach (var item in gos)
        {
            AddObject(item);
        }
    }

    public MatchesInfo()
    {
        matchedTiles = new List<GameObject>();
        NewAbility = ETileAbility.None;
    }
    
    public ETileAbility NewAbility { get; set; }
}
