using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // ���� ���� ������ ��ȯ�ϴ� �޼���
    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // �ص� ������ ��ȯ�ϴ� �޼���
    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    // ���� ������ ��ȯ�ϴ� �޼��� (�ʿ��� ���)
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}