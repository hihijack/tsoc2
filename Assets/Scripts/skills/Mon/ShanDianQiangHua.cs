﻿using UnityEngine;
using System.Collections;

/// <summary>
/// 闪电强化，攻击附加闪电伤害
/// </summary>
public class ShanDianQiangHua : IMonSkill {

    int dam;
    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(5);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnAttackHit(IActor target, int attack)
    {
        Enermy curE = GetCurEnermy();
        curE.DamageTarget(dam, target, EDamageType.Lighting);
    }
}