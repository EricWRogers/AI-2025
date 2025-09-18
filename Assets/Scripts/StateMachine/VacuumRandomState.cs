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
    public override void OnStart()
    {
        m_vacuumStateMachine = (VacuumStateMachine)stateMachine;
        m_dirtManager = m_vacuumStateMachine.dirtManager;
        m_motorController = stateMachine.GetComponent<MotorController>();
        m_bumper = stateMachine.GetComponent<Bumper>();

        // connect unity event in code
        m_bumper.hit.AddListener(Bump);
    }

    public override void UpdateState(float _dt)
    {
        if (m_motorController.turning == false)
            m_motorController.Forward();

        List<GameObject> dirtPile = m_dirtManager.FindDirtInCircle(stateMachine.transform.position, stateMachine.transform.localScale.x / 2.0f);

        m_dirtManager.RemoveDirt(dirtPile);
    }

    public override void OnExit()
    {
        // disconnect unity event in code
        m_bumper.hit.RemoveListener(Bump);
    }

    public void Bump()
    {
        m_motorController.Turn(Random.Range(85.0f, 95.0f));
    }
}
