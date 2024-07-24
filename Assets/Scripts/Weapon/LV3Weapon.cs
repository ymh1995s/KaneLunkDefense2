using UnityEngine;

public class LV3Weapon : BaseWeapon
{
    protected override void Start()
    {
        base.Start(); // Call BaseWeapon's Start method
        bulletPrefab = Resources.Load<GameObject>(prefabNames[(int)Level.LV1]);
    }

    protected override void Update()
    {
        base.Update();
        Fire();
    }

    protected override void Fire()
    {
        fireRate = 0.2f; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
        base.Fire();
    }
}
