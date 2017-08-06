public class BattleStateDefingNPC : IBattleState
{
    private ManagerBattleStateNPC mananger;

    public BattleStateDefingNPC(ManagerBattleStateNPC managerBattleStateNPC)
    {
        this.mananger = managerBattleStateNPC;
        _stateType = EBattleState.Defing;
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
}