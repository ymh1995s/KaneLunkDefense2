using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class BaseMonster : MonoBehaviour
{
    GameObject target; // 목표 지점
    private Rigidbody2D rigid;
    [SerializeField] protected int hp = 9999;
    protected float speed =10f;
    [SerializeField] private int searchRange = 1;

    public int attackPower = 1;
    private float attackInterval = 1.0f;
    private float lastAttackTime = 0.0f;

    private bool isDying = false;   // 몬스터가 생존 여부
    private float DetectRange = 5; // 목표를 탐지하기 위한 범위
    private LayerMask[] targetLayer;    // 적 레이어

    protected virtual void Start()
    {
        target = GameObject.Find("tempCC");
        rigid = GetComponent<Rigidbody2D>();
        targetLayer = new LayerMask[3];
        targetLayer[0] = LayerMask.GetMask("Player");
        targetLayer[1] = LayerMask.GetMask("Tower");
        targetLayer[2] = LayerMask.GetMask("CommanCenter"); //실제 사용하진 않음
    }

    private void Update()
    {
        ChooseTarget();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ChooseTarget()
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

        if (hp <= 0)
        {
            Death();
        }
    }

    void Death()
    {        
        DropItem();
        Destroy(gameObject);
        isDying = true;
    }

    void DropItem()
    {
        // 중복 수혜 방지
        if (isDying == true) return;

        int index = Random.Range(0, 100);
        string coin;

        if (index < 3) coin = "tempCoin";
        else if (index < 15) coin = "tempCoin2";
        else coin = "tempCoin3";

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
        if (collision.CompareTag("Bullet"))
        {
            BaseProjectile projectile = collision.GetComponent<BaseProjectile>();
            if (projectile != null)
            {
                TakeDamage(projectile.attackPower);
                projectile.Destroy();
            }   
        }

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(AttackPlayer(collision.GetComponent<BasePlayer>()));
        }

        if (collision.CompareTag("Tower"))
        {
            StartCoroutine(AttackTower(collision.GetComponent<BaseTower>()));
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StopCoroutine(AttackPlayer(collision.GetComponent<BasePlayer>()));
        }

        if (collision.CompareTag("Tower"))
        {
            StopCoroutine(AttackTower(collision.GetComponent<BaseTower>()));
        }
    }

    IEnumerator AttackPlayer(BasePlayer player)
    {
        while (player != null)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                player.TakeDamage(attackPower);
                lastAttackTime = Time.time;
            }
            yield return null;
        }
    }

    IEnumerator AttackTower(BaseTower tower)
    {
        while (tower != null)
        {
            if (Time.time - lastAttackTime >= attackInterval)
            {
                tower.TakeDamage(attackPower);
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
