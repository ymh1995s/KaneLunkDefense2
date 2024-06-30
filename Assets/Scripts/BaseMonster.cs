using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class BaseMonster : MonoBehaviour
{
    GameObject target; // ��ǥ ����
    private Rigidbody2D rigid;
    [SerializeField] private int hp = 1;
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private int searchRange = 1;

    private bool isDying = false;   // ���Ͱ� ���� ����
    float DetectRange = 5; // ��ǥ�� Ž���ϱ� ���� ����

    LayerMask[] targetLayer;    // �� ���̾�

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = GameObject.Find("tempCC");
        rigid = GetComponent<Rigidbody2D>();
        targetLayer = new LayerMask[3];
        targetLayer[0] = LayerMask.GetMask("Player");
        targetLayer[1] = LayerMask.GetMask("Tower");
        targetLayer[2] = LayerMask.GetMask("CommanCenter"); //���� ������� ����
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

    void Hit()
    {
        Death();
    }

    void Death()
    {        
        SpawnItem();
        Destroy(gameObject);
        isDying = true;
    }

    void SpawnItem()
    {
        // �ߺ� ���� ����
        if (isDying == true) return;

        int index = Random.Range(0, 100);
        string coin;

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
        if (collision.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject); // �Ѿ� ����
            Hit(); //�ǰ�           
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectRange);
    }
}
