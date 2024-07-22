using UnityEngine;
using UnityEngine.UIElements;

public class BaseTower : MonoBehaviour
{
    static public int maxHP = 10;
    [SerializeField] private int currentHp;
    public static int attackPower;
    [SerializeField] private int attackRange;
    [SerializeField] private int attackCoolTime;

    //체력바 관련
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    public LayerMask enemyLayer;    // 적 레이어
    Collider2D enemyCollider;       // 적 콜라이더
    Transform target;               // 타게팅된 적

    private GameObject bulletPrefab; // 발사할 총알 프리팹
    private float bulletSpeed = 5f;  // 총알 속도
    private float fireRate = 1f; // 발사 간격을 초 단위로 설정 (5초에 한 번 발사)
    public static float fireRateMmul = 1.0f;
    public static float detectionRadius = 5f;  // 타워의 탐지 반경
    private float fireCountdown = 0f;// 발사 간격을 체크하기 위한 카운트다운 변수

    protected string[] prefabNames = { "Projectile/Basic", "Projectile/ADVBasic", "Projectile/ICE", "Projectile/FIRE", "Projectile/Special2" }; // 사용할 프리팹 이름들

    void Start()
    {
        bulletPrefab = Resources.Load<GameObject>(prefabNames[0]);
        currentHp = maxHP;
        // 하위 하이러라키에서 healthBarForeground를 할당
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        DetectEnemy();
        Rotate();
        Fire();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, maxHP);
        UpdateHealthBar();

        if (currentHp <= 0)
        {
            Death();
        }
    }

    void Death()
    {

    }

    void UpdateHealthBar()
    {
        // 체력 비율 계산
        float healthPercent = (float)currentHp / maxHP;

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
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

                fireCountdown = (fireRate* fireRateMmul);
            }
        }
    }

    static public void HPRecovery()
    {
    }

    // Gizmos를 사용하여 탐지 반경을 시각적으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
