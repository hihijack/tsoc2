public class BattleStateDodgeNPC : IBattleState
{
    internal float dur;
    float curTime;
    private ManagerBattleStateNPC manager;

    public BattleStateDodgeNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.Dodge;
    }

    public override IBattleState ActionDodgeEnd(float unControlTime)
    {
        return manager.bsIdle;
    }

    public override void Start()
    {
        base.Start();
        curTime = 0f;
        manager.npc.OnBSStartDodge();
    }

    public override void End()
    {
        base.End();
        manager.npc.OnBSEndDodge();
    }

    public override void Update()
    {
        base.Update();
        curTime += UnityEngine.Time.deltaTime;
        if (curTime > dur)
        {
            manager.ActionDogleTimeEnd(0f);
        }
    }
}