using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 input;
    //TODO : 플레이어 이속 대폭 낮춰야함
    static public float moveSpeed = 5f;
    static public int maxHP = 5;
    static public int currentHP = 5;

    //무기 관리
    GameObject[] weapon1;
    GameObject weaponPrefab; // Inspector에서 할당할 무기 프리팹
    const int maxWeaponCount = 8;
     GameObject LV1WeaponPrefab; // Instantiate로 생성한 프리팹을 참조
     GameObject LV2WeaponPrefab; // Instantiate로 생성한 프리팹을 참조
     GameObject LV3WeaponPrefab; // Instantiate로 생성한 프리팹을 참조

    //아이템 자석 효과
    float attractionRange = 0.8f;   // 아이템 자석 효과 범위 (플레이어 기준)
    float attractionSpeed = 2f;     // 아이템 끌려들어가는 속도
    LayerMask itemLayer;            // 아이템 레이어

    //선언과 동시에 초기화
    int playerLv = 0;
    int weaponIndex = 0;
    int towerIndex = 0;
    int playerIndex = 0;
    int w1_level = 0;
    int maxExp = 1;
    int curExp = 0;
    
    //하위 스크립트
    ItemCollector itemcollector;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");

        // 하위 스크립트 로드
        itemcollector = new ItemCollector();

        // 1렙 무기 기본 제공
        weapon1 = new GameObject[maxWeaponCount];
        weaponPrefab = Resources.Load<GameObject>("유튜브쟁이");
        WeaponAdd();

        //TODO : weaponPrefab 변수랑 아래 변수랑 통일
        LV1WeaponPrefab = Resources.Load<GameObject>("유튜브쟁이"); ; // Instantiate로 생성한 프리팹을 참조
        LV2WeaponPrefab = Resources.Load<GameObject>("치지직갈걸"); ; // Instantiate로 생성한 프리팹을 참조
        LV3WeaponPrefab = Resources.Load<GameObject>("숲에남을걸"); ; // Instantiate로 생성한 프리팹을 참조
    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);
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
        rigid.velocity = new Vector2(input.x, input.y).normalized * moveSpeed;
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
        LevelUp();
    }

    //TODO : 업그레이드 관련 다 Helper쪽으로 빼기
    void WeaponAdd()
    {
        if (weaponPrefab != null && w1_level <= 7)
        {
            // 무기 프리팹을 플레이어의 자식으로 추가
            // TODO : 가독이 안좋아서 리팩토링 해야됨
            GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정
            weapon1[w1_level] = weapon;
            WeaponSort(w1_level);
            w1_level++;
        }
        else
        {
            Debug.LogWarning("무기 추가 실패");
            //LevelUp();
        }
    }

    void WeaponUpgrade()
    {
        GameObject[] tempWeapon1arr = new GameObject[maxWeaponCount];
        Array.Copy(weapon1, tempWeapon1arr, weapon1.Length);
        Gatcha.Shuffle(tempWeapon1arr);

        //TODO 반복문 리팩토링
        for (int i = 0; i < tempWeapon1arr.Length;i++)
        {
            GameObject newWeapon = null;
            if (tempWeapon1arr[i] != null)
            {
                if (tempWeapon1arr[i].CompareTag("LV1Weapon")) newWeapon = Instantiate(LV2WeaponPrefab, transform.position, Quaternion.identity);
                else if (tempWeapon1arr[i].CompareTag("LV2Weapon")) newWeapon = Instantiate(LV3WeaponPrefab, transform.position, Quaternion.identity);
                else if (tempWeapon1arr[i].CompareTag("LV3Weapon")) print("LV3 weapon detected, so Cant Upgrade");
                else print("weapon level up err");
            }

            if(newWeapon != null)
            {
                newWeapon.transform.parent = transform; // 현재 플레이어를 부모로 설정
                //Destroy(weapon1[i]);
                for (int j = 0;j < tempWeapon1arr.Length;j++)
                {
                    //원본 무기 배열을 모두 날리고
                    Destroy(weapon1[j]);
                    weapon1[j] = null;
                }

                int arrindex = 0;
                for (int j = 0; j < tempWeapon1arr.Length; j++)
                {
                    if (tempWeapon1arr[j]!=null)
                    {
                        weapon1[arrindex++] = tempWeapon1arr[i];
                    }
                }

                //weapon1[i] = newWeapon;
                WeaponSort(w1_level-1); //저쪽 로직에서 ++해놔서 여기서 -1 하긴 했는데 이거 좀더 좋은 방법을 찾아보자
                return;
            }
        }
    }
    
    //버그 : WeaponUpgrade 시 기존 무기 사라짐
    //TODO 조건문 자제
    void LevelUp()
    {
        //디버그하려고 try 걸음
        try
        {
            int index = Gatcha.levelUpGatcha[playerLv++];
            if (index == 0)
            {
                int _weaponIndex = Gatcha.weaponGatcha[weaponIndex++];

                if (_weaponIndex == 0)
                {
                    try
                    {
                        WeaponAdd();
                        // print("플에이어 무기 추가(완료)");
                    }
                    catch (Exception ex)
                    {
                        print("EXCEPTION");
                        print(ex.ToString());
                    }
                }
                else if (_weaponIndex == 1)
                {
                    try
                    {
                        // 무기 업그레이드 : TODO
                        WeaponUpgrade();
                        // print("플에이어 무기 등급 업(TODO)");
                        LevelUp();
                    }
                    catch (Exception ex)
                    {
                        print("EXCEPTION");
                        print(ex.ToString());
                    }
                }


                else if (_weaponIndex == 2)
                {
                    LevelUpHelper.WeaponAttackSpeedUp();
                    // print("플에이어 무기 공속 업(완료)");
                }
                else if (_weaponIndex == 3)
                {
                    LevelUpHelper.WeaponRangedUp();
                    // print("플에이어 무기 레인지 업(완료)");
                }
                else if (_weaponIndex == 4)
                {
                    // 공격력 시스템은 보류
                    print("플에이어 무기 공격력 업(TODO)");
                }
                else
                {
                    print("플에이어 무기 업그레이드 실패");
                }
            }
            else if (index == 1)
            {
                int _towerIndex = Gatcha.towerGatcha[towerIndex++];
                LevelUpHelper.TowerUpgrade(_towerIndex);
            }
            else if (index == 2)
            {
                int _playerIndex = Gatcha.playerGatcha[playerIndex++];
                LevelUpHelper.PlayerUpgrade(_playerIndex);
            }
            else
            {
                print("LEVELUP ERROR");
            }
        }
        catch (Exception ex)
        {
            print("EXCEPTION");
            print(ex.ToString());
        }
    }

    // 궤도 무기를 일정한 간격에서 공전하게함
    void WeaponSort(int weaponCount)
    {
        try
        {

            weaponCount += 1; //weaponArr이 0부터이므로 보정
            int rad = 360 / weaponCount;

            for (int i = 0; i < weaponCount; i++)
            {
                BaseWeapon bweapon = weapon1[i].GetComponent<BaseWeapon>();
                bweapon.currentAngle = rad * i;
            }
        }
        catch (Exception ex)
        {
            print("sort err");
            print(ex.ToString());
        }
    }

    void Hit()
    {
        Death();
    }

    void Death()
    {

    }

    // 자석효과 범위 디버그
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
