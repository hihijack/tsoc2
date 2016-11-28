using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 自爆，死亡时对所有人造成伤害
/// </summary>
public class ZiBao : IMonSkill {

    int dam;

    public override void Init(int level)
    {
        this.level = level;
        this.skillBD = GameDatas.GetMonSkillBD(2);
        this.dam = skillBD.jdData[level - 1]["val"].AsInt;
    }

    public override void OnDead()
    {
        List<IActor> acs = GameManager.gameView.GetAllInBattle();
        Enermy curE = GetCurEnermy();
        for (int i = 0; i < acs.Count; i++)
        {
            IActor temp = acs[i];
            if (temp._State != EActorState.Dead)
            {
                curE.DamageTarget(dam, temp, EDamageType.Fire);
            }
        }
    }
}
