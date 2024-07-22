using UnityEngine;

public class LV1Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 1;

        //TODO : enum으로 교체
        hit = Resources.Load<GameObject>(hitFrefabNames[0]);  // 충돌 효과 오브젝트
        flash = Resources.Load<GameObject>(flashFrefabNames[0]);  // 발사 효과 오브젝트
    }
}
