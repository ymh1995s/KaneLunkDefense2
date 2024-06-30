using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    private Transform playerTransform;  // �÷��̾��� Transform

    [SerializeField] private float orbitRadius = 2.0f;  // ���� ������
    [SerializeField] private float orbitSpeed = 120;   // ���� �ӵ� (���� ����)
    public float currentAngle { get; set; }   // ���� ȸ�� ����
    [SerializeField] private float detectionRadius = 3f;  // Ÿ���� Ž�� �ݰ�

    LayerMask enemyLayer;           // �� ���̾�
    Collider2D enemyCollider;       // �� �ݶ��̴�
    [SerializeField] Transform target;               // Ÿ���õ� ��

    //TODO  ; �ڵ�� ����
    private GameObject bulletPrefab; // �߻��� �Ѿ� ������
    private float fireRate = 2f; // �߻� ������ �� ������ ���� (5�ʿ� �� �� �߻�)
    private float bulletSpeed = 20f;  // �Ѿ� �ӵ�
    private float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>("tempBullet");

        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            playerTransform = parentTransform;
        }
        else
        {
            Debug.LogError("Parent transform not found.");
        }

        enemyLayer = LayerMask.GetMask("Enemy");
    }

    void Update()
    {
        Orbit();
        DetectEnemy();
        Fire();
    }

    void Orbit()
    {
        if (playerTransform == null) return;

        // ���� ��ġ ����.
        Vector3 playerPosition = playerTransform.position;
        currentAngle += orbitSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

        // ������Ʈ�� ��ġ�� �÷��̾� ������ �����մϴ�.
        transform.position = playerPosition + offset;
    }

    void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        Vector2 towerPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // ���� ����� �� Ž��
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }
    }

    void Fire()
    {
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

                fireCountdown = fireRate;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}