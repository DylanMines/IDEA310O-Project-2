using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int speed;
    public int dampening;
    public int sensitivity;

    [SerializeField]
    private ParticleSystem effects;

    private Vector3 momentum;

    private InputAction lookAction;
    InputAction moveAction;
    InputAction rollLeft;
    InputAction rollRight;

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
    }

    // Update is called once per frame
    void Update()
    {
        var emission = effects.emission;

        if (moveAction.IsPressed() == true)
        {
            emission.rateOverTime = 70f;
            //Debug.Log("moving");
            Vector3 moveDirection = transform.forward * -(speed / 50f);
            momentum += moveDirection * Time.deltaTime;
        }
        else
        {
            emission.rateOverTime = 0f;
            momentum -= momentum * (dampening / 50f) * Time.deltaTime;
            // effects.Pause();
        }

        transform.position += momentum * Time.deltaTime;
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
}
