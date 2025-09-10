using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumCPU : MonoBehaviour
{
    public DirtManager dirtManager;
    private MotorController motorController;
    // Start is called before the first frame update
    void Start()
    {
        // get a reference to the motor controller
        motorController = GetComponent<MotorController>();
    }

    void FixedUpdate()
    {
        // stop agent's movement when time left is 0
        if (dirtManager.timeLeft <= 0.0f)
            return;

        // stop agent's movement while turning
        if (motorController.turning)
            return;

        // You can change code starting here
        

        // You can not change code below here

        // pickup dirt
        List<GameObject> dirtPile = dirtManager.FindDirtInCircle(transform.position, transform.localScale.x/2.0f);
        dirtManager.RemoveDirt(dirtPile);
    }

    public void Bump()
    {
        // called when the bumper is pressed
        // You can change code starting here
        // You can not change code below here
    }
}
