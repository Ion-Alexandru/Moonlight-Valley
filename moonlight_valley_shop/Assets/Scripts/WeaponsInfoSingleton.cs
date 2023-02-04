using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInfoSingleton : MonoBehaviour
{
    public static WeaponsInfoSingleton instance;
    private int money;
    private int quantity;

    private void Awake()
    {
        // Ensure that the script persists across multiple scenes
        DontDestroyOnLoad(this.gameObject);

        // Check if there's already an instance of the script
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
}
