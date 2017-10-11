using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
public class BtnHandler : MonoBehaviour {

    UIButton btn;

    void Awake() 
    {
        btn = GetComponent<UIButton>();
    }

    void OnPress(bool pressed)
    {
        //UIManager.Inst.OnBtnPress(btn, pressed);
    }
}
