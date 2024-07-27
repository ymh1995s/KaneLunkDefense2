using UnityEngine;

public class LV3Weapon : BaseWeapon
{
    protected override void Start()
    {
        base.Start();
        detectionRadius = 5;  // Ÿ���� Ž�� �ݰ�
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV3]);
    }

    protected override void Update()
    {
        base.Update();
        Fire();
    }

    protected override void Fire()
    {
        fireRate = 0.2f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.Fire();
    }
}
