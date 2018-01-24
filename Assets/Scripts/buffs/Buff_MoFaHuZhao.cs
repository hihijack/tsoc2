using UnityEngine;
using System.Collections;

/// <summary>
/// 魔法护罩
/// </summary>
public class Buff_MoFaHuZhao : IBaseBuff
{
    int val;

    public void Init(IActor target, int val)
    {
        this.target = target;
        this.val = val;
        baseData = GameDatas.GetBuff(8);
    }

    public override void StartEffect()
    {
        OnAdd();
    }

    public override void OnAdd()
    {
        UIManager.Inst.AddASmallTip(target.actorName + "的火焰抗性提升了!");
        target.Prop.ResFireIncrease(val);
        UIManager.Inst.uiMain.AddABuffToTarget(target, this);
    }

}
