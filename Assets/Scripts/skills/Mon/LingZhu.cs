using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 领主-战斗时提示友军护甲
/// </summary>
public class LingZhu : IMonSkill {

    int arm;
    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(1);
        this.arm = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnEnterBattle()
    {
        // 增加友军护甲
        Enermy curE = GetCurEnermy();
        List<Enermy> allies = curE.GetAlliesInBattle();
        for (int i = 0; i < allies.Count; i++)
        {
            Enermy temp = allies[i];
            Buff_LingZhu buff = temp.gameObject.AddComponent<Buff_LingZhu>();
            buff.Init(temp, arm);
            buff.StartEffect();
        }
    }
}
