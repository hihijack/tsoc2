using UnityEngine;
using System.Collections;

/// <summary>
/// 防御神坛
/// 护甲及抗性提升200%， 持续3回合
/// </summary>

public class AltarDef : AltarBase {

    float val;
    int dur;

    public override void Init(ItemNPC npc)
    {
        base.Init(npc);
		val = npc.npcData.data["val"].AsFloat;
		dur = npc.npcData.data ["dur"].AsInt;
    }

    
	public override void OnActive()

	{       
		base.OnActive();

		// 不叠加buff
		Buff_AltarDef bad = GameManager.hero.gameObject.GetComponent<Buff_AltarDef>();
		if (bad == null)
		{
			bad = Tools.AddCommentToGobj<Buff_AltarDef> (GameManager.hero.gameObject);
			bad.Init (GameManager.hero, val, dur);
			bad.StartEffect ();
			
			UIManager.Inst.GeneralTip("你的防御硬如钢铁",Color.green);
			// 使用后立即销毁
			DestroyObject(gameObject);
		}

	}
}
