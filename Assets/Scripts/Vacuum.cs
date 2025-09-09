using UnityEngine;

public class Vacuum : MonoBehaviour
{
    public DirtManager dirtManager;
    public float speed = 30.0f;

    // FixedUpdate its goal is to be called 52 times a second
    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        direction = direction.normalized;

        transform.position += direction * speed * Time.fixedDeltaTime;

        int c = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x/2.0f).Count;
        Debug.Log("Dirt Count: " + c);
    }

    public void Bump()
    {
        Debug.Log("Bump");
    }
}
