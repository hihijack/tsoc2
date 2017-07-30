public class Buff_AltarFury : IBaseBuff {

    float percent;

    public void Init(IActor target, float percent, int dur) 
    {
        int id = 5;
        this.percent = percent;
        durRound = dur;
        this.target = target;
        baseData = GameDatas.GetBuff(id);
    }

    public override void StartEffect()
    {
        OnAdd();
        base.StartRoundCD();
    }

    public override void OnAdd()
    {
        if (target._State != EActorState.Dead)
        {
            target._Prop.IasParmaB *= (1 + percent);
            UIManager.Inst.uiMain.AddABuffToTarget(target, this);
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target._Prop.IasParmaB /= (1 + percent);
            UIManager.Inst.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
