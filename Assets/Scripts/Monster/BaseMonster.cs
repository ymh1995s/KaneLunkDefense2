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
    private bool isDying = false;   // ���� ���� ����
    protected enum Level { LV1, LV2, LV3, LV4, LV5, BOSS }
    private Coroutine attackCoroutine;

    //Ž�� ����
    GameObject target; // ��ǥ ����
    private float DetectRange = 5; // ��ǥ�� Ž���ϱ� ���� ����
    private LayerMask[] targetLayer;    // �� ���̾�

    //���� ����
    [SerializeField] protected int hp = 9999;
    protected float speed = 2f;
    public int attackPower = 1;
    private float attackInterval = 1.0f;
    private float lastAttackTime = 0.0f;
    protected int[] master_Hp = new int[6] { 1,2,3,4,5,10 };

    //����� ����
    public AudioClip[] deathSound = new AudioClip[5]; // ��� ���� ����
    private Transform playerTransform; // �÷��̾� �Ÿ� ��� ���� ����
    private AudioSource audioSource; // ������Ʈ
    private Renderer monsterRenderer; // ��� �� ���� ��� ó���� ���� ������ ������
    private Collider monsterCollider; // ��� �� ���� ��� ó���� ���� ������ �ݶ��̴�

    // ������ ��Ʈ�� Arr
    string[] deathSoundName = new string[5] { "Sounds/SFX/���̰�1", "Sounds/SFX/���̰�2", "Sounds/SFX/���̰�3", "Sounds/SFX/���̰�4", "Sounds/SFX/���̰�5", };


    protected virtual void Start()
    {
        SetAudio();
        SetSearch();
    }

    private void Update()
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
        monsterCollider = GetComponent<Collider>();
        audioSource.spatialBlend = 1.0f; // 3D ���� ����
        audioSource.minDistance = 1.0f; // n�� �� �Ҹ� �ּ�, �� ���� �Ҹ� ����
        audioSource.maxDistance = 5.0f; // n�� �� �Ҹ� �ִ�, �� �̻� �Ҹ� �ִ�
        LoadAudioClips();
    }

    void SetSearch()
    {
        target = GameObject.Find("tempCC");
        rigid = GetComponent<Rigidbody2D>();
        targetLayer = new LayerMask[3];
        targetLayer[0] = LayerMask.GetMask("Player");
        targetLayer[1] = LayerMask.GetMask("Tower");
        targetLayer[2] = LayerMask.GetMask("CommanCenter"); //���� ������� ����
    }

    void LoadAudioClips()
    {
        for (int i = 0; i < deathSound.Length; i++)
        {
            deathSound[i] = Resources.Load<AudioClip>(deathSoundName[i]);
        }
    }

    void ChooseTarget()
    {
        // �÷��̾� > Ÿ�� > ���� �� ��׷�
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
        target = GameObject.Find("tempCC");
    }

    void Move()
    {
        if (target != null)
        {
            Vector2 goal = new Vector2(target.transform.position.x, target.transform.position.y);
            Vector2 direction = (goal - (Vector2)transform.position).normalized;
            rigid.velocity = direction * speed;
        }
    }


    void TakeDamage(int damage)
    {
        hp -= damage;
        //healthBar.SetHealth(currentHealth); //TODO?

        if (hp <= 0) StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        DropItem();
        PlayDeathSound();

        isDying = true;
        // �Ҹ��� ����Ǵ� ���� �ݶ��̴��� �±� ����
        if (monsterCollider != null)
        {
            Destroy(monsterCollider); // �ݶ��̴� ����
        }
        gameObject.tag = "Untagged"; // �±� ����

        if (monsterRenderer != null)
        {
            monsterRenderer.enabled = false; // ������ ��Ȱ��ȭ
        }

        // �Ҹ��� ������ ������Ʈ�ϴ� ����
        while (audioSource.isPlaying)
        {
            UpdateSoundVolume();
            yield return null; // �� �����Ӹ��� ������ ������Ʈ�մϴ�.
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
        // �ߺ� ���� ����
        if (isDying == true) return;

        int index = Random.Range(0, 100);
        string coin;

        //TODO? �ϵ��ڵ� ����?
        if (index < 3) coin = "tempCoin";
        else if (index < 15) coin = "tempCoin2";
        else coin = "tempCoin3";

        // Resources �������� ������ �������� �ε�
        GameObject itemPrefab = Resources.Load<GameObject>(coin);

        if (itemPrefab != null)
        {
            // ���� ������Ʈ�� ��ġ�� ������ �������� ����
            Instantiate(itemPrefab, transform.position, transform.rotation);            
        }
        else
        {
            print(" �߸��� ����");
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
        else if (collision.CompareTag("Player") || collision.CompareTag("Tower"))
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

        //if (collision.CompareTag("Player"))
        //{
        //    StartCoroutine(AttackPlayer(collision.GetComponent<BasePlayer>()));
        //}

        //if (collision.CompareTag("Tower"))
        //{
        //    StartCoroutine(AttackTower(collision.GetComponent<BaseTower>()));
        //}
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
        //if (collision.CompareTag("Player"))
        //{
        //    StopCoroutine(AttackPlayer(collision.GetComponent<BasePlayer>()));
        //}

        //if (collision.CompareTag("Tower"))
        //{
        //    StopCoroutine(AttackTower(collision.GetComponent<BaseTower>()));
        //}
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

    //IEnumerator AttackPlayer(BasePlayer player)
    //{
    //    while (player != null)
    //    {
    //        if (Time.time - lastAttackTime >= attackInterval)
    //        {
    //            player.TakeDamage(attackPower);
    //            lastAttackTime = Time.time;
    //        }
    //        yield return null;
    //    }
    //}

    //IEnumerator AttackTower(BaseTower tower)
    //{
    //    while (tower != null)
    //    {
    //        if (Time.time - lastAttackTime >= attackInterval)
    //        {
    //            tower.TakeDamage(attackPower);
    //            lastAttackTime = Time.time;
    //        }
    //        yield return null;
    //    }
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
