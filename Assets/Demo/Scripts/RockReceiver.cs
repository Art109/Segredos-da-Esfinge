using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RockReceiver
{
    public void TakeRock(DemoPlayer player, DemoRock rock);

    public void RemoveRock(DemoPlayer player, DemoRock rock);
}
