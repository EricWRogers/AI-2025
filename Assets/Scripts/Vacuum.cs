using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vacuum : MonoBehaviour
{
    public DirtManager dirtManager;
    public float speed = 30.0f;

    // FixedUpdate its goal is to be called 52 times a second
    void FixedUpdate()
    {
        if (dirtManager.timeLeft <= 0.0f)
            return;
            
        Vector3 direction = Vector3.zero;

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        direction = direction.normalized;

        transform.position += direction * speed * Time.fixedDeltaTime;

        List<GameObject> dirtCollected = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x / 2.0f);
        foreach (GameObject dirt in dirtCollected)
            Destroy(dirt);
    }

    public void Bump()
    {
        Debug.Log("Bump");
    }
}
