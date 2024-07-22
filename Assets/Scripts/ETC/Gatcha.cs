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
        // �˵� ���� ����
        // �˵� ���� ��� ���׷��̵� 
        // �˵� ���� ���� ����
        // �˵� ���� ������ ����
        // �˵� ���� ���ݷ� ����
        //40%
        // Ÿ�� ü��
        // Ÿ�� ����
        // Ÿ�� ������ 
        // Ÿ�� ���ݷ�
        //10%
        // �÷��̾� ü��
        // �÷��̾� �̵��ӵ�

        //TODO :�ƴ� �۷ι� ����, ����ü ��

        // �迭�� 0�� 50��, 1��,40��, 2�� 10 �� �߰�
        AddNumbersToArray(0, 50, levelUpGatcha); //���� ����
        AddNumbersToArray(1, 40, levelUpGatcha); //Ÿ�� ����
        AddNumbersToArray(2, 10, levelUpGatcha); //�÷��̾� ����

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 170, weaponGatcha); // ���� ���� MAX7 (�⺻ 1���̹Ƿ�)
        AddNumbersToArray(1, 140, weaponGatcha); // ���� ��� 3*8 = 24
        AddNumbersToArray(2, 290, weaponGatcha); // ���� ���� 
        AddNumbersToArray(3, 100, weaponGatcha); // ���� ������
        AddNumbersToArray(4, 300, weaponGatcha); // ���� ���ݷ�

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 300, towerGatcha); // Ÿ�� ü��
        AddNumbersToArray(1, 300, towerGatcha); // Ÿ�� ����
        AddNumbersToArray(2, 100, towerGatcha); // Ÿ�� ������
        AddNumbersToArray(3, 300, towerGatcha); // Ÿ�� ���ݷ�

        //���� 1000�� �ǵ���
        AddNumbersToArray(0, 500, playerGatcha); // �÷��̾� ü��
        AddNumbersToArray(1, 500, playerGatcha); // �÷��̾� �̵��ӵ�

        ShuffleAll(); //�迭�� ���� �� ������ ����
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
        // �迭�� �� ������ ���ڸ� �߰�
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
