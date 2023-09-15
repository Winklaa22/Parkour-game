using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponsManager : SceneSingleton<WeaponsManager>
{
    [SerializeField] private Weapon[] m_weaponsControllers;
    [SerializeField] private WeaponType[] m_slots;
    private bool _cantChangeSlot;
    [SerializeField] private int _currentSlot;

    public Weapon GetCurrentWeapon()
    {
        return m_weaponsControllers.First(x=>x.Type == m_slots[_currentSlot]);
    }

    public Weapon GetGunFromSlot(int slot)
    {
        return m_weaponsControllers.First(x=>x.Type == m_slots[slot]);

    }

    private int GetNextSlotIndex()
    {
        return _currentSlot + 1 > m_slots.Length - 1 ? 0 : _currentSlot + 1;
    }

    private int GetPreviousSlotIndex()
    {
        return _currentSlot - 1 < 0 ? m_slots.Length - 1 : _currentSlot - 1;
    }

    public void TryToNextWeapon()
    {
        if(m_slots[GetNextSlotIndex()] is  WeaponType.EMPTY || _cantChangeSlot)
            return;


        StartCoroutine(ChangeWeapon(GetNextSlotIndex()));
    }

    public void TryToPreviousWeapon()
    {
        if(m_slots[GetPreviousSlotIndex()] is  WeaponType.EMPTY || _cantChangeSlot)
            return;
        
        
        StartCoroutine(ChangeWeapon(GetPreviousSlotIndex()));

    }

    private IEnumerator ChangeWeapon(int newSlot)
    {
        var currentWeapon = GetCurrentWeapon();
        var newWeapon = GetGunFromSlot(newSlot);
        
        currentWeapon.Hide();
        
        _cantChangeSlot = true;
        yield return new WaitForSeconds(currentWeapon.TimeToHide);
        _cantChangeSlot = false;
        _currentSlot = newSlot;
        newWeapon.Show();
    }
}
