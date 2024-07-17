using UnityEngine;

public class LV2Weapon : BaseWeapon
{
    float fireRate = 1f; // 발사 간격을 초 단위로 설정 (X초에 한 번 발사)

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start(); // Call BaseWeapon's Start method
    }

    // Update is called once per frame
    void Update()
    {
        Orbit();
        DetectEnemy();
        Fire();
    }

    protected override void Fire()
    {
        //TODO : 공통 함수로 뺌?
        //아래 조건문에 넣으면 적 없을 때 재장전이 안돼서 위로 놨음
        fireCountdown -= Time.deltaTime;

        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            if (fireCountdown <= 0f)
            {
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

                Vector3 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * bulletSpeed;

                fireCountdown = (fireRate * fireRateMmul);
            }
        }
    }
}
