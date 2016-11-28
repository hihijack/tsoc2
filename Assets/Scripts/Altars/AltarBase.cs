using UnityEngine;
using System.Collections;

public class AltarBase : MonoBehaviour {

    public ItemNPC npc;
    
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Init(ItemNPC npc) 
    {
        this.npc = npc;
    }

    public virtual void OnActive(){}
}
