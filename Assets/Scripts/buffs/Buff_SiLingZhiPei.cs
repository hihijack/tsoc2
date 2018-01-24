using UnityEngine;
using System.Collections;

/// <summary>
/// BUFF - 死灵支配
/// </summary>
public class Buff_SiLingZhiPei : IBaseBuff
{
    float pstAtk;
    float pstArm;
    
    public void Init(IActor target, float pstAtk, float pstArm)
    {
        this.target = target;
        this.pstAtk = pstAtk;
        this.pstArm = pstArm;
        baseData = GameDatas.GetBuff(9);
    }

    public override void StartEffect()
    {
        base.StartEffect();
        OnAdd();
    }

    public override void OnAdd()
    {
        base.OnAdd();
        UIManager.Inst.AddASmallTip(target.actorName + "的攻击力防御力提升了!");
        target.Prop.DefIncrease(1 + pstArm);
        target.Prop.AtkParmaC += pstAtk;
    }
}
