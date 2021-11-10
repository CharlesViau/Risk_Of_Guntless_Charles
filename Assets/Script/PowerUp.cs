using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public void GiveExplosionRange(Collision2D col)
    {
        PlayerBehavior behavior;
        if (col.gameObject.TryGetComponent<PlayerBehavior>(out behavior))
        {
            behavior.bonusRange += 1;
            gameObject.GetComponent<ObjectBehavior>().SetInactive();
        }
    }

    public void AddMaxAmmo(Collision2D col)
    {
        Dropper dropper;
        if(col.gameObject.TryGetComponent<Dropper>(out dropper))
        {
            dropper.MaxAmmo += 1;
            dropper.CurrentAmmo += 1;
            gameObject.GetComponent<ObjectBehavior>().SetInactive();
        }
    }
}
