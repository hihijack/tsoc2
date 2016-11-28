using UnityEngine;
using System.Collections;

/// <summary>
/// 提升百分比攻击
/// </summary>
public class Buff_SuperAtk : IBaseBuff {

	public float percent;
	
	// Use this for initialization
	void Start () {
		
	}
	
	public override void OnAdd ()
	{
		base.OnAdd();
        //target.AtkExtra += (int)(percent * target.atkPhy);
	}
	
	public override void OnRemove ()
	{
		base.OnRemove();
        //target.AtkExtra -= (int)(percent * target.atkPhy);
	}

}
