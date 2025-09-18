using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperPupSystems.StateMachine;

public class VacuumStateMachine : SimpleStateMachine
{
    public DirtManager dirtManager;

    public VacuumRandomState vacuumRandomState;
    //public SomeState someState;

    void Awake()
    {
        // add all states first
        states.Add(vacuumRandomState);
        //states.Add(someState);
    }

    void Start()
    {
        // start the first state
        ChangeState("vacuumRandomState");
    }
}
