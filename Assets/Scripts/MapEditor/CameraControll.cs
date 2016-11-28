using UnityEngine;
using System.Collections;

public class CameraControll : MonoBehaviour {

    public float speed;
    public float rollSpeed;
    // Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime, Input.GetAxis("Mouse ScrollWheel") * rollSpeed * Time.deltaTime);
	}
}
