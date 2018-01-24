public class BattleStateDefingNPC : IBattleState
{
    private ManagerBattleStateNPC mananger;

    public BattleStateDefingNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.mananger = managerBattleStateNPC;
        _stateType = EBattleState.Defing;
    }

    public override void Start()
    {
        base.Start();
        //创建特效
        mananger.npc.OnBSStartDef();
    }

    public override void End()
    {
        base.End();
        mananger.npc.OnBSEndDef();
    }

    public override IBattleState ActionStopDef()
    {
        return mananger.bsIdle;
    }

    public override IBattleState ActionUnControl(float dur)
    {
        mananger.bsUnControl.dur += dur;
        return mananger.bsUnControl;
    }

    public override IBattleState ActionAtk(int skillId, IActor target)
    {
        mananger.bsAtkBefore.skillId = skillId;
        mananger.bsAtkBefore.target = target;
        return mananger.bsAtkBefore;
    }
}