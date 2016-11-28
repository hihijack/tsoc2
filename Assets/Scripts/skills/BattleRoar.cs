using UnityEngine;
using System.Collections;
using SimpleJSON;

/// <summary>
/// 提升百分比护甲，持续一段时间
/// </summary>
public class BattleRoar : ISkill{

    public int buffId;
    public float buffDur;
    public float armAdd;

	public override void Init(){
		SetBaseData(GameDatas.GetSkillBD(1));
        JSONNode jdData = GetBaseData().jdData;
        this.buffId = jdData["buff"].AsInt;
        this.buffDur = jdData["dur"].AsFloat;
        this.armAdd = jdData["arm_add"].AsFloat;
	}
	
	public override void SetCaster(IActor _caster){
		caster = _caster;
	}
	
	public override IEnumerator Act ()
	{
		if(!InCD && CheckCost()){
			StartCD();
			caster.IsSkilling = true;
			//caster.PlayAnim("Attack2HA");
			
			// 施法前摇
            yield return new WaitForSeconds(0.5f);
			
			Buff_SuperArm bsa = caster.gameObject.AddComponent<Buff_SuperArm>();
            bsa.Init(caster, buffDur, armAdd);
            bsa.StartEffect();
			
			// 施法后摇
            yield return new WaitForSeconds(0.5f);
			caster.IsSkilling = false;
			yield return 0;
		}
	}
}