using UnityEngine;
using UnityEngine.Events;

public class Bumper : MonoBehaviour
{
    public Transform bumperTransform;
    public bool hittingObject = false;
    public UnityEvent hit;
    public LayerMask mask;

    void FixedUpdate()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            bumperTransform.position,
            bumperTransform.localScale.x/2.0f,
            transform.right,
            0.0f);

        if (hits.Length > 0 && hittingObject == false)
        {
            hit.Invoke();
        }

        hittingObject = (hits.Length > 0);
    }
}
