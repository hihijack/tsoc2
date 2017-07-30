public class Buff_AltarDef : IBaseBuff
{

    float percent;


    public void Init(IActor target, float percent, int dur)
    {
        int id = 6;
        this.percent = percent;
        durRound = dur;
        this.target = target;
        baseData = GameDatas.GetBuff(id);
    }

    public override void OnAdd()
    {
        if (target._State != EActorState.Dead)
        {
            // 提升防御，抗性
            target._Prop.DefIncrease(percent);
            target._Prop.ResFireIncrease(percent);
            target._Prop.ResForzenIncrease(percent);
            target._Prop.ResPosisionIncrease(percent);
            target._Prop.ResThunderIncrease(percent);

            UIManager.Inst.uiMain.AddABuffToTarget(target, this);
        }
    }

    public override void StartEffect()
    {
        OnAdd();
        base.StartRoundCD();
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target._Prop.DefIncrease(1 / percent);
            target._Prop.ResFireIncrease(1 / percent);
            target._Prop.ResForzenIncrease(1 / percent);
            target._Prop.ResPosisionIncrease(1 / percent);
            target._Prop.ResThunderIncrease(1 / percent);
            UIManager.Inst.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
