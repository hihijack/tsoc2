using UnityEngine;
using System.Collections;

public class IBaseBuff : MonoBehaviour {

	public IActor target;
	public float durTime;
    public int durRound;

    public BuffBaseData baseData;
	
	public virtual void StartCD(){
		StartCoroutine(CoTime());
	}
	
	public virtual void OnAdd(){
	}
    public virtual void OnRemove() { EventsMgr.GetInstance().DetachEvent(eEventsKey.RoundChange, OnRoundChange); }
	
	public virtual void DoPerSecond(){}

    public virtual void StartEffect() { }
	
	IEnumerator CoTime(){
		while(true){
			if(durTime > 0){
				yield return new WaitForSeconds(1f);
				durTime --;
				if(durTime == 0){
					OnRemove();
					break;
				}else{
					DoPerSecond();
				}
			}else{
				break;
			}
		}
	}

    /// <summary>
    /// 开始回合制cd
    /// </summary>
    /// 
    int startRound;
    public virtual void StartRoundCD() 
    {
        startRound = GameView.Inst.GRoundCount;
        EventsMgr.GetInstance().AttachEvent(eEventsKey.RoundChange, OnRoundChange);
        
    }

    void OnRoundChange(object data) 
    {
        int curRound = (int)data;
        if (curRound - startRound < 0 || curRound - startRound > durRound)
        {
            OnRemove();
        }
        else
        {
            DoPerSecond();
        }
    }

    public void Remove()
    {
        OnRemove();
        DestroyObject(this);
    }
}
