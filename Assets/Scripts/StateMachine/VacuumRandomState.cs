using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this is required for SimpleState
using SuperPupSystems.StateMachine;

[System.Serializable]
public class VacuumRandomState : SimpleState // change monobehavior to SimpleState
{
    private VacuumStateMachine m_vacuumStateMachine;
    private DirtManager m_dirtManager;
    private MotorController m_motorController;
    private Bumper m_bumper;
    private Battery m_battery;

    private DistanceSensor m_sensorN90;
    private DistanceSensor m_sensorN45;
    private DistanceSensor m_sensor0;
    private DistanceSensor m_sensor45;
    private DistanceSensor m_sensor90;

    [Range(0.0f, 3.0f)]
    public float weightSensorN90 = 1.0f;
    [Range(0.0f, 3.0f)]
    public float weightSensorN45 = 1.0f;
    [Range(0.0f, 3.0f)]
    public float weightSensor0 = 1.0f;
    [Range(0.0f, 3.0f)]
    public float weightSensor45 = 1.0f;
    [Range(0.0f, 3.0f)]
    public float weightSensor90 = 1.0f;

    public float sn45to0 = 0.0f;
    public float s0to45 = 0.0f;

    public override void OnStart()
    {
        m_vacuumStateMachine = (VacuumStateMachine)stateMachine;
        m_dirtManager = m_vacuumStateMachine.dirtManager;
        m_motorController = stateMachine.GetComponent<MotorController>();
        m_bumper = stateMachine.GetComponent<Bumper>();
        m_battery = stateMachine.GetComponent<Battery>();

        m_sensorN90 = stateMachine.transform.Find("Distance Sensor -90").GetComponent<DistanceSensor>();
        m_sensorN45 = stateMachine.transform.Find("Distance Sensor -45").GetComponent<DistanceSensor>();
        m_sensor0 = stateMachine.transform.Find("Distance Sensor").GetComponent<DistanceSensor>();
        m_sensor45 = stateMachine.transform.Find("Distance Sensor 45").GetComponent<DistanceSensor>();
        m_sensor90 = stateMachine.transform.Find("Distance Sensor 90").GetComponent<DistanceSensor>();

        // connect unity event in code
        m_bumper.hit.AddListener(Bump);

        if (m_motorController.distanceTraveled < 1.0f)
            m_motorController.Turn(90.0f);
    }

    public override void UpdateState(float _dt)
    {
        if (m_motorController.turning == false)
            m_motorController.Forward();

        List<GameObject> dirtPile = m_dirtManager.FindDirtInCircle(stateMachine.transform.position, stateMachine.transform.localScale.x / 2.0f);

        m_dirtManager.RemoveDirt(dirtPile);


        if (m_sensor0.distance <= 0.25f)
        {
            m_motorController.Turn(CalculateTurn());
        }
        
        Vector2 endOfN45 = m_sensorN45.transform.position + (m_sensorN45.transform.right * m_sensor0.distance);
        Vector2 endOf0 = m_sensor0.transform.position + (m_sensor0.transform.right * m_sensor0.distance);
        Vector2 endOf45 = m_sensor45.transform.position + (m_sensor45.transform.right * m_sensor0.distance);

        sn45to0 = Mathf.Asin(Vector2.Distance(endOfN45, endOf0)/m_sensor0.distance);

        if (m_battery.lowPower && m_battery.charging)
            stateMachine.ChangeState("VacuumChargeState");
    }

    public override void OnExit()
    {
        // disconnect unity event in code
        m_bumper.hit.RemoveListener(Bump);
    }

    public float CalculateTurn()
    {
        float turn = 0.0f;
        turn += m_sensorN90.distance * -45.0f * weightSensorN90;// * m_sensor0.distance;
        turn += m_sensorN45.distance * -45.0f * weightSensorN45;
        turn += m_sensor0.distance * 0 * weightSensor0;
        turn += m_sensor45.distance * 45.0f * weightSensor45;
        turn += m_sensor90.distance * 45.0f * weightSensor90;// * m_sensor0.distance;

        turn = Mathf.Clamp(turn, -90.0f, 90.0f);

        if (Mathf.Abs(turn) < 3.0f)
        {
            float[] d = {-90.0f, 90.0f};
            turn = d[Random.Range(0, 2)];
        }

        return turn;
    }

    public void Bump()
    {
        

        m_motorController.Turn(CalculateTurn());
    }
}
