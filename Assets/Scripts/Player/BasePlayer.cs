using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class BasePlayer : MonoBehaviour
{
    Rigidbody2D rigid;
    Vector2 input;
    //TODO : �÷��̾� �̼� ���� �������
    static public float moveSpeed = 5f;
    static public int maxHP = 5;
    static public int currentHP = 5;

    //���� ����
    GameObject[] weapon1;
    GameObject weaponPrefab; // Inspector���� �Ҵ��� ���� ������
    const int maxWeaponCount = 8;
     GameObject LV1WeaponPrefab; // Instantiate�� ������ �������� ����
     GameObject LV2WeaponPrefab; // Instantiate�� ������ �������� ����
     GameObject LV3WeaponPrefab; // Instantiate�� ������ �������� ����

    //������ �ڼ� ȿ��
    float attractionRange = 0.8f;   // ������ �ڼ� ȿ�� ���� (�÷��̾� ����)
    float attractionSpeed = 2f;     // ������ �������� �ӵ�
    LayerMask itemLayer;            // ������ ���̾�

    //����� ���ÿ� �ʱ�ȭ
    int playerLv = 0;
    int weaponIndex = 0;
    int towerIndex = 0;
    int playerIndex = 0;
    int w1_level = 0;
    int maxExp = 1;
    int curExp = 0;
    
    //���� ��ũ��Ʈ
    ItemCollector itemcollector;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        itemLayer = LayerMask.GetMask("Item");

        // ���� ��ũ��Ʈ �ε�
        itemcollector = new ItemCollector();

        // 1�� ���� �⺻ ����
        weapon1 = new GameObject[maxWeaponCount];
        weaponPrefab = Resources.Load<GameObject>("��Ʃ������");
        WeaponAdd();

        //TODO : weaponPrefab ������ �Ʒ� ������ ����
        LV1WeaponPrefab = Resources.Load<GameObject>("��Ʃ������"); ; // Instantiate�� ������ �������� ����
        LV2WeaponPrefab = Resources.Load<GameObject>("ġ��������"); ; // Instantiate�� ������ �������� ����
        LV3WeaponPrefab = Resources.Load<GameObject>("����������"); ; // Instantiate�� ������ �������� ����
    }

    private void Update()
    {
        itemcollector.CollectItem(transform.position, attractionRange, attractionSpeed, itemLayer);
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
        rigid.velocity = new Vector2(input.x, input.y).normalized * moveSpeed;
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
        LevelUp();
    }

    //TODO : ���׷��̵� ���� �� Helper������ ����
    void WeaponAdd()
    {
        if (weaponPrefab != null && w1_level <= 7)
        {
            // ���� �������� �÷��̾��� �ڽ����� �߰�
            // TODO : ������ �����Ƽ� �����丵 �ؾߵ�
            GameObject weapon = Instantiate(weaponPrefab, transform.position, Quaternion.identity);
            weapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����
            weapon1[w1_level] = weapon;
            WeaponSort(w1_level);
            w1_level++;
        }
        else
        {
            Debug.LogWarning("���� �߰� ����");
            //LevelUp();
        }
    }

    void WeaponUpgrade()
    {
        GameObject[] tempWeapon1arr = new GameObject[maxWeaponCount];
        Array.Copy(weapon1, tempWeapon1arr, weapon1.Length);
        Gatcha.Shuffle(tempWeapon1arr);

        //TODO �ݺ��� �����丵
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
                newWeapon.transform.parent = transform; // ���� �÷��̾ �θ�� ����
                //Destroy(weapon1[i]);
                for (int j = 0;j < tempWeapon1arr.Length;j++)
                {
                    //���� ���� �迭�� ��� ������
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
                WeaponSort(w1_level-1); //���� �������� ++�س��� ���⼭ -1 �ϱ� �ߴµ� �̰� ���� ���� ����� ã�ƺ���
                return;
            }
        }
    }
    
    //���� : WeaponUpgrade �� ���� ���� �����
    //TODO ���ǹ� ����
    void LevelUp()
    {
        //������Ϸ��� try ����
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
                        // print("�ÿ��̾� ���� �߰�(�Ϸ�)");
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
                        // ���� ���׷��̵� : TODO
                        WeaponUpgrade();
                        // print("�ÿ��̾� ���� ��� ��(TODO)");
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
                    // print("�ÿ��̾� ���� ���� ��(�Ϸ�)");
                }
                else if (_weaponIndex == 3)
                {
                    LevelUpHelper.WeaponRangedUp();
                    // print("�ÿ��̾� ���� ������ ��(�Ϸ�)");
                }
                else if (_weaponIndex == 4)
                {
                    // ���ݷ� �ý����� ����
                    print("�ÿ��̾� ���� ���ݷ� ��(TODO)");
                }
                else
                {
                    print("�ÿ��̾� ���� ���׷��̵� ����");
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

    // �˵� ���⸦ ������ ���ݿ��� �����ϰ���
    void WeaponSort(int weaponCount)
    {
        try
        {

            weaponCount += 1; //weaponArr�� 0�����̹Ƿ� ����
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

    // �ڼ�ȿ�� ���� �����
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attractionRange);
    }
}
