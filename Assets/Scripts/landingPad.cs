using UnityEngine;
using System.Collections;

public class landingPad : MonoBehaviour {


    public int scoreMultiplier;
    GameObject player;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
	
        

	}

    void updateScore()
    { 
        if(player!=null)
        {
            player.gameObject.SendMessage("addScore", scoreMultiplier);
        }

    }



}
