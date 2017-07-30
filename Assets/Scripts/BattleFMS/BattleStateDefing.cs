public class BattleStateDefing : IBattleState
{
    float maxTime = 1f;
    float curTime = 0f;
    private ManagerBattleState managerBattleState;

    public BattleStateDefing(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
        stateType = EBattleState.Defing;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartDef();
        curTime = 0f;
    }

    public override void Update()
    {
        base.Update();
        curTime += UnityEngine.Time.deltaTime;
        if (curTime >= maxTime)
        {
            managerBattleState.ActionStopDef();
        }
    }

    public override void End()
    {
        base.End();
        managerBattleState.hero.OnBSEndDef();
    }

    public override IBattleState ActionStopDef()
    {
        return managerBattleState.bsNormal;
    }
}
