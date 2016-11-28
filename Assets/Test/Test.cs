using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public UILabel txt;
    int count;
    int hit2;
    int hit8;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        count++;
        System.Random ran = new System.Random();
        int ranVal = ran.Next(1, 101);
        if (ranVal <= 2)
        {
            hit2++;
        }
        else if (ranVal <= 8)
        {
            hit8++;
        }
        Debug.Log(string.Format("{0}:{1} / {2}", count, hit2, hit8));//###########
	}
}
