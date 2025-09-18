using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargePad : MonoBehaviour
{
    public Transform playerTransform;
    public float rateOfChargeInWatts = 100.0f;
    private bool m_wasCharging = false;
    
    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) < transform.localScale.x/2.0f)
        {
            m_wasCharging = true;
            Battery battery = playerTransform.GetComponent<Battery>();
            battery.charging = true;
            battery.ChargeBattery(rateOfChargeInWatts * Time.deltaTime);
        }
        else
        {
            if (m_wasCharging)
            {
                m_wasCharging = false;
                Battery battery = playerTransform.GetComponent<Battery>();
                battery.charging = false;
            }
        }
    }
}