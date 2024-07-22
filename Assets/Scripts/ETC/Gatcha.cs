using UnityEngine;

public static class Gatcha
{
    static public int[] levelUpGatcha = new int[100]; //MAX LEVEL 100
    static public int[] weaponGatcha = new int[1000];
    static public int[] towerGatcha = new int[1000];
    static public int[] playerGatcha = new int[1000];

    static public void GameStart()
    {
        //50%
        // 궤도 무기 증가
        // 궤도 무기 등급 업그레이드 
        // 궤도 무기 공속 증가
        // 궤도 무기 레인지 증가
        // 궤도 무기 공격력 증가
        //40%
        // 타워 체력
        // 타워 공속
        // 타워 레인지 
        // 타워 공격력
        //10%
        // 플레이어 체력
        // 플레이어 이동속도

        //TODO :아님 글로벌 상점, 투사체 등

        // 배열에 0을 50개, 1을,40개, 2를 10 개 추가
        AddNumbersToArray(0, 50, levelUpGatcha); //무기 관련
        AddNumbersToArray(1, 40, levelUpGatcha); //타워 관련
        AddNumbersToArray(2, 10, levelUpGatcha); //플레이어 관련

        //합이 1000이 되도록
        AddNumbersToArray(0, 170, weaponGatcha); // 무기 개수 MAX7 (기본 1개이므로)
        AddNumbersToArray(1, 140, weaponGatcha); // 무기 등급 3*8 = 24
        AddNumbersToArray(2, 290, weaponGatcha); // 무기 공속 
        AddNumbersToArray(3, 100, weaponGatcha); // 무기 레인지
        AddNumbersToArray(4, 300, weaponGatcha); // 무기 공격력

        //합이 1000이 되도록
        AddNumbersToArray(0, 300, towerGatcha); // 타워 체력
        AddNumbersToArray(1, 300, towerGatcha); // 타워 공속
        AddNumbersToArray(2, 100, towerGatcha); // 타워 레인지
        AddNumbersToArray(3, 300, towerGatcha); // 타워 공격력

        //합이 1000이 되도록
        AddNumbersToArray(0, 500, playerGatcha); // 플레이어 체력
        AddNumbersToArray(1, 500, playerGatcha); // 플레이어 이동속도

        ShuffleAll(); //배열에 랜덤 요쇼가 오도록 섞음
    }

    static public void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    static public void Shuffle(GameObject[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    static void ShuffleAll()
    {        
        Shuffle(levelUpGatcha);
        Shuffle(weaponGatcha);
        Shuffle(towerGatcha);
        Shuffle(playerGatcha);
    }

    static void AddNumbersToArray(int number, int count, int[] TargetArr)
    {
        int currentIndex = 0;
        // 배열의 빈 공간에 숫자를 추가
        for (int i = 0; i < count; i++)
        {
            while (TargetArr[currentIndex] != 0)
            {
                currentIndex++;
            }
            TargetArr[currentIndex] = number;
        }
    }
}
