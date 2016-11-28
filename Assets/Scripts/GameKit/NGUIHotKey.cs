using UnityEngine;
using System.Collections;

public class NGUIHotKey : MonoBehaviour {

    public KeyCode hotKey;
    UIButton btn;

    void Start()
    {
        btn = GetComponent<UIButton>();
    }

	void Update () {
        if (Input.GetKeyDown(hotKey))
        {
            btn.Click();
        }
	}
}
