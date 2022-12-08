using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_SwitchDoor : Object_Door
{
    [SerializeField] private Object_Switch[] m_Switches;
    private int m_ActiveSwitches = 0;

    protected override void Awake()
    {
        base.Awake();

        foreach(Object_Switch switchObj in m_Switches)
        {
            switchObj.OnStateChange += UpdateDoorState;
        }
    }

    private void OnDestroy()
    {
        foreach (Object_Switch switchObj in m_Switches)
        {
            switchObj.OnStateChange -= UpdateDoorState;
        }
    }

    private void UpdateDoorState(bool state)
    {
        if (state)
            m_ActiveSwitches++;
        else
            m_ActiveSwitches--;

        if(m_ActiveSwitches >= m_Switches.Length)
        {
            ChangeState(false);
        }
        else
        {
            ChangeState(true);
        }
    }
}
