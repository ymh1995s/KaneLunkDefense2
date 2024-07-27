using UnityEngine;

public class LV2Weapon : BaseWeapon
{    
    protected override void Start()
    {
        base.Start();
        detectionRadius = 4;  // 타워의 탐지 반경
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
        fireRate = 0.5f; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)
        base.Fire();
    }
}
