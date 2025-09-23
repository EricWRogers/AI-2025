using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// this is required for SimpleState
using SuperPupSystems.StateMachine;

[System.Serializable]
public class VacuumChargeState : SimpleState // change monobehavior to SimpleState
{
    [Range(0.0f, 1.0f)]
    public float targetCharge = 0.8f;
    private VacuumStateMachine m_vacuumStateMachine;
    private DirtManager m_dirtManager;
    private MotorController m_motorController;
    private Bumper m_bumper;
    private Battery m_battery;
    public override void OnStart()
    {
        m_vacuumStateMachine = (VacuumStateMachine)stateMachine;
        m_dirtManager = m_vacuumStateMachine.dirtManager;
        m_motorController = stateMachine.GetComponent<MotorController>();
        m_bumper = stateMachine.GetComponent<Bumper>();
        m_battery = stateMachine.GetComponent<Battery>();
    }

    public override void UpdateState(float _dt)
    {
        if (m_battery.powerLevel >= targetCharge)
            stateMachine.ChangeState("VacuumRandomState");
    }

    public override void OnExit()
    {
        
    }
}
