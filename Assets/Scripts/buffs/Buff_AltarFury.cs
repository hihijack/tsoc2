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
            target._IAS *= (1 + percent);
            UIManager._Instance.uiMain.AddABuffToTarget(target, this);
        }
    }

    public override void OnRemove()
    {
        base.OnRemove();
        if (target._State != EActorState.Dead)
        {
            target._IAS /= (1 + percent);
            UIManager._Instance.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
    }
}
