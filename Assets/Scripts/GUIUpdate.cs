using UnityEngine;
using UnityEngine.UI; // UI ������Ʈ�� ����ϱ� ���� �߰�
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

public class GUIUpdate : MonoBehaviour
{
    private Text lvText;

    void Start()
    {
        // ����� ã�� ��ƿ��Ƽ �Լ�
        lvText = FindChild<Text>(transform, "MENU/UI/LVText");
        if (lvText != null)
        {
            Debug.Log("LVText found: " + lvText.name);
        }
        else
        {
            Debug.LogError("LVText not found in Canvas/MENU/UI");
        }
    }

    void Update()
    {
        StatUpdate();
    }

    T FindChild<T>(Transform parent, string path) where T : Component
    {
        Transform child = parent.Find(path);
        if (child != null)
        {
            return child.GetComponent<T>();
        }
        return null;
    }

    private void StatUpdate()
    {
        if (lvText != null)
        {
            lvText.text = $"LV {GameManager.Instance.player.playerLv} EXP {GameManager.Instance.player.curExp}/{GameManager.Instance.player.maxExp}";
        }
        else
        {
            Debug.LogError("Target Text not found, cannot update text");
        }
    }
}
