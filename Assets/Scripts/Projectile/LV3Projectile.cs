using UnityEngine;

public class LV3Projectile : BaseProjectile
{
    static public int attackPower_ForUI;
    protected override void Start()
    {
        base.Start();
        attackPower = master_attackPower[(int)Level.LV3] + attackPowerUp;

        hit = Resources.Load<GameObject>(hitFrefabNames[(int)Level.LV3]);  // 충돌 효과 오브젝트
        flash = Resources.Load<GameObject>(flashFrefabNames[(int)Level.LV3]);  // 발사 효과 오브젝트
    }

    private void Update()
    {
        attackPower_ForUI = attackPower + attackPowerUp; ;
    }
}
