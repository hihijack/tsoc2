using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("NGUI/GameKit/UIArrangeList")]
public class UIArrangeList : MonoBehaviour {
	public Vector2 cellsize = Vector2.zero;
	public bool hideInactive = true;
	public bool repositionNow = false;
	
	List<Transform> mChildren = new List<Transform>();
	
	public List<Transform> children
	{
		get
		{
			if (mChildren.Count == 0)
			{
				Transform myTrans = transform;
				mChildren.Clear();

				for (int i = 0; i < myTrans.childCount; ++i)
				{
					Transform child = myTrans.GetChild(i);

					if (child && child.gameObject && (!hideInactive || NGUITools.GetActive(child.gameObject))) mChildren.Add(child);
				}
			}
			return mChildren;
		}
	}
	
	public void Reposition ()
	{
		Transform myTrans = transform;
		mChildren.Clear();
		List<Transform> ch = children;
		if (ch.Count > 0) 
			RepositionVariableSize(ch);
	}
	
	void RepositionVariableSize (List<Transform> ch)
	{
		int count = ch.Count;
		float offsetX;
		if(count % 2 == 0){
			int half = count / 2;
			offsetX = -1 *((half - 1) * cellsize.x + cellsize.x * 0.5f);
		}else{
			int half = count / 2;
			offsetX = -1 * half * cellsize.x;
		}
		
		for (int i = 0; i < count; i++) {
			Transform t = ch[i];
			Vector3 pos = t.localPosition;
			pos.x = offsetX + i * cellsize.x;
			t.localPosition = pos;
		}
	}
	
	void Start () {
		Reposition();
	}
	
	void LateUpdate ()
	{
		if (repositionNow)
		{
			repositionNow = false;
			Reposition();
		}
	}
}
