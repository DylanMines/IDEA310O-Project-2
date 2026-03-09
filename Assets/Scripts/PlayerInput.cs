using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 look;
    public bool moving;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnLook(InputValue value)
    {
        look = value.Get<Vector2>();
    }
    public void OnMove(InputValue value)
    {
        moving = value.isPressed;
    }
}
