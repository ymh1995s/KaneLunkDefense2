using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    //TODO : ����� �߰��ҰŴϱ� �̰� ������� ���°� ����
    int[] levelUpGatcha = new int[100];

    Rigidbody2D rigid;
    Vector2 input;
    float p_speed = 5f;

    //���� ����
    public GameObject[] weapon1;
    public GameObject weaponPrefab; // Inspector���� �Ҵ��� ���� ������
    int maxWeaponCount = 8;

    //������ �ڼ� ȿ��
    float attractionRange = 1f; // ������ �ڼ� ȿ�� ���� (�÷��̾� ����)
    float attractionSpeed = 1f; // ������ �ڼ�ȿ�� �ӵ�
    LayerMask itemLayer; // ������ ���̾�

    //����� ���ÿ� �ʱ�ȭ
    int w1_level = 1;
    int maxExp=1;
    int curExp=0;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        //GetWeaponInfo();

        //���� ����
        weapon1 = new GameObject[maxWeaponCount];
        weaponPrefab = Resources.Load<GameObject>("tempWeapon");
        MakeLevelUpGatchaArr();
        AddWeapon();
    }

    private void Update()
    {
        CollectItem();
    }


    void GetWeaponInfo()
    {
        // Weapon1 ���� ��������
        weapon1 = new GameObject[maxWeaponCount];
        Transform WeaponMaster1 = transform.GetChild(0); //���� ������Ʈ�� ������ �ִ� �θ�

        for (int i = 0; i < maxWeaponCount; i++)
        {
            Transform weapon = WeaponMaster1.GetChild(i);
            weapon1[i] = weapon.gameObject;
        }

        // ������ ��� �ڽ� ������Ʈ ���
        // TODO : ����
        foreach (GameObject childObject in weapon1)
        {
            childObject.SetActive(false);
            Debug.Log("Found child: " + childObject.name);
        }
    }

    void CollectItem()
    {   
        // �÷��̾� �ֺ��� ������ Ž��
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, attractionRange, itemLayer);
        foreach (Collider2D itemCollider in items)
        {
            GameObject item = itemCollider.gameObject;
            float distanceToItem = Vector3.Distance(transform.position, item.transform.position);

            // �ڼ�ȿ��
            item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //������ ȹ��
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject); // �Ѿ� ����
            CheckLevelUp();
        }

        // TODO: �ǰ� ��
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(input.x, input.y).normalized * p_speed;
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }

    void CheckLevelUp()
    {
        //����� : ������ ������ �����
        //if (curExp < maxExp) return; 

        //Level up
        curExp = System.Math.Max(0, maxExp - curExp);
        //maxExp += 5; //������ �� �ּ� ����
        Upgrade();
        
    }

    void AddWeapon()
    {
        if (weaponPrefab != null && w1_level < 8)
        {
            // ���� �������� �÷��̾��� �ڽ����� �߰�
            GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����
            WeaponSort(w1_level++);
        }
        else
        {
            Debug.LogWarning("Weapon Prefab is not assigned.");
        }
    }

    void Upgrade()
    {
        int index = Random.Range(0, 100);
        //print(index);
        string temp;
        AddWeapon();
        //weapon1[w1_level++].SetActive(true);
        //WeaponSort(w1_level);

        //if (index < 10) print("1");
        //else if (index < 20) print("2");
        //else if (index < 30) print("3");
        //else if (index < 40) print("4");
        //else if (index < 50) print("5");
        //else if (index < 60) print("6");
        //else if (index < 70) print("7");
        //else if (index < 80) print("8");
        //else if (index < 90) print("9");
        //else print("10");
    }

    //������ �����ִ°� ������ �˵� �߲ٵǼ� �ڷ�ƾ���� ������
    void WeaponSort(int weaponCount)
    {
        //�� �ٵ� �̹�� ���� �̸� �������� �÷��̾ �޾� ���� �ʿ䰡 ���ݾ�?
        int rad = 360 / weaponCount;

        for(int i = 0; i<weaponCount;i++) 
        {
            BaseWeapon bweapon = weapon1[i].GetComponent<BaseWeapon>();
            bweapon.currentAngle = rad*i;
        }
    }

    void MakeLevelUpGatchaArr()
    {
        // �迭�� 0�� 3��, 1�� 10��, 2�� 87�� �߰�
        AddNumbersToArray(0, 3);
        AddNumbersToArray(1, 10);
        AddNumbersToArray(2, 87);

        // �迭�� ����
        Shuffle(levelUpGatcha);

        // �׽�Ʈ ���
        foreach (int number in levelUpGatcha)
        {
            Debug.Log(number);
        }
    }

    void AddNumbersToArray(int number, int count)
    {
        int currentIndex = 0;
        // �迭�� �� ������ ���ڸ� �߰�
        for (int i = 0; i < count; i++)
        {
            while (levelUpGatcha[currentIndex] != 0)
            {
                currentIndex++;
            }
            levelUpGatcha[currentIndex] = number;
        }
    }

    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    void Hit()
    {

    }

    void Death()
    {

    }


    // �÷��̾��� �ڼ� ȿ�� ���� �ð�ȭ�� ���� ����� �ڵ�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }

}
