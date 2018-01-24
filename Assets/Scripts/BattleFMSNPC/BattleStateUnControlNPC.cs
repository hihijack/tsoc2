public class BattleStateUnControlNPC : IBattleState
{
    internal float dur;
    private ManagerBattleStateNPC manager;

    public BattleStateUnControlNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.manager = managerBattleStateNPC;
        _stateType = EBattleState.Uncontrol;
    }

    public override IBattleState ActionUnControlEnd()
    {
        return manager.bsIdle;
    }

    public override void Start()
    {
        base.Start();
        manager.npc.OnBSStartUnCtl();
    }

    public override void End()
    {
        base.End();
        manager.npc.OnBSEndUnCtl();
    }

    public override void Update()
    {
        base.Update();
        dur -= UnityEngine.Time.deltaTime;
        if (dur <= 0)
        {
            //持续结束
            manager.ActionUnControlEnd();
        }
    }
}