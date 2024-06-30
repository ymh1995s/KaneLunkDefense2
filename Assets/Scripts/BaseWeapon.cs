using UnityEngine;

public class BaseWeapon : MonoBehaviour
{
    private Transform playerTransform;  // 플레이어의 Transform

    [SerializeField] private float orbitRadius = 2.0f;  // 공전 반지름
    [SerializeField] private float orbitSpeed = 120;   // 공전 속도 (각도 단위)
    public float currentAngle { get; set; }   // 현재 회전 각도
    [SerializeField] private float detectionRadius = 3f;  // 타워의 탐지 반경

    LayerMask enemyLayer;           // 적 레이어
    Collider2D enemyCollider;       // 적 콜라이더
    [SerializeField] Transform target;               // 타게팅된 적

    //TODO  ; 코드로 지정
    private GameObject bulletPrefab; // 발사할 총알 프리팹
    private float fireRate = 2f; // 발사 간격을 초 단위로 설정 (5초에 한 번 발사)
    private float bulletSpeed = 20f;  // 총알 속도
    private float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수

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

        // 공전 위치 설정.
        Vector3 playerPosition = playerTransform.position;
        currentAngle += orbitSpeed * Time.deltaTime;
        float radians = currentAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0) * orbitRadius;

        // 오브젝트의 위치를 플레이어 주위로 설정합니다.
        transform.position = playerPosition + offset;
    }

    void DetectEnemy()
    {
        // 초기화
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        Vector2 towerPosition = transform.position;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // 가장 가까운 적 탐지
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }
    }

    void Fire()
    {
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