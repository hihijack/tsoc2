using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗神坛:
/// 提升攻击力
/// </summary>

public class AltarFight : AltarBase {

		float val;
		int dur;

		public override void Init (ItemNPC npc)
	{
		base.Init (npc);
		this.val = npc.npcData.data ["val"].AsFloat;
		this.dur = npc.npcData.data ["dur"].AsInt;
	}

		public override void OnActive ()
	{
		base.OnActive ();
		// 不允许叠加buff
		Buff_AltarFight baf = GameManager.hero.gameObject.GetComponent<Buff_AltarFight>();
		if (baf == null) 
		{
			
			baf = GameManager.hero.gameObject.AddComponent<Buff_AltarFight> ();
			baf.Init (GameManager.hero, val, dur);
			baf.StartEffect ();
			UIManager.Inst.GeneralTip("你觉得充满力量",Color.green);
			// 使用后立即销毁
			DestroyObject(gameObject);
		}
	}
}
