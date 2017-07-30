using UnityEngine;
using System.Collections;

public class ISkill : MonoBehaviour{

    private int level;

    public int _Level
    {
        get { return level; }
        set 
        {
            int oriLevel = level;
            level = value;
            if (oriLevel > 0 && level > 0)
            {
                OnLevelChange();
            }
        }
    }
    
    public IActor caster;

    public virtual IEnumerator Act() { yield return 0; }

    public virtual void StartEff() { } // 被动技能开始生效
    public virtual void RemoveEff() { } // 被动技能结束生效

    public virtual void OnLevelChange() { } // 当等级改变

    private SkillBD baseData;
	
	protected bool _inCD;
	public bool InCD{
		get{
			return _inCD;
		}
		
		set{
			_inCD = value;
		}
	}
	
	
	public void SetBaseData(SkillBD bd)
    {
        this.baseData = bd;
    }

    public SkillBD GetBaseData()
    {
        return baseData;
    }

    public int range;       // 施法距离。只有战术技能需要。0：无需检测距离

	public virtual void SetCaster(IActor caster){}
    public virtual void SetTarget(IActor target) { }

    public virtual void Init() { }

	protected void StartCD(){
		InCD = true;
		StartCoroutine(CoCDTime());
        UIManager.Inst.uiMain.StartSkillCD(this, GetBaseData().cd);
    }
	
	IEnumerator CoCDTime(){
		yield return new WaitForSeconds(baseData.cd);
		InCD = false;
	}
	
	protected bool CheckCost(){
        bool r = false;
        int cost = baseData.cost;
        if (caster.isHero)
        {
            Hero hero = caster as Hero;
            if (hero._Prop.EnergyPoint >= cost)
            {
                r = true;
            }
            else
            {
                // 怒气不足
                UIManager.Inst.GeneralTip("能量点不足", Color.red);
            }
        }
        else
        {
            r = true;
        }
        return r;
	}

    protected void StartCost()
    {
        Hero hero = caster as Hero;
        hero._Prop.EnergyPoint -= baseData.cost;
        UIManager.Inst.uiMain.RefreshHeroEnergy();
    }

    public virtual bool CheckCast()
    {
        return false;
    }
}
