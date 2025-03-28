using NUnit.Framework.Constraints;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour, IDamageable
{
    Rigidbody2D rigid;
    Vector2 input;
    private SpriteRenderer spriteRenderer;
    public Vector2 minBounds; // 최소 경계
    public Vector2 maxBounds; // 최대 경계

    //스텟 영역
    static public float moveSpeed = 2.0f;
    static public int maxHP = 20;
    static public int currentHP;

    //무기 관리
    const int maxWeaponCount = 100;
    public GameObject[] weapon1;
    public GameObject weaponPrefab; 
    GameObject LV1WeaponPrefab; 
    GameObject LV2WeaponPrefab; 
    GameObject LV3WeaponPrefab; 

    //아이템 자석 효과
    float attractionRange = 0.8f;   // 아이템 자석 효과 범위 (플레이어 기준)
    float attractionSpeed = 2f;     // 아이템 끌려들어가는 속도
    LayerMask itemLayer;            // 아이템 레이어

    //체력바 영역
    private RectTransform healthBarForeground;
    private Vector3 originalScale;

    //선언과 동시에 초기화
    private int playerLv = 0;
    private int gatcha_weaponIndex = 0;
    private int gatcha_towerIndex = 0;
    private int gatcha_playerIndex = 0;
    private int maxExp = 10;
    private int curExp = 0;
    
    //하위 스크립트
    ItemCollector itemcollector;

    // 참조용 스트링 Arr
    string[] weaponName = new string[3] { "Weapon/유튜브쟁이", "Weapon/치지직갈걸", "Weapon/숲에남을걸" };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");
        spriteRenderer = GetComponent<SpriteRenderer>();

        currentHP = maxHP;

        // 하위 스크립트 로드
        itemcollector = new ItemCollector();

        // 무기 프리펩 로드
        LV1WeaponPrefab = Resources.Load<GameObject>(weaponName[0]); // Instantiate로 생성한 프리팹을 참조
        LV2WeaponPrefab = Resources.Load<GameObject>(weaponName[1]); // Instantiate로 생성한 프리팹을 참조
        LV3WeaponPrefab = Resources.Load<GameObject>(weaponName[2]); // Instantiate로 생성한 프리팹을 참조

        // 무기 1개 기본 제공
        weapon1 = new GameObject[maxWeaponCount];
        WeaponAdd();

        //체력바
        healthBarForeground = transform.Find("HPBar/RED").GetComponent<RectTransform>();
        originalScale = healthBarForeground.localScale;
        UpdateHealthBar();

    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);

        // HUD 업데이트
        GameManager.Instance.hudManager.PlayerHUDUpdate(playerLv, curExp, maxExp, currentHP, moveSpeed);
        GameManager.Instance.hudManager.WeaponHUDUpdate(BaseProjectile.attackPowerUp, BaseWeapon.detectionRadiusMul, BaseWeapon.fireRateMmul);
        GameManager.Instance.hudManager.TowerHUDUpdate(BaseProjectile.attackPowerUp, BaseTower.detectionRadius, BaseTower.fireRateMmul);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            if (collision.gameObject.name == "Coin3(Clone)")
                curExp += 1;
            else if (collision.gameObject.name == "Coin2(Clone)")
                curExp += 3;
            else if (collision.gameObject.name == "Coin(Clone)")
                curExp += 10;
            Destroy(collision.gameObject);
            CheckLevelUp();
        }

        if (collision.gameObject.CompareTag("Boundary"))
        {
            rigid.velocity = Vector2.zero;
        }
    }

    void FixedUpdate()
    {
        rigid.velocity = new Vector2(input.x, input.y).normalized * moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        input = value.Get<Vector2>();

        if (input.x < 0)
        {
            // 왼쪽으로 이동할 때 좌우 반전
            spriteRenderer.flipX = false;
        }
        else if (input.x > 0)
        {
            // 오른쪽으로 이동할 때 좌우 반전 해제
            spriteRenderer.flipX = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        UpdateHealthBar();

        if (currentHP <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        SceneLoader.SceneLoad_OverScene();
    }

    void UpdateHealthBar()
    {
        // 체력 비율 계산
        float healthPercent = (float)currentHP / maxHP;

        // 체력바의 스케일 조정
        healthBarForeground.localScale = new Vector3(originalScale.x * healthPercent, originalScale.y, originalScale.z);
    }

    void CheckLevelUp()
    {
        if (curExp < maxExp) return;

        //Level up
        curExp = System.Math.Max(0, maxExp - curExp);
        maxExp += 1; //본게임 때 주석 해제
        LevelUp();
    }

    public void Debug_WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for (int i = 0; i < maxWeaponCount; i++)
            {
                if (weapon1[i] == null)
                {
                    GameObject weapon;

                    weapon = Instantiate(LV3WeaponPrefab, transform.position, Quaternion.identity);

                    weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정

                    weapon1[i] = weapon;
                    WeaponSort();
                    GameManager.Instance.hudManager.LevelUpHintUpdate("무기 추가!");
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("무기 추가 실패");
        }
    }

    //TODO : 업그레이드 관련 다 Helper쪽으로 빼기
    void WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for (int i = 0; i < maxWeaponCount; i++)
            {
                if (weapon1[i] == null)
                {
                    GameObject weapon;

                    int index = UnityEngine.Random.Range(0, 100);

                    //TODO 하드코딩 제거
                    if (index <70) weapon = Instantiate(LV1WeaponPrefab, transform.position, Quaternion.identity);
                    else if (index < 95) weapon = Instantiate(LV2WeaponPrefab, transform.position, Quaternion.identity);
                    else weapon = Instantiate(LV3WeaponPrefab, transform.position, Quaternion.identity);

                    weapon.transform.parent = transform; // 현재 플레이어를 부모로 설정

                    weapon1[i] = weapon;
                    WeaponSort();
                    GameManager.Instance.hudManager.LevelUpHintUpdate("무기 추가!");
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("무기 추가 실패");
        }
    }

    void LevelUp()
    {
        //디버그하려고 try 걸음
        try
        {
            int index = Gatcha.levelUpGatcha[playerLv++];

            if (index == 0)
            {
                int _weaponIndex = Gatcha.weaponGatcha[gatcha_weaponIndex++];

                if (_weaponIndex == 0) WeaponAdd();
                else if (_weaponIndex == 1) LevelUpHelper.WeaponAttackSpeedUp();
                else if (_weaponIndex == 2) LevelUpHelper.WeaponRangedUp();
                else if (_weaponIndex == 3) LevelUpHelper.WeaponAttackPowerUp(); 
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
                UpdateHealthBar(); //체력 업그레이드 시 업그레이드 된 체력 반영
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
    void WeaponSort()
    {
        GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;

        try
        {
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

    // 자석효과 범위 디버그
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
