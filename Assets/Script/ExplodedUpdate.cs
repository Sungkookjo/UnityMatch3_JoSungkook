using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodedUpdate : MonoBehaviour
{
    private Vector3 scale = new Vector3(0,0,0);

    public void Explosion( GameObject go, ETileAbility Abil )
    {
        var goSR = go.GetComponent<SpriteRenderer>();
        var mySR = gameObject.GetComponent<SpriteRenderer>();

        if (goSR != null && mySR != null)
        {
            mySR.sprite = goSR.sprite;
        }

        switch(Abil)
        {
            case ETileAbility.DestroyHorizontally:
                scale.x = (50.0f / Common.ExplosionDuration);
                break;
            case ETileAbility.DestroyVertically:
                scale.y = (50.0f / Common.ExplosionDuration);
                break;
            case ETileAbility.DestroyNearArea:
                scale.x = scale.y = scale.z = (5.04f / Common.ExplosionDuration);
                break;
        }        
    }

    // Update is called once per frame
    void Update () {
        gameObject.transform.localScale = gameObject.transform.localScale + (scale * Time.deltaTime);
    }
}
