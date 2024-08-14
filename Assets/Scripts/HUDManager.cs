using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트를 사용하기 위해 추가
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

public class HUDManager : MonoBehaviour
{
    private Text lvText;
    private Text playerSpecText;
    private Text WeaponSpecText;
    private Text TowerSpecText;
    private Text levelupHintText;



    // 참조용 스트링 Arr
    protected string[] specTextDir = { "EXP/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };
    protected string levelUpHintDir = "LevelUPText";

    private void Awake()
    {

        TextObjectSet();
    }


    void Start()
    {
    }

    void TextObjectSet()
    {
        // 재귀적 찾기 유틸리티 함수
        lvText = FindChild<Text>(transform, specTextDir[0]);
        playerSpecText = FindChild<Text>(transform, specTextDir[1]);
        WeaponSpecText = FindChild<Text>(transform, specTextDir[2]);
        TowerSpecText = FindChild<Text>(transform, specTextDir[3]);
        levelupHintText = FindChild<Text>(transform, levelUpHintDir);
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

    public RectTransform experienceBar; // 게이지바에 해당하는 RectTransform
    public float maxWidth = 200f; // 게이지바의 최대 너비

    public void UpdateExperienceBar(float currentExperience, float maxExperience)
    {
        float width = (currentExperience / maxExperience) * maxWidth;
        experienceBar.sizeDelta = new Vector2(width, experienceBar.sizeDelta.y);
    }


    public void PlayerHUDUpdate(int lv, int curExp, int maxExp, int currentHP, float moveSpeed)
    { 
        lv+=1; //배열 관리상 +1
        lvText.text = $"LV {lv} EXP {curExp}/{maxExp}";
        playerSpecText.text = $"플레이어 체력 / 이동속도 : {currentHP}/{moveSpeed}";

    }

    public void WeaponHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        WeaponSpecText.text = $"무기 AP/AR/AS : +{attackPower} / +{attackRange} / x{formattedAttackSpeed}";
    }

    public void TowerHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        TowerSpecText.text = $"타워 /AP/AR/AS : +{attackPower} / +{attackRange} / x{formattedAttackSpeed}";
    }

    public float fadeDuration = 2f; // 텍스트가 서서히 사라지는 시간
    public float displayDuration = 2f; // 텍스트가 표시되는 시간

    public void LevelUpHintUpdate(string msg)
    {
        // 텍스트를 빨간색과 볼드체로 설정
        levelupHintText.text = $"{msg}";
        // 초기 색상을 완전히 불투명하게 설정
        levelupHintText.color = new Color(1f, 0f, 0f, 1f);

        // 페이드 아웃 코루틴 시작
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        // 텍스트 표시 후 대기
        yield return new WaitForSeconds(displayDuration);

        // 초기 색상을 가져온다
        Color originalColor = levelupHintText.color;
        float elapsedTime = 0f;

        // 페이드 아웃 진행
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종적으로 완전히 투명하게 설정
        levelupHintText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}