using UnityEngine;
using System.Collections;

/// <summary>
/// 武器掌握
/// 被动提升攻击速度
/// </summary>
public class Wqzw : ISkill {
    public float iasAdd;

    public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(1));
        SetIasAddByLevel();
    }

    public void SetIasAddByLevel()
    {
        if (_Level > 0)
	    {
            iasAdd = GetBaseData().jdData[_Level - 1]["val"].AsFloat;
	    }
    }

    public override void StartEff()
    {
        GameManager.hero._IAS *= (1 + iasAdd);
    }

    public override void RemoveEff()
    {
        GameManager.hero._IAS /= (1 + iasAdd);
    }

    public override void OnLevelChange()
    {
        RemoveEff();
        SetIasAddByLevel();
        StartEff();
    }
}
