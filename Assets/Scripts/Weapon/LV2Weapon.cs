using UnityEngine;

public class LV2Weapon : BaseWeapon
{    
    protected override void Start()
    {
        base.Start();
        detectionRadius = 4;  // Ÿ���� Ž�� �ݰ�
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV2]);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Fire();
    }

    protected override void Fire()
    {
        fireRate = 0.5f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)
        base.Fire();
    }
}
