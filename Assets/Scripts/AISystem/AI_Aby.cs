using System;
using UnityEngine;

public class AI_Aby : IAI
{
    AIStateIdle stateIdle;
    AIStateAtk stateAtk;
    AIStateAtk stateHeavy;
    AIStateDef stateDef;
    AIStateAtk stateStomp;

    public override void Init(Enermy npc)
    {
        base.Init(npc);
        stateIdle = new AIStateIdle(this);
        stateAtk = new AIStateAtk(1,this);
        stateHeavy = new AIStateAtk(2,this);
        stateDef = new AIStateDef(this);
        stateStomp = new AIStateAtk(3,this);
    }

    public override void DoStart()
    {
        ToAIState(stateIdle);
    }

    public override void DoUpdate()
    {
        Update_Idle();
        Update_Atk();
        Update_AtkHeavy();
        Update_Def();
        Update_Stomp();
    }

    private void Update_Stomp()
    {
        if (curState == stateStomp)
        {
            //硬直 - 失败
            if (IsInUnCtl())
            {
                ToAIState(stateIdle);
            }
            else if (true)
            {
                //回到静止状态，且上一个状态是轻击 - 成功
                ToAIState(stateIdle);
            }
        }
    }

    private void Update_Def()
    {
        if (curState == stateDef)
        {
            stateDef.dur += Time.deltaTime;

            //硬直 - 失败
            if (IsInUnCtl())
            {
                ToAIState(stateIdle);
            }
            else if (stateDef.dur >= 3)
            {
                //持续防御3秒后
                if (HitOdds(0.5f))
                {
                    ToAIState(stateHeavy);
                }
                else
                {
                    ToAIState(stateAtk);
                }
            }
        }
    }

    private void Update_AtkHeavy()
    {
        if (curState == stateHeavy)
        {
            //硬直 - 失败
            if (IsInUnCtl())
            {
                ToAIState(stateIdle);
            }
            else if (true)
            {
                //回到静止状态，且上一个状态是轻击 - 成功
                ToAIState(stateIdle);
            }
        }
    }

    private void Update_Atk()
    {
        if (curState == stateAtk)
        {
            //硬直 - 失败
            if (IsInUnCtl())
            {
                ToAIState(stateIdle);
            }
            else if (true)
            {
                //回到静止状态，且上一个状态是轻击 - 成功
                ToAIState(stateStomp);
            }
        }
    }

    private void Update_Idle()
    {
        if (curState == stateIdle)
        {
            if (CheckTargetUnCtling())
            {
                //概率轻击或重击
                if (HitOdds(0.5f))
                {
                    ToAIState(stateAtk);
                }
                else
                {
                    ToAIState(stateHeavy);
                }
            }
            else if(stateIdle.dur >= 0.5f)
            {
                //静止0.5S
                ToAIState(stateDef);
            }

            curState.dur += Time.deltaTime;
        }
    }
}