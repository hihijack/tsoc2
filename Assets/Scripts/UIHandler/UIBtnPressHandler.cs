using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIButton))]
public class UIBtnPressHandler : MonoBehaviour {

    GameView gameView;
    UIButton btn;
	// Use this for initialization
	void Awake () {
        gameView = GameObject.FindGameObjectWithTag("CPU").GetComponent<GameView>();
        btn = GetComponent<UIButton>();
	}

    void OnPress(bool pressed)
    {
        UIManager._Instance.OnBtnPress(btn, pressed);
    }
}
