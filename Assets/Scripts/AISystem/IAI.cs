public class IAI : UnityEngine.MonoBehaviour
{
    public IAIState curState;

    public Enermy npc;

    public virtual void Init(Enermy npc)
    {
        this.npc = npc;
    }

    /// <summary>
    /// 开始AI逻辑
    /// </summary>
    public virtual void DoStart()
    {
       
    }

    public virtual void DoUpdate() { }
    public virtual void ToAIState(IAIState next)
    {
        curState = next;
        curState.OnInto();
    }

    /// <summary>
    /// 检测目标是否硬直中
    /// </summary>
    /// <returns></returns>
    public bool CheckTargetUnCtling()
    {
        return false;
    }

    /// <summary>
    /// 硬直中
    /// </summary>
    /// <returns></returns>
    protected bool IsInUnCtl()
    {
      return npc.gFSMManager.InUnControl();
    }

    /// <summary>
    /// 回到静止状态的上一个状态。非静止状态返回Idle
    /// </summary>
    /// <returns></returns>
    protected EBattleState LastStateToIdle()
    {
        return npc.gFSMManager.LastStateToIdle();
    }

    /// <summary>
    /// 命中概率
    /// </summary>
    /// <param name="odd"></param>
    /// <returns></returns>
    public bool HitOdds(float odd)
    {
        return true;
    }
}
