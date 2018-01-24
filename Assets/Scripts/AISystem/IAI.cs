public class IAI : UnityEngine.MonoBehaviour
{
    public IAIState curState;

    protected bool atkMiss = false;

    protected Enermy npc;

    public virtual void Init(Enermy npc)
    {
        this.npc = npc;
    }

    public Enermy NPC
    {
        get
        {
            return npc;
        }
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
        atkMiss = false;
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

    protected bool IsInIdle()
    {
        return npc.gFSMManager.InIdle();
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
    /// 回到静止状态且攻击丢失了
    /// </summary>
    /// <returns></returns>
    protected bool IsAtkMiss()
    {
        bool r = IsInIdle() && atkMiss;
        //UnityEngine.Debug.LogError("IsInIdle:" + IsInIdle() + ",miss:" + atkMiss);//##########
        return r;
    }


    /// <summary>
    /// 成功执行一次攻击
    /// </summary>
    /// <returns></returns>
    public bool IsAtkSuccess()
    {
        return LastStateToIdle() == EBattleState.AtkAfter;
    }

    public bool IsDodgeSuccess()
    {
        return LastStateToIdle() == EBattleState.Dodge;
    }

    /// <summary>
    /// 命中概率
    /// </summary>
    /// <param name="odd"></param>
    /// <returns></returns>
    public bool HitOdds(float odd)
    {
        return Tools.IsHitOdds(odd);
    }

    /// <summary>
    /// 当被攻击
    /// </summary>
    public virtual void OnAtked(IActor atker) { }

    public virtual void OnAtkMiss() { atkMiss = true; }
}
