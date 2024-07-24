using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour, IDamageable
{
    Rigidbody2D rigid;
    Vector2 input;

    //���� ����
    static public float moveSpeed = 5f;
    static public int maxHP = 10;
    static public int currentHP = 10;

    //���� ����
    public GameObject[] weapon1;
    public GameObject weaponPrefab; 
    const int maxWeaponCount = 8;
    GameObject LV1WeaponPrefab; 
    GameObject LV2WeaponPrefab; 
    GameObject LV3WeaponPrefab; 

    //������ �ڼ� ȿ��
    float attractionRange = 0.8f;   // ������ �ڼ� ȿ�� ���� (�÷��̾� ����)
    float attractionSpeed = 2f;     // ������ �������� �ӵ�
    LayerMask itemLayer;            // ������ ���̾�

    //����� ���ÿ� �ʱ�ȭ
    public int playerLv = 0;
    public int gatcha_weaponIndex = 0;
    public int gatcha_towerIndex = 0;
    public int gatcha_playerIndex = 0;
    public int maxExp = 1;
    public int curExp = 0;
    
    //���� ��ũ��Ʈ
    ItemCollector itemcollector;

    // ������ ��Ʈ�� Arr
    string[] weaponName = new string[3] { "��Ʃ������", "ġ��������", "����������" };

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");

        // ���� ��ũ��Ʈ �ε�
        itemcollector = new ItemCollector();

        // ���� ������ �ε�
        LV1WeaponPrefab = Resources.Load<GameObject>(weaponName[0]); // Instantiate�� ������ �������� ����
        LV2WeaponPrefab = Resources.Load<GameObject>(weaponName[1]); // Instantiate�� ������ �������� ����
        LV3WeaponPrefab = Resources.Load<GameObject>(weaponName[2]); // Instantiate�� ������ �������� ����

        // 1�� ���� �⺻ ����
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
        //����� : ������ ������ �����
        //if (curExp < maxExp) return; 

        //Level up
        curExp = System.Math.Max(0, maxExp - curExp);
        //maxExp += 5; //������ �� �ּ� ����
        LevelUp();
    }

    //TODO : ���׷��̵� ���� �� Helper������ ����
    void WeaponAdd()
    {
        if (LV1WeaponPrefab != null)
        {
            for(int i = 0;i <maxWeaponCount;i++)
            {
                if (weapon1[i]==null)
                {
                    // ���� �������� �÷��̾��� �ڽ����� �߰�
                    GameObject weapon = Instantiate(LV1WeaponPrefab, transform.position, Quaternion.identity);
                    weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����

                    weapon1[i] = weapon;
                    WeaponSort();
                    return;
                }
            }
        }
        else
        {
            Debug.LogWarning("���� �߰� ����");
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
        newWeapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����
        Destroy(oldWeapon);
        weapon1[index] = newWeapon;
        WeaponSort();
    }

    //TODO ���ǹ� ���� ��� ȣ�������� ���ٲٳ�?
    void LevelUp()
    {
        //������Ϸ��� try ����
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
                    print("�ÿ��̾� ���� ���׷��̵� ����");
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

    // �˵� ���⸦ ������ ���ݿ��� �����ϰ���
    // TODO : FInd ������ int �Ű����� ���� ==> WeaponSrt()�� ȣ��
    void WeaponSort()
    {
        //TODO : ���׷��̵� �� ���� ���׷��̵� �� ������Ʈ�� ����־ �ڷ�ƾ���� ������
        //GameObject[] weapons1 = GameObject.FindGameObjectsWithTag("LV1Weapon");
        //GameObject[] weapons2 = GameObject.FindGameObjectsWithTag("LV2Weapon");
        //GameObject[] weapons3 = GameObject.FindGameObjectsWithTag("LV3Weapon");

        //int weaponCount = weapons1.Length + weapons2.Length + weapons3.Length;
        //Debug.Log("Number of weapons: " + weaponCount);

        //try
        //{
        //    //weaponCount += 1; //weaponArr�� 0�����̹Ƿ� ����
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

    // �ڼ�ȿ�� ���� �����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
