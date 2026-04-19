using System;
using UnityEngine;

enum State
{
    IDLE,
    CHASE,
    EXPLODE
}

public class EnemyController : MonoBehaviour
{
    private Vector3 movement;

    [SerializeField] private ParticleSystem up;
    [SerializeField] private ParticleSystem down;
    [SerializeField] private ParticleSystem forward;
    [SerializeField] private ParticleSystem back;
    [SerializeField] private ParticleSystem left;
    [SerializeField] private ParticleSystem right;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float explodeDistance;
    private Boolean exploded;

    [SerializeField] private GameObject explosion;
    [SerializeField] private float maxHealth;
    [SerializeField] private AudioClip explosionSound;
    public float health;

    [SerializeField] private float turnOffAxis;

    private Material material;

    private State currentState = State.IDLE;

    public float speed;

    [SerializeField] private MeshRenderer lights;
    private float hitFlashDuration = 1.0f;
    private float hitFlashDelta = 1.0f;
    private bool hitFlashDirection;

    

    private GameObject Player;

    [SerializeField] private AudioSource slowBeep;
    [SerializeField] private AudioSource fastBeep;
    [SerializeField] private AudioSource thrust;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movement = new Vector3();
        Player = GameObject.FindGameObjectWithTag("Player");
        ToggleThrusters();
        lights.materials[1] = new Material(lights.materials[1]);
        material = lights.materials[1];
        material.color = Color.red;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        movement = Player.transform.position - transform.position;
        float dist = movement.magnitude;
        if (exploded)
        {
            currentState = State.IDLE;
        }
        else if (dist < explodeDistance || health < 5)
        {
            currentState = State.EXPLODE;
        }
        else if (dist < chaseDistance)
        {
            currentState = State.CHASE;
        }
        movement.Normalize();
       

        if (currentState == State.CHASE)
        {
            transform.position += movement * (speed / 50f) * Time.deltaTime;
            if (!slowBeep.isPlaying)
            {
                slowBeep.Play();
            }
            if (!thrust.isPlaying)
            {
                thrust.Play();
            }
            ToggleThrusters();
        }

        if (currentState == State.EXPLODE)
        {
            if (!exploded)
            {
                exploded = true;
                health = 0;
                slowBeep.Stop();
                if (!fastBeep.isPlaying)
                {
                    fastBeep.Play();
                }
                Invoke("Explode", 2.0f);
            }
        }

        hitFlashDuration = (health + 5) / maxHealth * 3;

        if (hitFlashDirection == true)
        {
            hitFlashDelta += Time.deltaTime;
        }
        else
        {
            hitFlashDelta -= Time.deltaTime;
        }

        if (hitFlashDelta > hitFlashDuration)
        {
            hitFlashDelta = hitFlashDuration;
            hitFlashDirection = false;
        }
        else if (hitFlashDelta < 0)
        {
            hitFlashDirection = true;
        }
        float flashStrength = hitFlashDelta / hitFlashDuration % 1;
        material.SetVector("_EmissionColor", Color.Lerp(Color.yellow, Color.black, flashStrength));
    }

    void ToggleThrustGroup(float direction, ParticleSystem p1, ParticleSystem p2)
    {
        var emission1 = p1.emission;
        var emission2 = p2.emission;
        emission1.enabled = direction > 0.1;
        emission2.enabled = direction < -0.1;
    }
    void ToggleThrusters()
    {
        ToggleThrustGroup(movement.y, down, up);
        ToggleThrustGroup(movement.x, left, right);
        ToggleThrustGroup(movement.z, back, forward);
    }

    void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        thrust.PlayOneShot(explosionSound);
        Collider[] colliders;
        colliders = Physics.OverlapSphere(this.transform.position, 17);
        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                c.gameObject.SendMessage("Kill");
                break;
            }
        }
        Destroy(gameObject);
    }
}
