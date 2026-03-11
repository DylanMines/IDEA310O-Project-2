using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int speed;
    public int sensitivity;

    [SerializeField]
    private ParticleSystem effects;

    private Vector3 momentum;

    private InputAction lookAction;
    InputAction moveAction;
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        var emission = effects.emission;

        if (moveAction.IsPressed() == true)
        {
            emission.rateOverTime = 30f;
            //Debug.Log("moving");
            Vector3 moveDirection = transform.forward * -(speed / 50);
            momentum += moveDirection * Time.deltaTime;
        }
        else
        {
            emission.rateOverTime = 0f;
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
        Vector3 rotateVector = new Vector3(-lookValue.y, lookValue.x, 0);
        transform.Rotate(rotateVector * Time.deltaTime * sensitivity);
            
    }
}
