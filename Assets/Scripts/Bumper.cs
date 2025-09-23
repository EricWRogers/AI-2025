using UnityEngine;
using UnityEngine.Events;

public class Bumper : MonoBehaviour
{
    public Transform bumperTransform;
    public bool hittingObject = false;
    public UnityEvent hit;
    public UnityEvent autoReverse;
    public LayerMask mask;
    private MotorController m_motor;

    void Start()
    {
        m_motor = GetComponent<MotorController>();
    }

    void FixedUpdate()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            bumperTransform.position,
            bumperTransform.localScale.x/2.0f,
            transform.right,
            0.0f);

        if (hits.Length > 0 && (hittingObject == false))
        {
            autoReverse.Invoke();

            if (m_motor.moving)
            {
                hit.Invoke();
            }
        }

        hittingObject = (hits.Length > 0);
    }
}
