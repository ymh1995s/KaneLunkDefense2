using UnityEngine;

public class LV2Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 2;

        hit = Resources.Load<GameObject>(hitFrefabNames[1]);  // 충돌 효과 오브젝트
        flash = Resources.Load<GameObject>(flashFrefabNames[1]);  // 발사 효과 오브젝트
    }
}
