using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗神坛buff
/// </summary>
public class Buff_AltarFight : IBaseBuff
{
    float percent;

    public void Init(IActor target, float percent, int dur)
    {
        int id = 7;
        this.percent = percent;
        durRound = dur;
        this.target = target;
        baseData = GameDatas.GetBuff(id);
    }

    public override void OnAdd()
    {
        base.OnAdd();
        if (target._State != EActorState.Dead)
        {
            target._Prop.AtkParmaC *= percent;
            UIManager.Inst.uiMain.AddABuffToTarget(target, this);
        }

    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target._Prop.AtkParmaC /= percent;
            UIManager.Inst.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }

    public override void StartEffect()
    {
        OnAdd();
        StartRoundCD();
    }

}

