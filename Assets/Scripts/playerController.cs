using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class playerController : MonoBehaviour {

    Transform _transform;
    Rigidbody2D rb;
    public ParticleSystem ps;
    public float force;
    public float safeVForce = -40;
    float fuel;
    int score;
    float time;
    int minutes = 0;

    public Text txt_altitude;
    public Text txt_vForce;
    public Text txt_hForce;
    public Text t_fuel;
    public Text t_Score;
    public Text t_Time;
    public float altitude;
    public Vector2 velocity;
    bool play;

    
    public GameObject brokenShip;
    GameObject ship;
    GameObject gameController;

    AudioSource audio = new AudioSource();
    public AudioClip sfx_explosion;
    public AudioClip sfx_landing;
    public AudioClip sfx_thruster;
    Transform shipClone;
    Vector3 startPos ;

	// Use this for initialization
	void Start () {
        altitude = 0;
        _transform = transform;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        gameController = GameObject.Find("GameController");
        audio = this.transform.GetComponent<AudioSource>();
        force = 0;
        fuel = 1000;
        score = 0;
        time = 0;
        play = false;
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        ship = this.transform.FindChild("ship").gameObject;       
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (play)
        {
            playerMovement();
            collisionCheck();


            time += Time.deltaTime;
            if (time > 60)
            {
                time = 0;
                minutes++;
            }

            t_Time.text = minutes + " : " + time.ToString("F0"); ;

        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {
        if (play)
        {
            if (other.gameObject.tag == "landingPad")           // Landed on a pad
            {
                float angle = transform.rotation.eulerAngles.z;  // Get the ship rotation from 0 to 360
                float vForce = Mathf.Floor(velocity.y * 100);
                if (vForce > safeVForce && (angle < 5 || angle > 355))
                {
                    //Checking the 
                    Vector2 pos = transform.position;
                    pos.x -= 0.2f;
                    RaycastHit2D hitObj1 = Physics2D.Raycast(pos, -Vector2.up); // raycasting downward
                    Debug.DrawRay(pos, -Vector2.up, Color.green);
                    pos.x += 0.6f;
                    RaycastHit2D hitObj2 = Physics2D.Raycast(pos, -Vector2.up);
                    Debug.DrawRay(pos, -Vector2.up, Color.green);
                    if (hitObj1.collider != null && hitObj2.collider != null) // Checking if ship is completely inside the landing pad
                    {
                        if (hitObj1.collider.gameObject.tag == "landingPad" && hitObj2.collider.gameObject.tag == "landingPad")
                        {
                            Debug.Log(" Safe Landing");
                            other.gameObject.SendMessage("updateScore");
                            ps.Stop();
                            play = false;
                            audio.clip = sfx_landing;
                            audio.Play();
                            StartCoroutine(resetPlayer(6));
                            StartCoroutine("nextlevel");
                        }
                        else
                        {
                            Debug.Log(" Ship landed on " + hitObj2.collider.gameObject.tag);
                            explode();
                        }
                    }
                }
                else
                {
                    Debug.Log(" Ship speed : " + vForce + ". Ship angle : " + angle);
                    explode();
                }
            }
            else if (other.gameObject.tag == "terrain")  // landed on terrain.
            {
                explode();
            }
        }
    }

    IEnumerator resetPlayer(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        ship.SetActive(true);
       // transform.position = startPos;
        play = true;
        ps.Play();
    }

    // Explode the ship 
    void explode()
    {
        Debug.Log(" Ship destroyed");
        ship.SetActive(false);
        shipClone = Instantiate(brokenShip.transform, transform.position, transform.rotation) as Transform;
        
        ps.Stop();
        audio.clip = sfx_explosion;
        audio.Play();
        play = false;
      //  StartCoroutine(gameOver());
        StartCoroutine("restartGame");
    }

    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(10);
        gameController.SendMessage("showMenu");
    }


    // Add Score
    void addScore(int scoreMultiplier)
    {
        score += 100 * scoreMultiplier;
        t_Score.text = score.ToString();
    }

    void collisionCheck()
    {
        // Raycasting for Altitude
        Vector2 pos = transform.position;
        pos.y -= 0.2f;
        RaycastHit2D hitObj = Physics2D.Raycast(pos, -Vector2.up); // raycasting downward
      
        if (hitObj.collider != null)    // If collision
        {
            altitude = Mathf.Floor(Mathf.Abs(hitObj.point.y - transform.position.y) * 100) - 20.0f;  // Find the distance   
        }

        // Update UI
       txt_altitude.text = altitude.ToString();
       txt_hForce.text = Mathf.Abs(Mathf.Floor(velocity.x * 100)).ToString();
       txt_vForce.text = Mathf.Abs(Mathf.Floor(velocity.y * 100)).ToString();

       if (Mathf.Floor(velocity.y * 100) < safeVForce )
       {
           txt_vForce.color = Color.red;
       }
       else
       {
           txt_vForce.color = Color.white;
       }

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

        // add thrust to the player
        if (Input.GetKey(KeyCode.UpArrow))
        {
            force += 0.2f;
            if (force > 0.5f)
            {
                force = 0.5f;
            }
            if (!audio.isPlaying)
            {
                audio.clip = sfx_thruster;
                audio.Play();
            }
        }
        else
        {
            if (audio.isPlaying && audio.clip.name == "sfx_thruster")
            {
                audio.Stop();
            }
            force -= 0.1f;
            if (force < 0)
                force = 0;
        }

        // remove acceleration
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            force -= 0.1f;
            if (force < 0)
                force = 0;
        }

        if ( force == 0)
        {

            ps.startSize = 0;
        }
        else
        {
            ps.startSize = 0.1f;
        }
        
        velocity = rb.velocity;

        if ( fuel == 0)
        {
            force = 0;
            ps.Stop();
        }

        ps.startSpeed = force + 5.0f;
        // update the acting forces
        rb.AddForce(_transform.up * force);
        velocity = rb.velocity;
        if (rb.velocity.y > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, 1, 0);
        }

        // Fuel
        if (t_fuel != null)
        {
            t_fuel.text = fuel.ToString("F0");
        }

        if (fuel < 100)
        {
            t_fuel.color = Color.red;
        }

        // decrease the fuel. If it reaches 0 cant control anymore
        fuel -= 0.1f + (force * 0.1f);
        if (fuel < 0)
            fuel = 0;

       
    }



    void playGame()
    {

        play = true;
        this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        fuel = 1000;
        score = 0;
        time = 0;
        StartCoroutine(resetPlayer(0));
        ship.SetActive(true);

    }


    public IEnumerator nextlevel()
    {
        rb.isKinematic = true;
        // resets vars for next level
        yield return new WaitForSeconds(2.0f);
        transform.position = new Vector3(3.68f, 6.65f, 0);
        CameraManager.SINGLETON.reset();
        ps.Play();
        play = true;
        altitude = 0; // setting altitude to zero to fix a bug in the camera
        GameProperties.SINGLETON.reset();
        rb.isKinematic = false;

    }

    public IEnumerator restartGame()
    {
        rb.isKinematic = true;
        // resets everything
        altitude = 0; // setting altitude to zero to fix a bug in the camera
        yield return new WaitForSeconds(2.0f);
        Destroy(shipClone.gameObject);
        ship.SetActive(true);
        transform.position = new Vector3(3.68f, 6.65f, 0);
        CameraManager.SINGLETON.reset();
        ps.Play();
        play = true;
        fuel = 850;
        playGame();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
    }

   


}
