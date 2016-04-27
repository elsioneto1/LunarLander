using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {



    public static CameraManager SINGLETON;
    public playerController pController;
    GameObject camera;

    bool landing = false;
    Vector3 oriPos;
    float oriSize;

	// Use this for initialization
	void Start () {

        SINGLETON = this;
        camera  = Camera.main.gameObject;
        oriSize = Camera.main.orthographicSize;
        oriPos  = camera.transform.position;

	}
	

	// Update is called once per frame
	void Update () {

        cameraLogic();


	}

    void cameraLogic()
    {

        if (pController == null)
            return;

        if (pController.altitude < 200 && pController.altitude != 0)
        {
            
            landing = true;

        }

        if (landing && pController.gameObject.active)
        {
           
            Camera.main.orthographicSize = 4;
            camera.transform.position = new Vector3(pController.transform.position.x,
                                                    pController.transform.position.y,-10);

        }
    }

    public void reset()
    {

        landing                         = false;
        Camera.main.orthographicSize    = oriSize;
        camera.transform.position       = new Vector3( oriPos.x, oriPos.y, -10);

    }


 

}
