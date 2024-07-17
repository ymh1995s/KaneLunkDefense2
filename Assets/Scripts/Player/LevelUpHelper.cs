using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO �÷��̾ �ִ°� ����� ������
    }

    static public void WeaponAttackSpeedUp()
    {
        BaseWeapon.fireRateMmul *= 0.95f;
    }

    static public void WeaponRangedUp()
    {
        BaseWeapon.detectionRadiusMul += 0.1f;
    }

    //�̰͵� �б��� ���� �ȵǳ�
    static public void TowerUpgrade(int index)
    {
        if (index == 0)
        {
            TowerHPUp();
            Debug.Log("Ÿ�� ü�� ��(TODO)");
        }
        else if (index == 1)
        {
            TowerAttackSpeedUp();
            Debug.Log("Ÿ�� ���� ��(TODO)");
        }
        else if (index == 2)
        {
            TowerRangeUp();
            Debug.Log("Ÿ�� ������ ��(TODO)");
        }
        else if (index == 3)
        {
            //���ݷ� �ý����� ����
            Debug.Log("Ÿ�� ���ݷ� ��(TODO)");
        }
        else
        {
            Debug.Log("Ÿ�� ����� ERROR");
        }
    }

    static public void TowerHPUp()
    {
        BaseTower.maxHP += 2;
    }

    static public void TowerAttackSpeedUp()
    {
        BaseTower.fireRateMmul *= 0.9f;
    }

    static public void TowerRangeUp()
    {
        BaseTower.detectionRadius += 0.15f;
    }

    static public void PlayerUpgrade(int index)
    {
        if (index == 0)
        {
            PlayerHPUp();
            Debug.Log("�ÿ��̾� ü�¾�");
        }
        else if (index == 1)
        {
            PlayerSpeedUp();
            Debug.Log("�÷��̾� �̼Ӿ�");
        }
        else
        {
            Debug.Log("LEVELUP ERROR playerIndex");
        }
    }

    static public void PlayerHPUp()
    {
        BasePlayer.maxHP += 1;
    }

    static public void PlayerSpeedUp()
    {
        BasePlayer.moveSpeed += 0.1f;
    }
}
