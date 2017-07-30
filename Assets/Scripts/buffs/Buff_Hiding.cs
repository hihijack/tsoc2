using UnityEngine;
/// <summary>
/// buff。持续隐身，直到解除
/// </summary>
public class Buff_Hiding : IBaseBuff
{
    Enermy etarget;
    public void Init(Enermy target)
    {
        etarget = target;
        this.target = target;
    }

    public override void StartEffect()
    {
        OnAdd();
    }

    public override void OnAdd()
    {
        etarget.RefreshHiding(true);
    }

    public override void OnRemove()
    {
        etarget.RefreshHiding(false);
    }
}