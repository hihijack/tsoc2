using System;

public class Buff_Hide : IBaseBuff
{
    
    internal void Init(int dur)
    {
        durRound = dur;
    }

    public override void StartEffect()
    {
        base.StartRoundCD();
        OnAdd();
    }

    public override void OnAdd()
    {
        base.OnAdd();
        Hero.Inst.Avroar2D.alpha = 0.5f;
    }

    public override void OnRemove()
    {
        base.OnRemove();
        DestroyObject(this);
        Hero.Inst.Avroar2D.alpha = 1f;
    }
}