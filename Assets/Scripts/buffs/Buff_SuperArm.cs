using UnityEngine;
using System.Collections;

/// <summary>
/// 提升百分比护甲，持续一段时间
/// </summary>
public class Buff_SuperArm : IBaseBuff{
	
    public float percent;

    int armAdd;

    public void Init(IActor target, float dur, float percent)
    {
        int id = 1;

        this.target = target;
        this.durTime = dur;
        this.percent = percent;
        baseData = GameDatas.GetBuff(id);
    }


    public override void StartEffect()
    {
        OnAdd();
        base.StartCD();
    }

	public override void OnAdd ()
	{
        if (target._State != EActorState.Dead)
        {
            armAdd = (int)(percent * target.arm);
            target.arm += armAdd;
            UIManager._Instance.uiMain.AddABuffToTarget(target, this);
        }
	}

    public override void OnRemove()
    {
        if (target._State != EActorState.Dead)
        {
            target.arm -= armAdd;
            UIManager._Instance.uiMain.RemoveABuff(target, this);
        }
        DestroyObject(this);
	}
	
}
