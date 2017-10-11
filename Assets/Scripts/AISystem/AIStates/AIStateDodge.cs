public class AIStateDodge : IAIState
{
    public AIStateDodge(IAI AI) : base(AI)
    {
    }
    public override void OnInto()
    {
        base.OnInto();
        AI.NPC.gFSMManager.ActionDodge(1f);
    }
}