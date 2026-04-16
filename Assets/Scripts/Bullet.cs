using UnityEngine;



public class Bullet : MonoBehaviour
{

    public Vector3 target;
    public int speed;
    public float time;
    private float timePassed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        timePassed += Time.deltaTime;
        if (timePassed > time && time != 0)
        {
            Destroy(gameObject);
        }
    }
}
