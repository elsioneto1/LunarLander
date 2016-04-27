using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

    public ParticleSystem particles;
    Transform _transform;
    Rigidbody2D rb;
    float force;
    
	// Use this for initialization
	void Start () {
        _transform = transform;
        rb = GetComponent<Rigidbody2D>();
        force = 0;
	}
	
	// Update is called once per frame
	void Update () {

        playerMovement();

	}

    void playerMovement()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _transform.eulerAngles += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            _transform.eulerAngles -= new Vector3(0, 0, 1);
        }

        if (_transform.eulerAngles.z > 90 && _transform.eulerAngles.z < 150)
        {
            _transform.eulerAngles = new Vector3(0, 0, 90);
        }
        if (_transform.eulerAngles.z < 270 && _transform.eulerAngles.z > 150)
        {
            _transform.eulerAngles = new Vector3(0, 0, 270);
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            force += 0.1f;
            if (force > 0.5f)
                force = 0.5f;
        } 
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            force -= 0.1f;
            if (force < 0)
                force = 0;
        }

        rb.AddForce(_transform.up * force);
    }
}
