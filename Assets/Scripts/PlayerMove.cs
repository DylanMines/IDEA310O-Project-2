using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int buttonsPressed;
    public int maxButtons;
    public int speed;
    public int dampening;
    public int sensitivity;
    [SerializeField] private GameObject bullet;

    [SerializeField]
    private ParticleSystem effects;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image hotbar1;
    [SerializeField] private Image hotbar2;
    [SerializeField] private TMP_Text score;

    [SerializeField] private MeshRenderer hotbar1Item;
    [SerializeField] private MeshRenderer hotbar2Item;
    [SerializeField] private GameObject gun;
    private Vector3 momentum;

    [SerializeField] private GameObject door;

    private InputAction lookAction;
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
        rollLeft = InputSystem.actions.FindAction("RollLeft");
        rollRight = InputSystem.actions.FindAction("RollRight");
        hotbar1Action = InputSystem.actions.FindAction("Hotbar1");
        hotbar2Action = InputSystem.actions.FindAction("Hotbar2");
        SelectHotbarButton(1);
    }

    void Shoot()
    {
        RaycastHit hit;
        
        GameObject newBullet =  Instantiate(bullet, gun.transform.position, transform.rotation);
        Bullet b = newBullet.GetComponent<Bullet>();
        
        b.time = 1;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 4000.0f))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                Debug.Log("Damaging Enemy");
                hit.collider.gameObject.GetComponent<EnemyController>().health -= 18;
            }
        }
        b.target = transform.forward * 1000;
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
        momentum += moveDirection * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        var emission = effects.emission;

        if (moveAction.IsPressed() == true)
        {
            if (extinguisherEnabled == true)
            {
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
            SelectHotbarButton(1);
        }
        else if (hotbar2Action.IsPressed() == true)
        {
            SelectHotbarButton(2);
        }

        
        if (maxButtons == buttonsPressed)
        {
            door.GetComponent<Door>().unlocked = true;
            score.text = "ALL BUTTONS ACTIVATED\nGET TO THE DOOR";
        }
        else
        {
            score.text = "Buttons Pressed: "+buttonsPressed+"/"+maxButtons;
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
        /*yrot += -lookValue.y;
        xrot += lookValue.x;
        zrot += rollValue;

        transform.rotation = Quaternion.Euler(new Vector3(yrot, xrot, zrot) * sensitivity/50);*/

    }

    private void SelectHotbarButton(short button)
    {
        // Vector4 selectedColor = new Vector4(95 / 255, 180 / 255, 255 / 255, 1);
        if (button == 1)
        {
            hotbar1.color = Color.cyan;
            hotbar2.color = Color.white;
            hotbar1Item.enabled = true;
            hotbar2Item.enabled = false;
            extinguisherEnabled = true;
        }
        else
        {
            hotbar1.color = Color.white;
            hotbar2.color = Color.cyan;
            hotbar1Item.enabled = false;
            hotbar2Item.enabled = true;
            extinguisherEnabled = false;

        }
    }
}
