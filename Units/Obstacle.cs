using System;
using UnityEngine;

public class Obstacle : Unit
{

    protected override void OnDeath(object sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        Destroy(gameObject);
    }
}
