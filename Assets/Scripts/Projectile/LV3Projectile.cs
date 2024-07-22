using UnityEngine;

public class LV3Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 3;

        hit = Resources.Load<GameObject>(hitFrefabNames[3]);  // 충돌 효과 오브젝트
        flash = Resources.Load<GameObject>(flashFrefabNames[3]);  // 발사 효과 오브젝트
    }
}
