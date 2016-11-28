using UnityEngine;
using System.Collections;

// How To Use:
//1.Add UINumberSelect To Parernt GameObject
//2.Create BtnAdd , BtnReduce, LabelNumber GameObject;
//3.Appoint Reference
// API: OnNumChange();GetCurNumber();GetAllPrice();Reset();
[AddComponentMenu("NGUI/GameKit/UINumberSelect")]
public class UINumberSelect : MonoBehaviour {

	public GameObject btnAdd;
	public GameObject btnReduce;
	public UILabel labelNumber;
	
	public int defaultNum = 1;
	public bool useUpperLimit = false;
	public int upperlimitNum = 0;
	public bool useLowerLimit = false;
	public int lowerlimitNum = 0;
	
	private int curNum;
	
	public delegate void DelegateOnNumChange(int curNum);
	public DelegateOnNumChange OnNumChange;
	
	
	public int price;
	private int allPrice;
	public UILabel labelAllPrice;
	
	// Use this for initialization
	void Start () {
		if(btnAdd != null){
			UIButtonMessage ubmAdd = btnAdd.GetComponent<UIButtonMessage>();
			if(ubmAdd == null){
				ubmAdd = btnAdd.AddComponent<UIButtonMessage>();
			}
			ubmAdd.target = gameObject;
			ubmAdd.trigger = UIButtonMessage.Trigger.OnClick;
			ubmAdd.functionName = "OnAddClick";
		}else{
			Debug.LogError("UINumberSelect BtnAdd No Init");
		}
		if(btnReduce != null){
			UIButtonMessage ubmReduce = btnReduce.GetComponent<UIButtonMessage>();
			if(ubmReduce == null){
				ubmReduce = btnReduce.AddComponent<UIButtonMessage>();
			}
			ubmReduce.target = gameObject;
			ubmReduce.trigger = UIButtonMessage.Trigger.OnClick;
			ubmReduce.functionName = "OnReduceClick";
		}else{
			Debug.LogError("UINumberSelect BtnReduce No Init");
		}
		
		if(labelNumber == null){
			Debug.LogError("UINumberSelect LabelNumber No Init");
		}
		
		curNum = defaultNum;
		RefreshByCurNum();
	}
	
	public void OnAddClick(){
		curNum ++;
		if(useUpperLimit){
			if(curNum > upperlimitNum){
				curNum = upperlimitNum;
			}
		}
		RefreshByCurNum();
	}
	
	public void OnReduceClick(){
		curNum --;
		if(useLowerLimit){
			if(curNum < lowerlimitNum){
				curNum = lowerlimitNum;
			}
		}
		RefreshByCurNum();
	}
	
	private void RefreshByCurNum(){
		if(OnNumChange != null){
			OnNumChange(curNum);
		}
		labelNumber.text = curNum.ToString();
		
		allPrice = curNum * price;
		if(labelAllPrice != null){
			labelAllPrice.text = allPrice.ToString();
		}
	}
	
	public int GetCurNumber(){
		return curNum;
	}
	
	public void Reset(){
		curNum = defaultNum;
		RefreshByCurNum();
	}
	
	public int GetAllPrice(){
		return allPrice;
	}
}
