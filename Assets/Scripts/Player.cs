using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    protected override void ReceiveDamage(Damage dmg)
    {
        base.ReceiveDamage(dmg);
        GameManager.instance.OnHitpointChange();
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal"); // Returns -1,1 or 0 | -1 if holding A, 0 not holding any keys, 1 if holding D
        float y = Input.GetAxisRaw("Vertical"); // Returns -1,1 or 0 | -1 if holding S, 0 not holding any keys, 1 if holding

        UpdatedMotor(new Vector3(x, y, 0));
    }

}
