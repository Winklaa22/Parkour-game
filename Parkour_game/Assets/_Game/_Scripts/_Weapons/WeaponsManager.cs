using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsManager : SceneSingleton<WeaponsManager>
{
    [SerializeField] private Weapon[] m_weaponsControllers;
    [SerializeField] private WeaponType[] m_slots;
    private int _currentSlot;

    public Weapon GetCurrentWeapon()
    {
        return m_weaponsControllers.First(x=>x.Type == m_slots[_currentSlot]);
    }
}
