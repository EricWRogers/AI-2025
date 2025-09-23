using UnityEngine;
using UnityEngine.UI;

public class DistanceSensor : MonoBehaviour
{
    public bool hit;
    public float distance;
    public float maxDistance = 1.0f;
    public SpriteRenderer graphics;

    void FixedUpdate()
    {
        Vector2 startPos = transform.position;
        Vector2 endPos = transform.position + transform.right * maxDistance;
        RaycastHit2D hit2D = Physics2D.Linecast(startPos, endPos);

        if (hit2D)
        {
            hit = true;
            graphics.color = Color.red;
            distance = Vector2.Distance(startPos, hit2D.point);
            transform.localScale = new Vector3(distance, 1.0f, 1.0f);
        }
        else
        {
            hit = false;
            graphics.color = Color.green;
            transform.localScale = new Vector3(maxDistance, 1.0f, 1.0f);
            distance = maxDistance;
        }
    }
}
