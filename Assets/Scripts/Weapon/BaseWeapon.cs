using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    protected Transform playerTransform;  // �÷��̾��� Transform

    protected float orbitRadius = 1f;  // ���� ������
    protected float orbitSpeed = 120;   // ���� �ӵ� (���� ����)
    public float currentAngle { get; set; }   // ���� ȸ�� ����

    protected LayerMask enemyLayer;           // �� ���̾�
    protected  Collider2D enemyCollider;       // �� �ݶ��̴�
    [SerializeField] protected Transform target;               // Ÿ���õ� ��

    protected GameObject bulletPrefab; // �߻��� �Ѿ� ������
    protected float bulletSpeed = 15f;  // �Ѿ� �ӵ�
    protected float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����

    //protected float fireRate = 2f; // �߻� ������ �� ������ ���� (X�ʿ� �� �� �߻�) => ��� �鿩���鼭 �ڽ� Ŭ�������� ����
    public static float fireRateMmul = 1.0f;

    protected float detectionRadius = 2f;  // Ÿ���� Ž�� �ݰ�
    public static float detectionRadiusMul = 1.0f;

    protected virtual void Start()
    {
        //bulletPrefab = Resources.Load<GameObject>("TESTORIGIN");
        bulletPrefab = Resources.Load<GameObject>("TEST");

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

    protected virtual void Update()
    {
        Orbit();
        DetectEnemy();
        Fire();
    }

    protected void Orbit()
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

    protected void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        Vector2 towerPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, (detectionRadius* detectionRadiusMul), enemyLayer);
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

    protected virtual void Fire()
    {
        //��� �����鼭 ���� ����
        ////�Ʒ� ���ǹ��� ������ �� ���� �� �������� �ȵż� ���� ����
        //fireCountdown -= Time.deltaTime;

        //if (enemyCollider != null)
        //{
        //    target = enemyCollider.transform;
            
        //    if (fireCountdown <= 0f)
        //    {
        //        GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //        Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();

        //        Vector3 direction = (target.position - transform.position).normalized;
        //        rb.velocity = direction * bulletSpeed;

        //        fireCountdown = (fireRate* fireRateMmul);
        //    }
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, (detectionRadius * detectionRadiusMul));
    }
}