using UnityEngine;
using UnityEngine.UIElements;

public class BaseGuard : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int power;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;

    public float detectionRadius = 5f;  // 타워의 탐지 반경

    public LayerMask enemyLayer;    // 적 레이어
    Collider2D enemyCollider;       // 적 콜라이더
    Transform target;               // 타게팅된 적

    public GameObject bulletPrefab; // 발사할 총알 프리팹
    public float fireRate = 1f; // 발사 간격을 초 단위로 설정 (5초에 한 번 발사)
    public float bulletSpeed = 5f;  // 총알 속도
    private float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수

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
        // 초기화
        target = null;
        enemyCollider = null;
        float closestDistance = Mathf.Infinity;

        // 타워의 위치
        Vector2 towerPosition = transform.position;

        // 탐지 범위 내의 모든 Collider 검사
        Collider2D[] colliders = Physics2D.OverlapCircleAll(towerPosition, detectionRadius, enemyLayer);
        foreach (Collider2D collider in colliders)
        {
            // 타워와 적 사이의 거리 계산
            float distanceToEnemy = Vector2.Distance(towerPosition, collider.transform.position);

            // 더 가까운 적인지 확인
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                enemyCollider = collider;
            }
        }        
    }

    void Rotate()
    {
        // 가장 가까운 적이 있다면 처리
        if (enemyCollider != null)
        {
            target = enemyCollider.transform;

            // 타겟을 향해 회전
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void Fire()
    {         
        // 발사 간격 체크
        fireCountdown -= Time.deltaTime;

        // 가장 가까운 적이 있다면 처리
        if (target != null)
        {
           
            if (fireCountdown <= 0f)
            {
                // 총알 생성
                GameObject bulletGO = Instantiate(bulletPrefab, transform.position, transform.rotation);
                Rigidbody2D rb = bulletGO.GetComponent<Rigidbody2D>();
                rb.velocity = transform.right * bulletSpeed;

                fireCountdown = fireRate;
            }
        }
    }

    // Gizmos를 사용하여 탐지 반경을 시각적으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
