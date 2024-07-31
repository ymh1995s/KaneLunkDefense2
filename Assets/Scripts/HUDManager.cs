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



    // 참조용 스트링 Arr
    protected string[] textNames = { "MENU/UI/StatGroup/LVText", "MENU/UI/StatGroup/PlayerSpec", "MENU/UI/StatGroup/WeaponSpec", "MENU/UI/StatGroup/TowerSpec" };


    void Start()
    {
        TextObjectSet();
    }

    void Update()
    {
        StatUpdate();
    }

    void TextObjectSet()
    {
        // 재귀적 찾기 유틸리티 함수
        lvText = FindChild<Text>(transform, textNames[0]);
        playerSpecText = FindChild<Text>(transform, textNames[1]);
        WeaponSpecText = FindChild<Text>(transform, textNames[2]);
        TowerSpecText = FindChild<Text>(transform, textNames[3]);
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

    //폐기 유예
    private void StatUpdate()
    {
        //lvText.text = $"LV {GameManager.Instance.player.playerLv} EXP {GameManager.Instance.player.curExp}/{GameManager.Instance.player.maxExp}";
        //playerSpecText.text = $"Player HP/MS : {BasePlayer.currentHP}/{BasePlayer.moveSpeed}";
        //lv1WeaponSpecText.text = $"LV1Weapon AP/AR/AS : {LV1Projectile.attackPower_ForUI}//{GameManager.Instance.player.maxExp}";
        //lv2WeaponSpecText.text = $"LV2Weapon AP/AR/AS : {LV2Projectile.attackPower_ForUI}//{GameManager.Instance.player.maxExp}";
        //lv3WeaponSpecText.text = $"LV3Weapon AP/AR/AS : {LV3Projectile.attackPower_ForUI}//{GameManager.Instance.player.maxExp}";
        //TowerSpecText.text = $"Tower HP/AP/AS/R : {GameManager.Instance.player.playerLv} EXP {GameManager.Instance.player.curExp}/{GameManager.Instance.player.maxExp}";
    }

    public void PlayerHUDUpdate(int lv, int curExp, int maxExp, int currentHP, float moveSpeed)
    {
        lvText.text = $"LV {lv} EXP {curExp}/{maxExp}";
        playerSpecText.text = $"P HP / Speed : {currentHP}/{moveSpeed}";
    }

    public void WeaponHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        WeaponSpecText.text = $"W AP/AR/AS : +{attackPower} / +{attackRange} / x{formattedAttackSpeed}";
    }

    public void TowerHUDUpdate(int attackPower, float attackRange, float attackSpeed)
    {
        // attackRange를 소수점 두 자리까지 포맷팅
        string formattedAttackSpeed = attackSpeed.ToString("F2");

        TowerSpecText.text = $"T! /AP/AR/AS : +{attackPower} / +{attackRange} / x{formattedAttackSpeed}";
    }
}