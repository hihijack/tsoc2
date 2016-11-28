using UnityEngine;
using System.Collections;

public class UIWords : MonoBehaviour {

    public UILabel txtName;
    public UILabel txtWords;
    public UIButton btn;

    public void Init(string name, string words)
    {
        txtName.text = name;
        txtWords.text = words;
        btn.onClick.Add(new EventDelegate(OnBtn_Close));
    }

    void OnBtn_Close()
    {
        UIManager._Instance.CloseNPCWords();
    }
}
