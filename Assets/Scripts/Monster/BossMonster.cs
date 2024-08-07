using UnityEngine;

public class BossMonster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        hp = master_Hp[(int)Level.BOSS];
        speed = 0.5f;
    }

    protected override void ChooseTarget()
    {
        target = commandCenter;
    }

    protected override void TakeDamage(int damage)
    {
        hp -= damage;

        if(hp < 0)
        {
            SceneLoader.SceneLoad_ClearScene();
        }
    }
}

