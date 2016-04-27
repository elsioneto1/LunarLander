using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameProperties : MonoBehaviour {

    // class containing general game properties



    public static GameProperties SINGLETON;


    public GameObject canvas_UI;
    public GameObject canvas_menu;

    public Image arrow_up;
    public Image arrow_down;
    public Image arrow_left;
    public Image arrow_right;


    playerController controller;

    GameObject mainCamera;
    GameObject player;

    public float cameraMenuPosZ = 66.0f;
    public float cameraGamePosZ = -15.0f;

    bool play;
    bool gameOver;

	// Use this for initialization
	void Start () 
    {
        SINGLETON = this;
        play = false;
        gameOver = false;

        player = GameObject.Find("Player");
        controller = player.GetComponent<playerController>();
        arrow_right.gameObject.SetActive(false);
        arrow_left.gameObject.SetActive(false);
        arrow_up.gameObject.SetActive(false);
        arrow_down.gameObject.SetActive(false);

        canvas_UI.SetActive(false);

        mainCamera = GameObject.Find("Main Camera");
    }
	

	// Update is called once per frame
	void Update () 
    {       

        if (controller.velocity.x < 0)
        {
            arrow_right.gameObject.SetActive(false);
            arrow_left.gameObject.SetActive(true);
        }
        else if (controller.velocity.x > 0)
        {
            arrow_right.gameObject.SetActive(true);
            arrow_left.gameObject.SetActive(false);
        }

        if (controller.velocity.y < 0)
        {
            arrow_up.gameObject.SetActive(false);
            arrow_down.gameObject.SetActive(true);
        }
        else if (controller.velocity.y > 0)
        {
            arrow_up.gameObject.SetActive(true);
            arrow_down.gameObject.SetActive(false);
        }


        // This could be replace with a camera script to make it easier.

        if (play)
        {
            if (mainCamera.transform.position.z > cameraGamePosZ+5)
            {
                Vector3 velocity = Vector3.zero;
                float lerpTime = 0.3f;
                Vector3 targetPos = mainCamera.transform.position;
                targetPos.z = cameraGamePosZ;

                mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPos, ref velocity, lerpTime);
            }
            else
            {
                canvas_UI.SetActive(true);
                player.gameObject.SendMessage("playGame");
                play = false;
            }
        }



        if (gameOver)
        {
            if (mainCamera.transform.position.z < cameraMenuPosZ - 15)
            {
                Vector3 velocity = Vector3.zero;
                float lerpTime = 0.3f;
                Vector3 targetPos = mainCamera.transform.position;
                targetPos.z = cameraMenuPosZ;

                mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPos, ref velocity, lerpTime);
            }
            else
            {
                canvas_menu.SetActive(true);
                gameOver = false;
            }
        }      

	}

    public void playGame()
    {
        play = true;
        canvas_menu.SetActive(false);
    }

    public void quitGame()
    {

    }

    void showMenu()
    {
        gameOver = true;   
        canvas_UI.SetActive(false);
    }


    public void reset()
    {

        //score = 0;
        //minutes = 0;
        //time = 0;


    }
}
