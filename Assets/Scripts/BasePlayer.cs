using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    //TODO : 무기는 추가할거니까 이거 상속으로 빼는거 연습
    int[] levelUpGatcha = new int[100];

    Rigidbody2D rigid;
    Vector2 input;
    float p_speed = 5f;

    //무기 관리
    public GameObject[] weapon1;
    public GameObject weaponPrefab; // Inspector에서 할당할 무기 프리팹
    int maxWeaponCount = 8;

    //아이템 자석 효과
    float attractionRange = 1f; // 아이템 자석 효과 범위 (플레이어 기준)
    float attractionSpeed = 1f; // 아이템 자석효과 속도
    LayerMask itemLayer; // 아이템 레이어

    //선언과 동시에 초기화
    int w1_level = 1;
    int maxExp=1;
    int curExp=0;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        //GetWeaponInfo();

        //무기 관장
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
        // Weapon1 정보 가져오기
        weapon1 = new GameObject[maxWeaponCount];
        Transform WeaponMaster1 = transform.GetChild(0); //무기 오브젝트를 가지고 있는 부모

        for (int i = 0; i < maxWeaponCount; i++)
        {
            Transform weapon = WeaponMaster1.GetChild(i);
            weapon1[i] = weapon.gameObject;
        }

        // 가져온 모든 자식 오브젝트 출력
        // TODO : 삭제
        foreach (GameObject childObject in weapon1)
        {
            childObject.SetActive(false);
            Debug.Log("Found child: " + childObject.name);
        }
    }

    void CollectItem()
    {   
        // 플레이어 주변의 아이템 탐지
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, attractionRange, itemLayer);
        foreach (Collider2D itemCollider in items)
        {
            GameObject item = itemCollider.gameObject;
            float distanceToItem = Vector3.Distance(transform.position, item.transform.position);

            // 자석효과
            item.transform.position = Vector3.MoveTowards(item.transform.position, transform.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //아이템 획득
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject); // 총알 삭제
            CheckLevelUp();
        }

        // TODO: 피격 등
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
        //디버그 : 먹으면 무조건 레밸업
        //if (curExp < maxExp) return; 

        //Level up
        curExp = System.Math.Max(0, maxExp - curExp);
        //maxExp += 5; //본게임 때 주석 해제
        Upgrade();
        
    }

    void AddWeapon()
    {
        if (weaponPrefab != null && w1_level < 8)
        {
            // 무기 프리팹을 플레이어의 자식으로 추가
            GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정
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

    //아이템 겹쳐있는거 먹으면 궤도 삐꾸되서 코루틴으로 빼야함
    void WeaponSort(int weaponCount)
    {
        //오 근데 이방법 쓰면 미리 프리팹을 플레이어에 달아 놓은 필요가 없잖아?
        int rad = 360 / weaponCount;

        for(int i = 0; i<weaponCount;i++) 
        {
            BaseWeapon bweapon = weapon1[i].GetComponent<BaseWeapon>();
            bweapon.currentAngle = rad*i;
        }
    }

    void MakeLevelUpGatchaArr()
    {
        // 배열에 0을 3개, 1을 10개, 2를 87개 추가
        AddNumbersToArray(0, 3);
        AddNumbersToArray(1, 10);
        AddNumbersToArray(2, 87);

        // 배열을 섞기
        Shuffle(levelUpGatcha);

        // 테스트 출력
        foreach (int number in levelUpGatcha)
        {
            Debug.Log(number);
        }
    }

    void AddNumbersToArray(int number, int count)
    {
        int currentIndex = 0;
        // 배열의 빈 공간에 숫자를 추가
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


    // 플레이어의 자석 효과 범위 시각화를 위한 디버그 코드
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }

}
