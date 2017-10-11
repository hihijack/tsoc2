using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

    public Transform worldPos;
    public Transform t;
    public Transform cam;
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
      
    }

    void OnGUI()
    {
        GUILayout.Label("worldToLocal:" + cam.InverseTransformPoint(worldPos.position));
        GUILayout.Label("worldToLocal2:" + cam.worldToLocalMatrix.MultiplyPoint(worldPos.position));
        GUILayout.Label("worldToLocal3:" + cam.worldToLocalMatrix.MultiplyVector(worldPos.position));
    }
}
