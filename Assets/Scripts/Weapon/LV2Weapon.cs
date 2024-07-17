using UnityEngine;

public class LV2Weapon : BaseWeapon
{
    float fireRate = 1f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�)

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
        //TODO : ���� �Լ��� ��?
        //�Ʒ� ���ǹ��� ������ �� ���� �� �������� �ȵż� ���� ����
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
