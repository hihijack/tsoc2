public class Buff_AddResPosi : IBaseBuff {

    int addval;

    public void Init(IActor target, int addval, int durRound) 
    {
        int id = 3;
        this.addval = addval;
        this.durRound = durRound;
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
            target.Prop.resPoision += addval;
            UIManager.Inst.uiMain.AddABuffToTarget(target, this);
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target.Prop.resPoision -= addval;
            UIManager.Inst.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
