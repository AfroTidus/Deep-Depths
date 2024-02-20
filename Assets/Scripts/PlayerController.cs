using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public float speed = 1.0f;
    public float turnSpeed = 1.0f;
    private Rigidbody2D _rigidbody;
    private bool launching;
    private float turnDirection;
    string curScene;
    string nextScene;
    bool fire1Collected;
    bool fire2Collected;
    bool fire3Collected;
    public Light2D EndLight;
    public Light2D Surroundlight;
    public Light2D Conelight;
    public GameObject contactOne;
    public GameObject contactTwo;
    public GameObject contactThree;
    public GameObject contactFinal;
    public AudioSource hitSound;
    public AudioSource ominousSound;
    public AudioSource goodSound;
    public AudioSource badSound;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        curScene = SceneManager.GetActiveScene().name;
        if (curScene == "Menu")
            nextScene = "MainLevel";
        else if (curScene == "MainLevel")
            nextScene = "Finish";
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        fire1Collected = false;
        fire2Collected = false;
        fire3Collected = false;
    }

    void Update()
    {
        launching = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        } else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        } else {
        
            turnDirection = 0.0f;
        }
    }

    private void FixedUpdate()
    {
        if (launching)
        {
            _rigidbody.AddForce(this.transform.up * this.speed);
        }

        if (turnDirection != 0.0f)
        {
            _rigidbody.AddTorque(turnDirection * this.turnSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Wall")
        {
            //print("hit");
            takeDamage(20);
            hitSound.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Flame")
        {
            Destroy(coll.gameObject);
            Destroy(contactThree); contactThree = null;
            fire1Collected = true;
            ominousSound.Play();
        }
        if (coll.gameObject.tag == "BFlame")
        {
            Destroy(coll.gameObject);
            Destroy(contactOne); contactOne = null;
            fire2Collected = true;
            Surroundlight.pointLightOuterRadius = 3.0f;
            ominousSound.Play();
        }
        if (coll.gameObject.tag == "GFlame")
        {
            Destroy(coll.gameObject);
            Destroy(contactTwo); contactTwo = null;
            fire3Collected = true;
            maxHealth += 40;
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            ominousSound.Play();
        }
        if (coll.gameObject.tag == "Finish")
        {
            EndLight.intensity = 2.0f;
            ominousSound.Play();
            if (fire1Collected && fire2Collected && fire3Collected)
            {
                StartCoroutine(GoodEnd());
            }
            else
            {
                StartCoroutine(BadEnd());
            }
        }
    }

    void takeDamage (int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(DoDeath());
        }
    }

    IEnumerator DoDeath()
    {
        // reload the level in 2 seconds
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(curScene);
    }
    IEnumerator GoodEnd()
    {
        yield return new WaitForSeconds(1.5f);
        goodSound.Play();
        yield return new WaitForSeconds(2);
        EndLight.intensity = 0.0f;
        Conelight.intensity = 0.0f;
        Surroundlight.intensity = 0.0f;
        yield return new WaitForSeconds(0.5f);
        Destroy(contactFinal); contactFinal = null;
        yield return new WaitForSeconds(2);
        EndLight.intensity = 2.0f;
        Conelight.intensity = 1;
        Surroundlight.intensity = 1;
        SceneManager.LoadScene(nextScene);
    }
    IEnumerator BadEnd()
    {
        yield return new WaitForSeconds(1.5f);
        badSound.Play();
        yield return new WaitForSeconds(0.75f);
        SceneManager.LoadScene(curScene);
    }
}
