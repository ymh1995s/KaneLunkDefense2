using UnityEngine;
using UnityEngine.UIElements;

public class BaseTower : MonoBehaviour
{
    static public int maxHP;
    [SerializeField] private int currentHp;
    [SerializeField] private int power;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;


    public LayerMask enemyLayer;    // �� ���̾�
    Collider2D enemyCollider;       // �� �ݶ��̴�
    Transform target;               // Ÿ���õ� ��

    private GameObject bulletPrefab; // �߻��� �Ѿ� ������
    private float bulletSpeed = 5f;  // �Ѿ� �ӵ�
    private float fireRate = 1f; // �߻� ������ �� ������ ���� (5�ʿ� �� �� �߻�)
    public static float fireRateMmul = 1.0f;
    public static float detectionRadius = 5f;  // Ÿ���� Ž�� �ݰ�
    private float fireCountdown = 0f;// �߻� ������ üũ�ϱ� ���� ī��Ʈ�ٿ� ����

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Rotate();
        Fire();
    }

    void Hit()
    {

    }

    void Death()
    {

    }

    void DetectEnemy()
    {
        // �ʱ�ȭ
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        // Ÿ���� ��ġ
        Vector2 towerPosition = transform.position;

        // Ž�� ���� ���� ��� Collider �˻�
        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            // Ÿ���� �� ������ �Ÿ� ���
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // �� ����� ������ Ȯ��
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }        
    }

    void Rotate()
    {
        // ���� ����� ���� �ִٸ� ó��
        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            // Ÿ���� ���� ȸ��
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void Fire()
    {         
        // �߻� ���� üũ
        fireCountdown -= Time.deltaTime;

        // ���� ����� ���� �ִٸ� ó��
        if (target != null)
        {
           
            if (fireCountdown <= 0f)
            {
                // �Ѿ� ����
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
                rb.velocity = transform.right * bulletSpeed;

                fireCountdown = (fireRate* fireRateMmul);
            }
        }
    }

    static public void HPRecovery()
    {
    }

    // Gizmos�� ����Ͽ� Ž�� �ݰ��� �ð������� ǥ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
