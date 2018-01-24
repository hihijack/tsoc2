using System.Collections.Generic;

/// <summary>
/// 进入战斗时 - 增加友军火焰抗性
/// </summary>
public class MoFaHuZhao : IMonSkill
{
    int val;
    public override void Init(int level)
    {
        this.level = level;
        skillBD = GameDatas.GetMonSkillBD(30);
        val = skillBD.GetIntVal(level, "val");
    }

    public override void OnEnterBattle()
    {
        // 增加友军火焰抗性
        Enermy curE = GetCurEnermy();
        List<Enermy> allies = curE.GetAlliesInBattle();
        for (int i = 0; i < allies.Count; i++)
        {
            Enermy temp = allies[i];
            Buff_MoFaHuZhao buff = temp.gameObject.AddComponent<Buff_MoFaHuZhao>();
            buff.Init(temp, val);
            buff.StartEffect();
        }
    }
}