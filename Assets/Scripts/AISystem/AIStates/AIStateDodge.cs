public class AIStateDodge : IAIState
{
    public AIStateDodge(IAI AI) : base(AI)
    {
    }
    public override void OnInto()
    {
        base.OnInto();
        AI.npc.gFSMManager.ActionDodge(1f);
    }
}