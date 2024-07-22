using UnityEngine;

public class BossMonster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = 10;
    }
}
