using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AssaultRifleController : Weapon
{
    public override void Show()
    {
        base.Show();
        InputManager.Instance.Actions.Weapon.Shot.started += OnShotInputStated;
    }

    private void OnShotInputStated(InputAction.CallbackContext obj)
    {
        Shot();
    }

    public override void Hide()
    {
        base.Hide();
        InputManager.Instance.Actions.Weapon.Shot.canceled += OnShotInputEnded;

    }

    private void OnShotInputEnded(InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
}
