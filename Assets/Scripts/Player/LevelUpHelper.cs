using UnityEngine;

public static class LevelUpHelper
{
    static void WeaponUpgrade(int index)
    {
        //TODO 플레이어에 있는거 여기로 빼오기
    }

    static public void WeaponAttackSpeedUp()
    {
        BaseWeapon.fireRateMmul *= 0.95f;
    }

    static public void WeaponRangedUp()
    {
        BaseWeapon.detectionRadiusMul += 0.1f;
    }

    //이것도 분기좀 빼면 안되나
    static public void TowerUpgrade(int index)
    {
        if (index == 0)
        {
            TowerHPUp();
            Debug.Log("타워 체력 업(TODO)");
        }
        else if (index == 1)
        {
            TowerAttackSpeedUp();
            Debug.Log("타워 공속 업(TODO)");
        }
        else if (index == 2)
        {
            TowerRangeUp();
            Debug.Log("타워 레인지 업(TODO)");
        }
        else if (index == 3)
        {
            //공격력 시스템은 보류
            Debug.Log("타워 공격력 업(TODO)");
        }
        else
        {
            Debug.Log("타워 레밸업 ERROR");
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
            Debug.Log("플에이어 체력업");
        }
        else if (index == 1)
        {
            PlayerSpeedUp();
            Debug.Log("플레이어 이속업");
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
