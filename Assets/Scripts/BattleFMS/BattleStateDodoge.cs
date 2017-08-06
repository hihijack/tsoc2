using UnityEngine;

public class BattleStateDodoge : IBattleState
{
    public float dur;
    float curTime;
    private ManagerBattleState manager;

    public BattleStateDodoge(ManagerBattleState manager)
    {
        this.manager = manager;
        _stateType = EBattleState.Dodge;
    }

    public override void Start()
    {
        base.Start();
        manager.hero.OnBSStartDodge(dur);
        curTime = 0f;
    }

    public override void End()
    {
        base.End();
        manager.hero.OnBSEndDodge();
    }

    public override void Update()
    {
        base.Update();
        curTime += Time.deltaTime;
        if (curTime > dur)
        {
            manager.ActionDodgeEnd(0.5f);
        }
    }

    public override IBattleState ActionDodgeEnd(float unControlTime)
    {
        manager.bsUnControl.dur = unControlTime;
        return manager.bsUnControl;
    }
}