using UnityEngine;

public class LV1Projectile : BaseProjectile
{
    protected override void Start()
    {
        base.Start();
        attackPower = 1;

        //TODO : enum���� ��ü
        hit = Resources.Load<GameObject>(hitFrefabNames[0]);  // �浹 ȿ�� ������Ʈ
        flash = Resources.Load<GameObject>(flashFrefabNames[0]);  // �߻� ȿ�� ������Ʈ
    }
}
