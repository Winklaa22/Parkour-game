using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollPart : MonoBehaviour
{
    private RagdollController m_controller;

    public RagdollController Controller
    {
        set
        {
            m_controller = value;
        }
    }

    public void Active()
    {
        m_controller.Active();
    }
}
