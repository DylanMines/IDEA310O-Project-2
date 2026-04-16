using UnityEngine;

public class Door : MonoBehaviour
{
    public bool unlocked;
    public Animation anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag == "Player" && unlocked)
        {
            anim.Play();
            // wait, then load next scene
        }
    }
}
