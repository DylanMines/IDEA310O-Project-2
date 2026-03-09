using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int speed;
    public int sensitivity;

    private Vector3 momentum;

    private PlayerInput _input;
    void Start()
    {
        _input = GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.moving == true)
        {
            Debug.Log("moving");
            Vector3 moveDirection = transform.forward * -(speed / 50);
            momentum += moveDirection * Time.deltaTime;
        }

        transform.position += momentum * Time.deltaTime;
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        Vector3 rotateVector = new Vector3(-_input.look.y, _input.look.x, 0);
        transform.Rotate(rotateVector * Time.deltaTime * sensitivity);
            
    }
}
