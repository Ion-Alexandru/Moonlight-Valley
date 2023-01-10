using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shop_Weapons : MonoBehaviour
{
    public int[,] shopItems = new int[6, 6];
    [SerializeField] public float coins;
    [SerializeField] public TextMeshProUGUI CoinTxt;

    void Start()
    {
        CoinTxt.text = "MoonCoins: " + coins.ToString();

        //Id's
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;

        //Price
        shopItems[2, 1] = 35;
        shopItems[2, 2] = 50;
        shopItems[2, 3] = 45;
        shopItems[2, 4] = 30;
        shopItems[2, 5] = 55;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonRef.GetComponent<Weapons_Info>().ItemID])
        {
            coins -= shopItems[2, ButtonRef.GetComponent<Weapons_Info>().ItemID];
            shopItems[3, ButtonRef.GetComponent<Weapons_Info>().ItemID]++;
            CoinTxt.text = "MoonCoins: " + coins.ToString();
            ButtonRef.GetComponent<Weapons_Info>().QuantityTxt.text = shopItems[3, ButtonRef.GetComponent<Weapons_Info>().ItemID].ToString();
        }
    }

    public void Sell()
    {
        GameObject ButtonSell = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonSell.GetComponent<Weapons_Info>().ItemID])
        {
            coins -= shopItems[2, ButtonSell.GetComponent<Weapons_Info>().ItemID];
            shopItems[3, ButtonSell.GetComponent<Weapons_Info>().ItemID]--;
            CoinTxt.text = "MoonCoins: " + coins.ToString();
            ButtonSell.GetComponent<Weapons_Info>().QuantityTxt.text = shopItems[3, ButtonSell.GetComponent<Weapons_Info>().ItemID].ToString();
        }
    }
}
