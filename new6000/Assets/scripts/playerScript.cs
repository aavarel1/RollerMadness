using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class playerscript : MonoBehaviour
{
    Rigidbody rb;
    InputSystem_Actions inputActions;
    public GameObject explosion;
    public GameObject coinExplosion;
    Vector3 movement = Vector3.zero;

    public float moveSpeed = 10f;

    public TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.tag == "coin1")
        {
            Destroy(collision.gameObject);
            GameManager.gm.add_score(1);
            Instantiate(coinExplosion, collision.transform.position, Quaternion.identity);
            GetComponent<AudioSource>().Play();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            GameManager.gm.decHealth();
        } 

        
    }

    void fixedUpdate()
    {
        rb.linearVelocity = movement; 

    }

    void OnMove(InputValue value)
    {
        if (GameManager.gm.playerHealth == 0) {
            moveSpeed = 0f; 
            return;
        } 

        if (GameManager.gm.score >= GameManager.gm.beatLevelScore)
        {
            moveSpeed = 0f;
        }

        Vector2 b = value.Get<Vector2>();
        movement = new Vector3(b.x, 0, b.y) * moveSpeed;
        rb.linearVelocity = movement;

        if (GameManager.gm.pauseGame)
        {
            movement = Vector3.zero;
            return;
        }
        
    }

    void OnPause(InputValue value)
    {
        if (value.Get<float>() > 0) // This will check for the key being pressed
        {
            Debug.Log("Pause toggled!");
            GameManager.gm.pauseGame = !GameManager.gm.pauseGame;
            GameManager.gm.pause();
        }
    }
}
