public class BUff_FireWpon : IBaseBuff
{
    private int val;

    public void Init(int dur, int val)
    {
        durRound = dur;
        this.val = val;
        baseData = GameDatas.GetBuff(10);
    }

    public override void StartEffect()
    {
        base.StartRoundCD();
        OnAdd();
    }

    public override void OnAdd()
    {
        base.OnAdd();
        Hero.Inst.Prop.atkFireParamAdd += val;
        UIManager.Inst.uiMain.AddABuffToTarget(Hero.Inst, this);
    }

    public override void OnRemove()
    {
        base.OnRemove();
        DestroyObject(this);
        Hero.Inst.Prop.atkFireParamAdd -= val;
        UIManager.Inst.uiMain.RemoveABuff(Hero.Inst, this);
    }
}