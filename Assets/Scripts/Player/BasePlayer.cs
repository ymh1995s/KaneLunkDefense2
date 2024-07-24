using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour, IDamageable
{
    Rigidbody2D rigid;
    Vector2 input;

    //스텟 영역
    static public float moveSpeed = 5f;
    static public int maxHP = 10;
    static public int currentHP = 10;

    //무기 관리
    public GameObject[] weapon1;
    public GameObject weaponPrefab; 
    const int maxWeaponCount = 8;
    GameObject LV1WeaponPrefab; 
    GameObject LV2WeaponPrefab; 
    GameObject LV3WeaponPrefab; 

    //아이템 자석 효과
    float attractionRange = 0.8f;   // 아이템 자석 효과 범위 (플레이어 기준)
    float attractionSpeed = 2f;     // 아이템 끌려들어가는 속도
    LayerMask itemLayer;            // 아이템 레이어

    //선언과 동시에 초기화
    public int playerLv = 0;
    public int gatcha_weaponIndex = 0;
    public int gatcha_towerIndex = 0;
    public int gatcha_playerIndex = 0;
    public int maxExp = 1;
    public int curExp = 0;
    
    //하위 스크립트
    ItemCollector itemcollector;

    // 참조용 스트링 Arr
    string[] weaponName = new string[3] { "유튜브쟁이", "치지직갈걸", "숲에남을걸" };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");

        // 하위 스크립트 로드
        itemcollector = new ItemCollector();

        // 무기 프리펩 로드
        LV1WeaponPrefab = Resources.Load<GameObject>(weaponName[0]); // Instantiate로 생성한 프리팹을 참조
        LV2WeaponPrefab = Resources.Load<GameObject>(weaponName[1]); // Instantiate로 생성한 프리팹을 참조
        LV3WeaponPrefab = Resources.Load<GameObject>(weaponName[2]); // Instantiate로 생성한 프리팹을 참조

        // 1렙 무기 기본 제공
        weapon1 = new GameObject[maxWeaponCount];
        WeaponAdd();
    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Destroy(collision.gameObject);
            CheckLevelUp();
        }
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(input.x, input.y).normalized * moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
        { 
            //TODO : DIE
        }
    }

    void UpdateHealthBar()
    {
        //TODO
    }

    void CheckLevelUp()
    {
        //디버그 : 먹으면 무조건 레밸업
        //if (curExp < maxExp) return; 

        //Level up
        curExp = System.Math.Max(0, maxExp - curExp);
        //maxExp += 5; //본게임 때 주석 해제
        LevelUp();
    }

    //TODO : 업그레이드 관련 다 Helper쪽으로 빼기
    void WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for(int i = 0;i <maxWeaponCount;i++)
            {
                if (weapon1[i]==null)
                {
                    // 무기 프리팹을 플레이어의 자식으로 추가
                    GameObject weapon = Instantiate(LV1WeaponPrefab, transform.position, Quaternion.identity);
                    weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정

                    weapon1[i] = weapon;
                    WeaponSort();
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("무기 추가 실패");
        }
    }

    void WeaponUpgrade()
    {
        int[] gatchaArr = new int[maxWeaponCount] {0,1,2,3,4,5,6,7};
        Gatcha.Shuffle(gatchaArr);

        for (int i = 0; i < gatchaArr.Length;i++)
        {
            int index = gatchaArr[i];
            GameObject currentWeapon = weapon1[index];

            if (currentWeapon != null)
            {   
                if (currentWeapon.CompareTag("LV1Weapon"))
                {
                    ChangeOldToNewWeapon(currentWeapon, LV2WeaponPrefab, index);
                    return;
                }
                else if (currentWeapon.CompareTag("LV2Weapon"))
                {
                    ChangeOldToNewWeapon(currentWeapon, LV3WeaponPrefab, index);
                    return;
                }
                else if (currentWeapon.CompareTag("LV3Weapon")) print("LV3 weapon detected, so Cant Upgrade");
                else print("weapon level up err");
            }
        }
    }

    void ChangeOldToNewWeapon(GameObject oldWeapon, GameObject newWeaponPrefab, int index)
    {
        GameObject newWeapon = Instantiate(newWeaponPrefab, transform.position, Quaternion.identity);
        newWeapon.transform.parent = transform; // 현재 플레이어를 부모로 설정
        Destroy(oldWeapon);
        weapon1[index] = newWeapon;
        WeaponSort();
    }

    //TODO 조건문 자제 어떻게 호출형으로 못바꾸나?
    void LevelUp()
    {
        //디버그하려고 try 걸음
        try
        {
            int index = Gatcha.levelUpGatcha[playerLv++];

            if (index == 0)
            {
                int _weaponIndex = Gatcha.weaponGatcha[gatcha_weaponIndex++];

                if (_weaponIndex == 0)
                {
                    try
                    {
                        WeaponAdd();
                    }
                    catch (Exception ex)
                    {
                        print(ex.ToString());
                    }
                }
                else if (_weaponIndex == 1)
                {
                    try
                    {
                        WeaponUpgrade();
                    }
                    catch (Exception ex)
                    {
                        print(ex.ToString());
                    }
                }
                else if (_weaponIndex == 2)
                {
                    LevelUpHelper.WeaponAttackSpeedUp();
                }
                else if (_weaponIndex == 3)
                {
                    LevelUpHelper.WeaponRangedUp();
                }
                else if (_weaponIndex == 4)
                {
                    LevelUpHelper.WeaponAttackPowerUp();
                }
                else
                {
                    print("플에이어 무기 업그레이드 실패");
                }
            }
            else if (index == 1)
            {
                int _towerIndex = Gatcha.towerGatcha[gatcha_towerIndex++];
                LevelUpHelper.TowerUpgrade(_towerIndex);
            }
            else if (index == 2)
            {
                int _playerIndex = Gatcha.playerGatcha[gatcha_playerIndex++];
                LevelUpHelper.PlayerUpgrade(_playerIndex);
            }
            else
            {
                print("LEVELUP ERROR");
            }
        }
        catch (Exception ex)
        {
            print(ex.ToString());
        }
    }

    // 궤도 무기를 일정한 간격에서 공전하게함
    // TODO : FInd 등으로 int 매개변수 없앰 ==> WeaponSrt()로 호출
    void WeaponSort()
    {
        //TODO : 업그레이드 시 아직 업그레이드 된 오브젝트가 살아있어서 코루틴으로 빼야함
        //GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        //GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        //GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        //int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;
        //Debug.Log("Number of weapons: " + weaponCount);

        //try
        //{
        //    //weaponCount += 1; //weaponArr이 0부터이므로 보정
        //    int rad = 360 / weaponCount;

        //    for (int i = 0; i < weaponCount; i++)
        //    {
        //        BaseWeapon bweapon = weapon1[i].GetComponent<BaseWeapon>();
        //        bweapon.currentAngle = rad * i;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    print("sort err");
        //    print(ex.ToString());
        //}
    }

    // 자석효과 범위 디버그
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
