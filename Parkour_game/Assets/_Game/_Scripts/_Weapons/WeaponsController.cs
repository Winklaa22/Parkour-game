using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsController : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (InputManager.Instance.ScrollAxis > 0)
        {
            WeaponsManager.Instance.TryToNextWeapon();
        }
        else if (InputManager.Instance.ScrollAxis < 0)
        {
            WeaponsManager.Instance.TryToPreviousWeapon();
        }
    }
}
