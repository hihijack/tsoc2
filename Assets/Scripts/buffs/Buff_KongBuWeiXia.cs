using UnityEngine;
using System.Collections;

/// <summary>
/// 恐怖威吓：降低攻击力及防御力
/// </summary>

public class Buff_KongBuWeiXia : IBaseBuff {

    float percent;

    public void Init(IActor target, float percent) 
    {
        int id = 4;
        this.percent = percent;
        this.target = target;
        this.durRound = 1;
        baseData = GameDatas.GetBuff(id);
    }

    public override void StartEffect()
    {
        base.StartEffect();
        OnAdd();
        base.StartRoundCD();
    }

    public override void OnAdd()
    {
        base.OnAdd();
        if (target._State != EActorState.Dead)
        {
            target.Prop.AtkParmaC *= (1 - percent);
            target.Prop.DefIncrease(1 - percent);
            UIManager.Inst.uiMain.AddABuffToTarget(target, this);
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target.Prop.AtkParmaC /= (1 - percent);
            target.Prop.DefIncrease(1 / (1 - percent));
            UIManager.Inst.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
