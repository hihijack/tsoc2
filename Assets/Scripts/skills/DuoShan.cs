﻿using UnityEngine;
using System.Collections;

public class DuoShan : ISkill {

    int val;

    public override void Init()
    {
        SetBaseData(GameDatas.GetSkillBD(6));
        SetVal();
    }

    void SetVal()
    {
        if (_Level > 0)
	    {
            this.val = GetBaseData().jdData[_Level - 1]["val"].AsInt;
	    }
    }

    public override void StartEff()
    {
        GameManager.hero.dodge += val;
    }

    public override void RemoveEff()
    {
        GameManager.hero.dodge -= val;
    }

    public override void OnLevelChange()
    {
        RemoveEff();
        SetVal();
        StartEff();
    }
}