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
            target.DefIncrease(percent);
            target.ResFireIncrease(percent);
            target.ResForzenIncrease(percent);
            target.ResPosisionIncrease(percent);
            target.ResThunderIncrease(percent);

            UIManager._Instance.uiMain.AddABuffToTarget(target, this);
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
            target.DefIncrease(1 / percent);
            target.ResFireIncrease(1 / percent);
            target.ResForzenIncrease(1 / percent);
            target.ResPosisionIncrease(1 / percent);
            target.ResThunderIncrease(1 / percent);
            UIManager._Instance.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
