using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    private int maxButtons;
    private int buttonsPressed;
    private bool dead;
    
    [Header("Settings")]    
    public int speed;
    public int dampening;
    public int sensitivity;

    [Header("References")]
    [SerializeField] private GameObject bullet;

    [SerializeField] private ParticleSystem effects;
    [SerializeField] private Canvas canvas;
    [SerializeField] private MeshRenderer hotbar1Item;
    [SerializeField] private MeshRenderer hotbar2Item;
    [SerializeField] private GameObject gun;
    private Vector3 momentum;
    [SerializeField] private Door door;

    [SerializeField] private UI UIScript;
    [SerializeField] private AudioSource extinguisherSound;
    [SerializeField] private AudioSource laserSound;
    [SerializeField] private GameObject DoorEnemies;
    [SerializeField] private Rigidbody body;

    private InputAction lookAction;
    InputAction menuAction;
    InputAction moveAction;
    InputAction rollLeft;
    InputAction rollRight;
    InputAction hotbar1Action;
    InputAction hotbar2Action;
    private bool extinguisherEnabled = true;
    private bool isShooting = false;
    private float xrot;
    private float yrot;
    private float zrot;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        menuAction = InputSystem.actions.FindAction("OpenMenu");
        maxButtons = GameObject.FindGameObjectsWithTag("Button").Length;
        rollLeft = InputSystem.actions.FindAction("RollLeft");
        rollRight = InputSystem.actions.FindAction("RollRight");
        hotbar1Action = InputSystem.actions.FindAction("Hotbar1");
        hotbar2Action = InputSystem.actions.FindAction("Hotbar2");
        SelectHotbar(1);
    }

    void Shoot()
    {
        laserSound.PlayOneShot(laserSound.clip);
        RaycastHit hit;
        
        GameObject newBullet =  Instantiate(bullet, gun.transform.position, transform.rotation);
        Bullet b = newBullet.GetComponent<Bullet>();
        
        b.time = 1;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4000.0f))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                hit.collider.gameObject.GetComponent<EnemyController>().health -= 18;
            }
        }
        b.target = transform.forward * 100000;
    }

    void Drag(ParticleSystem.EmissionModule emission)
    {
        emission.rateOverTime = 0f;
        momentum -= momentum * (dampening / 50f) * Time.deltaTime;
    }

    void Move(ParticleSystem.EmissionModule emission)
    {
        emission.rateOverTime = 70f;
        Vector3 moveDirection = transform.forward * -(speed / 50f);
        body.AddForce(moveDirection);
    }

    // Update is called once per frame
    void Update()
    {
        var emission = effects.emission;

        if (menuAction.WasPressedThisFrame() == true)
        {
            if (UIScript.menuOpen)
            {
                UIScript.SendMessage("HideMenu");
            }
            else
            {
                UIScript.SendMessage("ShowMenu");
            }
        }

        if (moveAction.IsPressed() == true)
        {
            if (extinguisherEnabled == true)
            {
                if (!extinguisherSound.isPlaying)
                {
                    extinguisherSound.Play();
                }
                Move(emission);
            }
            else if (isShooting == false)
            {
                isShooting = true;
                Shoot();
            }
        }
        else
        {
            extinguisherSound.Stop();
            Drag(emission);
            if (isShooting == true)
            {
                isShooting = false;
            }
        }
        Debug.DrawRay(transform.position, transform.forward * 1000, Color.white, 1.0f);
        transform.position += momentum * Time.deltaTime;

        if (hotbar1Action.IsPressed() == true)
        {
            SelectHotbar(1);
        }
        else if (hotbar2Action.IsPressed() == true)
        {
            SelectHotbar(2);
        } 
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        Vector2 lookValue = lookAction.ReadValue<Vector2>();
        int rollValue = 0;
        if (rollLeft.IsPressed()) { rollValue += 1; }
        if (rollRight.IsPressed()) { rollValue -= 1; }


        // method 1
        Vector3 rotateVector = new Vector3(-lookValue.y, lookValue.x, rollValue);
        transform.Rotate(rotateVector * Time.deltaTime * sensitivity);

        // method 2
        // yrot += -lookValue.y;
        // xrot += lookValue.x;
        // zrot += rollValue;

        // transform.rotation = Quaternion.Euler(new Vector3(yrot, xrot, zrot) * sensitivity/50);
    }

    void SelectHotbar(short option)
    {
        if (option == 1)
        {
            extinguisherEnabled = true;
            hotbar1Item.enabled = true;
            hotbar2Item.enabled = false;
        }
        else
        {
            extinguisherEnabled = false;
            hotbar1Item.enabled = false;
            hotbar2Item.enabled = true;
        }
        UIScript.BroadcastMessage("SelectHotbarButton", option);
    }

    void ButtonPressed()
    {
        if (dead) {return;}
        buttonsPressed += 1;
        UIScript.BroadcastMessage("UpdateScore", buttonsPressed);
        if (buttonsPressed == maxButtons)
        {
            GameObject.Find("/Music").SendMessage("BattleMusic");
            door.unlocked = true;
            DoorEnemies.SetActive(true);
        }
    }

    void Kill()
    {
        UIScript.BroadcastMessage("ShowDeath");
        Cursor.lockState = CursorLockMode.None;
    }
    
}
