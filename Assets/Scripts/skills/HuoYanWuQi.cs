using UnityEngine;
using System.Collections;

public class HuoYanWuQi : ISkill {

    public int fireDamage;

    public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(5));
        SetDamage();
    }

    void SetDamage()
    {
        if (_Level > 0)
        {
            fireDamage = GetBaseData().jdData[_Level - 1]["val"].AsInt;
        }
    }

    public override void OnLevelChange()
    {
        RemoveEff();
        SetDamage();
        StartEff();
    }

    public override void StartEff()
    {
        GameManager.hero.Prop.atkFireParamAdd += fireDamage;
    }

    public override void RemoveEff()
    {
        GameManager.hero.Prop.atkFireParamAdd -= fireDamage;
    }
}
