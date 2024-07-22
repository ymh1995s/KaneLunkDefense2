using UnityEngine;

public class LV3Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 3;

        hit = Resources.Load<GameObject>(hitFrefabNames[3]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[3]);  // �߻� ȿ�� ������Ʈ
    }
}
