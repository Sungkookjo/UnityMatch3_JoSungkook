using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileAbility
{
    None,
    DestroyHorizontally,
    DestroyVertically,
    DestroyNearArea,
    DestroySameTile,
    Max,
}

public class Tile : MonoBehaviour {

    public enum ETileState
    {
        None,
        Move,
    }

    public Sprite[] TileSprites = new Sprite[(int)ETileAbility.Max];
    protected ETileState State { get; set;  }
    protected Vector3 MoveGoal { get; set;  }
    protected float MoveDuration { get; set; }

    public bool isExplode { get; set; }
    public ETileAbility Ability { get; set; }
    public int Column { get; set; }
    public int Row { get; set; }
    public int score = 0;

    public string Type { get; set; }

    public Tile()
    {
        Ability = ETileAbility.None;
        State = ETileState.None;
    }

    public void SetSpecailAbility( ETileAbility NewAbil )
    {
        if (NewAbil == Ability) return;

        Ability = NewAbil;

        gameObject.GetComponent<SpriteRenderer>().sprite = TileSprites[(int)Ability];

        if (SoundManager.instance != null)
            SoundManager.instance.PlayGetAbilitySound();
    }

    // Is same type?
    public bool IsSameType(Tile Other)
    {
        if (Other == null || !(Other is Tile))
            throw new ArgumentException("Other Tile");

        return string.Compare(this.Type, (Other as Tile).Type) == 0;
    }

    public void Assign(string type, int row, int column)
    {

        if (string.IsNullOrEmpty(type))
            throw new ArgumentException("type");

        Column = column;
        Row = row;
        Type = type;
    }

    public bool IsNeighborTile( Tile other )
    {
        return (Column == other.Column ||
                        Row == other.Row)
                        && Mathf.Abs(Column - other.Column) <= 1
                        && Mathf.Abs(Row - other.Row) <= 1;
    }

    public void MoveTo( Vector3 position ,float movetime )
    {
        MoveGoal = position;
        MoveDuration = movetime;
        State = ETileState.Move;
    }

    public static void SwapTile(Tile a, Tile b)
    {
        int temp = a.Row;
        a.Row = b.Row;
        b.Row = temp;

        temp = a.Column;
        a.Column = b.Column;
        b.Column = temp;
    }

    public int GetScore()
    {
        return score;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if( State == ETileState.Move )
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, MoveGoal, Time.deltaTime / MoveDuration);

            MoveDuration -= Time.deltaTime;

            if( MoveDuration <= 0 )
            {
                gameObject.transform.position = MoveGoal;
                State = ETileState.None;
            }
        }
    }

    public void Explode()
    {
        // Play explosion effect
        //GameObject explosion = GetRandomExplosion();
        //var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
        //Destroy(newExplosion, Constants.ExplosionDuration);

        isExplode = true;
    }

    public static ETileAbility CalcAbility(int VertNum, int HorizNum)
    {
        if (VertNum >= Common.SameTileBombMatches || HorizNum >= Common.SameTileBombMatches)
        {
            return ETileAbility.DestroySameTile;
        }
        if (VertNum >= Common.AreaBombMatches && HorizNum >= Common.AreaBombMatches)
        {
            return ETileAbility.DestroyNearArea;
        }
        if (VertNum >= Common.HorizonBombMatches)
        {
            return ETileAbility.DestroyHorizontally;
        }
        if (HorizNum >= Common.VerticalBombMatches)
        {
            return ETileAbility.DestroyVertically;
        }

        return ETileAbility.None;
    }
}
