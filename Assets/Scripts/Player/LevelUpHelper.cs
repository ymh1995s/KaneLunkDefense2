using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO �÷��̾ �ִ°� ����� ������
    }

    static public void WeaponAttackPowerUp()
    {
        //BaseProjectile.attackPower += 2;
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
        if (index == 0)  TowerHPUp();
        else if (index == 1) TowerAttackSpeedUp();
        else if (index == 2) TowerRangeUp(); 
        else if (index == 3) TowerAttackPowerUp();
        else Debug.Log("Ÿ�� ����� ERROR");
    }

    static public void TowerHPUp()
    {
        BaseTower.maxHP += 20;
    }

    static public void TowerAttackSpeedUp()
    {
        BaseTower.fireRateMmul *= 0.9f;
    }

    static public void TowerAttackPowerUp()
    {
        BaseTower.attackPower += 2;
    }

    static public void TowerRangeUp()
    {
        BaseTower.detectionRadius += 0.15f;
    }

    static public void PlayerUpgrade(int index)
    {
        if (index == 0)  PlayerHPUp(); 
        else if (index == 1)  PlayerSpeedUp();
        else Debug.Log("LEVELUP ERROR playerIndex");
    }

    static public void PlayerHPUp()
    {
        BasePlayer.maxHP += 1;
        BasePlayer.currentHP += BasePlayer.maxHP;
    }

    static public void PlayerSpeedUp()
    {
        //BasePlayer.moveSpeed += 0.1f;
        BasePlayer.moveSpeed += 1f;
    }
}
