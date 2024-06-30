using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    // 메인 게임 씬으로 전환하는 메서드
    public void LoadMainGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    // 앤딩 씬으로 전환하는 메서드
    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    // 시작 씬으로 전환하는 메서드 (필요한 경우)
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
}