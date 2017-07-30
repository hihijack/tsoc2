public class BattleStateUnControl : IBattleState
{
    public float dur;
    private ManagerBattleState managerBattleState;

    public BattleStateUnControl(ManagerBattleState managerBattleState)
    {
        this.managerBattleState = managerBattleState;
    }

    public override void Start()
    {
        base.Start();
        managerBattleState.hero.OnBSStartUnControl(dur);
    }

    public override void Update()
    {
        base.Update();
        dur -= UnityEngine.Time.deltaTime;
        managerBattleState.hero.OnBSUpdateUnControl(dur);
        if (dur <= 0)
        {
            //持续结束
            managerBattleState.ActionUnControlEnd();
        }
    }

    public override IBattleState ActionUnControlEnd()
    {
        return managerBattleState.bsNormal;
    }
}