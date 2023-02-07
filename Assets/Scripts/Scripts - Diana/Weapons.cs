using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Weapons : MonoBehaviour
{
    public Sprite[] weaponSprites;
    public Image weaponImage;

    private string[] weapons = { "Hand", "Hoe", "Seeds Pack", "Scythe" };
    [SerializeField] public TextMeshProUGUI currentWeapon;
    public int weaponIndex;

    private void Start()
    {
        currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];
    }

    void Update()
    {
        float Scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Scroll > 0 && weaponIndex < weapons.Length - 1) // forward
        {
            weaponIndex += 1;
            currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];
            weaponImage.sprite = weaponSprites[weaponIndex];


        }
        else if (Scroll < 0 && weaponIndex > 0) // backwards
        {
            weaponIndex -= 1;
            currentWeapon.text = "Current Weapon: " + weapons[weaponIndex];
            weaponImage.sprite = weaponSprites[weaponIndex];
        }
    }
}
