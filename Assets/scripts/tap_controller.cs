using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class tap_controller : MonoBehaviour
{
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 10;
    public float tiltSmooth = 5;

    public AudioSource tapsound;
    public AudioSource scoresound;
    public AudioSource diesound;

    Rigidbody2D body;
    Quaternion downRotation;
    Quaternion forwardRotation;

    game_manager game;
    
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = game_manager.Instance;
        body.simulated = false;
         
    }
    void OnEnable()
    {
        game_manager.OnGameStarted += OnGameStarted;
        game_manager.OnGameOverConfirmed += OnGameOverConfirmed;
        
    }

    void OnDisable()
    {
        game_manager.OnGameStarted -= OnGameStarted;
        game_manager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        body.velocity = Vector3.zero;
        body.simulated = true;
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition =new Vector3(1.29f, 1.48f, 0f);
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        if (game.GameOver) return;
        if (Input.GetMouseButtonDown(0))
        {
            tapsound.Play();
            transform.rotation = forwardRotation;
            body.velocity = Vector3.zero;
            body.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "score_zone")
        {
            //register a score event
            OnPlayerScored();//event sent to GameManager
            //play a sound
            scoresound.Play();

        }
        if (collision.gameObject.tag == "dead_zone")
        {
            body.simulated = false;
            //register a dead event
            OnPlayerDied();//event sent to GameManager
                           //play a sound
            diesound.Play();
        }
    }
}
