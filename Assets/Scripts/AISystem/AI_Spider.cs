using System;
using UnityEngine;
/// <summary>
/// 蜘蛛AI：轻击接重击
/// </summary>
public class AI_Spider : IAI
{
    public RandomVal rdmIdleDur = new RandomVal();
    public int atkLCount;
    AIStateIdle sIdle;
    AIStateAtk sAtkL;
    AIStateAtk sAtkH;
    int mCurAtkCount;
    public override void Init(Enermy npc)
    {
        base.Init(npc);
        sIdle = new AIStateIdle(this);
        sAtkL = new AIStateAtk(27, this);
        sAtkH = new AIStateAtk(28, this);
    }

    public override void DoStart()
    {
        base.DoStart();
        mCurAtkCount = 0;
        ToAIState(sIdle);
    }

    public override void DoUpdate()
    {
        base.DoUpdate();
        Update_Idle();
        Update_AtkL();
        Update_AtkH();
    }

    private void Update_AtkH()
    {
        if (curState == sAtkH)
        {
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (IsAtkSuccess())
            {
                ToAIState(sIdle);
            }
        }
    }

    private void Update_AtkL()
    {
        if (curState == sAtkL)
        {
            //轻击接重击
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (IsAtkSuccess())
            {
                mCurAtkCount--;
                if (mCurAtkCount > 0)
                {
                    //连击
                    ToAIState(sAtkL);
                }
                else
                {
                    //接重击
                    ToAIState(sAtkH);
                }
            }
        }
    }

    private void Update_Idle()
    {
        if (curState == sIdle)
        {
            curState.dur += Time.deltaTime;
            if (curState.dur >= rdmIdleDur.RanVal())
            {
                mCurAtkCount = atkLCount;
                ToAIState(sAtkL);
            }
        }
    }
}