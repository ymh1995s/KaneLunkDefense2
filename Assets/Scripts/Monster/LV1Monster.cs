using UnityEngine;

public class LV1Monster : BaseMonster
{
    protected override void Start()
    {
        base.Start();
        //TODO : 체력 값은 Base에서 관리해서 하드코딩 제거
        hp = 1;
    }
}
