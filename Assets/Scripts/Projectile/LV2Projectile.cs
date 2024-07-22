using UnityEngine;

public class LV2Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 2;

        hit = Resources.Load<GameObject>(hitFrefabNames[1]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[1]);  // �߻� ȿ�� ������Ʈ
    }
}
