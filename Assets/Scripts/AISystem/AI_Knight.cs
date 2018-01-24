using System;
using UnityEngine;

public class AI_Knight : IAI
{
    public RandomVal rdmIdleTime = new RandomVal();
    public float oddsDef = 0.5f;
    public RandomVal rdmDefDur = new RandomVal();

    AIStateIdle sIdle;
    AIStateAtk sAtkL;
    AIStateAtk sAtkH;
    AIStateDef sDef;

    public override void Init(Enermy npc)
    {
        base.Init(npc);
        sIdle = new AIStateIdle(this);
        sAtkL = new AIStateAtk(27, this);
        sAtkH = new AIStateAtk(28, this);
        sDef = new AIStateDef(this);
    }

    

    public override void DoStart()
    {
        sAtkL.target = npc.curBattleTarget;
        sAtkH.target = npc.curBattleTarget;

        ToAIState(sIdle);
    }

    public override void DoUpdate()
    {
        Update_Idle();
        Update_AtkL();
        Update_AtkH();
        Update_Def();
    }

    private void Update_Def()
    {
        if (curState == sDef)
        {
            sDef.dur += Time.deltaTime;

            //硬直 - 失败
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if (sDef.dur >= rdmDefDur.RanVal())
            {
                //持续防御后
                ToAIState(sAtkL);
            }
        }
    }

    private void Update_AtkH()
    {
        if (curState == sAtkH)
        {
            //硬直 - 失败
            //重击结束回复
            if (IsInUnCtl() || IsAtkSuccess())
            {
                ToAIState(sIdle);
            }
        }
    }

    private void Update_AtkL()
    {
        if (curState == sAtkL)
        {
            //硬直 - 失败
            //轻击接重击
            if (IsInUnCtl())
            {
                ToAIState(sIdle);
            }
            else if(IsAtkSuccess())
            {
                ToAIState(sAtkH);
            }
        }
    }

    private void Update_Idle()
    {
        if (curState == sIdle)
        {
            curState.dur += Time.deltaTime;

            if (curState.dur >= rdmIdleTime.RanVal())
            {
                if (Tools.IsHitOdds(oddsDef))
                {
                    //格挡
                    ToAIState(sDef);
                }
                else
                {
                    ToAIState(sAtkL);
                }
            }
        }
    }
}