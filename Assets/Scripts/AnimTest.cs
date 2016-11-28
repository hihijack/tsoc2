using UnityEngine;
using System.Collections;

public class AnimTest : MonoBehaviour {

    public Animation anim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim["AttackA"].speed = 0.5f;
            anim.CrossFade("AttackA");
        }
	}

    IEnumerator CoTest()
    {
        while (true)
        {
            
            yield return new WaitForSeconds(0.1f);
        }
        
    }
}
