using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class BaseMonster : MonoBehaviour
{
    private Rigidbody2D rigid;
    [SerializeField] private int searchRange = 1;
    private bool isDying = false;   // 몬스터 생존 여부
    protected enum Level { LV1, LV2, LV3, LV4, LV5, BOSS }
    private Coroutine attackCoroutine;

    //탐지 영역
    protected GameObject target; // 목표 지점
    protected GameObject commandCenter; //디폴트 목표
    private float DetectRange = 5; // 목표를 탐지하기 위한 범위
    private LayerMask[] targetLayer;    // 적 레이어

    //스텟 영역
    [SerializeField] protected int hp;
    protected float speed = 1f;
    public int attackPower = 1;
    private float attackInterval = 1.0f;
    private float lastAttackTime = 0.0f;
    protected int[] master_Hp = new int[6] { 10, 80, 180, 350, 500, 10000 };

    //오디오 영역
    public AudioClip[] deathSound = new AudioClip[5]; // 사망 사운드 종류
    private Transform playerTransform; // 플레이어 거리 비례 사운드 조절
    private AudioSource audioSource; // 컴포넌트
    private Renderer monsterRenderer; // 사망 후 사운드 재생 처리를 위한 몬스터의 렌더러
    CircleCollider2D monsterCollider;

    // 참조용 스트링 Arr
    string[] deathSoundName = new string[5] { "Sounds/SFX/아이고난1", "Sounds/SFX/아이고난2", "Sounds/SFX/아이고난3", "Sounds/SFX/아이고난4", "Sounds/SFX/아이고난5", };


    protected virtual void Start()
    {
        commandCenter = GameObject.Find("CommandCenter");

        SetAudio();
        SetSearch();
    }

    protected void Update()
    {
        ChooseTarget();
    }

    void FixedUpdate()
    {
        Move();
    }
    
    void SetAudio()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        audioSource = gameObject.AddComponent<AudioSource>();
        monsterRenderer = GetComponent<Renderer>();
        monsterCollider = GetComponent<CircleCollider2D>();
        audioSource.spatialBlend = 1.0f; // 3D 사운드 설정
        audioSource.minDistance = 1.0f; // n일 때 소리 최소, 그 이하 소리 없음
        audioSource.maxDistance = 5.0f; // n일 때 소리 최대, 그 이상 소리 최대
        LoadAudioClips();
    }

    void SetSearch()
    {
        target = GameObject.Find("tempCC");
        rigid = GetComponent<Rigidbody2D>();
        targetLayer = new LayerMask[3];
        targetLayer[0] = LayerMask.GetMask("Player");
        targetLayer[1] = LayerMask.GetMask("Tower");
        targetLayer[2] = LayerMask.GetMask("CommanCenter");
    }

    void LoadAudioClips()
    {
        for (int i = 0; i < deathSound.Length; i++)
        {
            deathSound[i] = Resources.Load<AudioClip>(deathSoundName[i]);
        }
    }

    protected virtual void ChooseTarget()
    {
        // 플레이어 > 타워 > 기지 순 어그로
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, DetectRange, targetLayer[0]);
        if (targets.Length != 0)
        {
            target = targets[0].gameObject;
            return;
        }

        targets = Physics2D.OverlapCircleAll(transform.position, DetectRange, targetLayer[1]);
        if (targets.Length != 0)
        {
            target = targets[0].gameObject;
            return;
        }

        target = commandCenter;
    }

    void Move()
    {
        if (target != null && rigid!=null)
        {
            Vector2 goal = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 direction = (goal - (Vector2)transform.position).normalized;

            // 이동 오차 값
            float randomOffsetX = Random.Range(-1f, 1f); // X축 오차
            float randomOffsetY = Random.Range(-1f, 1f); // Y축 오차
            Vector2 randomOffset = new Vector2(randomOffsetX, randomOffsetY);

            // 이동에 오차 적용
            Vector2 finalDirection = (direction + randomOffset).normalized;

            rigid.velocity = finalDirection * speed;
        }
    }


    protected virtual void TakeDamage(int damage)
    {
        hp -= damage;
        //healthBar.SetHealth(currentHealth); //TODO?

        if (hp <= 0) StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        DropItem();
        //PlayDeathSound();

        isDying = true;

        // 소리가 재생되는 동안 콜라이더와 태그 제거
        if (monsterCollider != null)
        {
            Destroy(monsterCollider); // 콜라이더 제거
        }

        gameObject.tag = "Untagged"; // 태그 제거

        if(rigid!=null)
        {            
           Destroy(rigid);
        }

        if (monsterRenderer != null)
        {
            monsterRenderer.enabled = false; // 렌더러 비활성화
        }

        // 소리의 볼륨을 업데이트하는 루프
        while (audioSource.isPlaying)
        {
            UpdateSoundVolume();
            yield return null; // 매 프레임마다 볼륨을 업데이트합니다.
        }

        Destroy(gameObject);
    }

    void UpdateSoundVolume()
    {
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            float volume = Mathf.Clamp01(1 - (distance / audioSource.maxDistance));
            volume = Mathf.Max(volume, audioSource.volume);

            audioSource.volume = volume;
        }
    }

    void PlayDeathSound()
    {
        int index = Random.Range(0, deathSound.Length);
        AudioClip clip = deathSound[index];

        audioSource.clip = clip;
        audioSource.Play();
    }

    void DropItem()
    {
        // 중복 수혜 방지
        if (isDying == true) return;

        int index = Random.Range(0, 100);
        string coin;

        //TODO? 하드코딩 제거?
        if (index < 3) coin = "Coin/Coin";
        else if (index < 15) coin = "Coin/Coin2";
        else coin = "Coin/Coin3";

        // Resources 폴더에서 아이템 프리팹을 로드
        GameObject itemPrefab = Resources.Load<GameObject>(coin);

        if (itemPrefab != null)
        {
            // 현재 오브젝트의 위치에 아이템 프리팹을 생성
            Instantiate(itemPrefab, transform.position, transform.rotation);            
        }
        else
        {
            print(" 잘못된 코인");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying == true) return;

        if (collision.CompareTag("Bullet"))
        {
            BaseProjectile projectile = collision.GetComponent<BaseProjectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.attackPower);
                projectile.Destroy();
            }
        }
        else if (collision.CompareTag("Player") || collision.CompareTag("Tower") || collision.CompareTag("CommandCenter"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }
                attackCoroutine = StartCoroutine(Attack(damageable));
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Tower"))
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack(IDamageable target)
    {
        while (target != null)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                target.TakeDamage(attackPower);
                lastAttackTime = Time.time;
            }
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
