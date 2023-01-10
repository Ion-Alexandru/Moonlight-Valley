using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Seeds : MonoBehaviour
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
        shopItems[2, 1] = 1;
        shopItems[2, 2] = 2;
        shopItems[2, 3] = 20;
        shopItems[2, 4] = 7;
        shopItems[2, 5] = 15;

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

        if (coins >= shopItems[2, ButtonRef.GetComponent<Seeds_Info>().ItemID])
        {
            coins -= shopItems[2, ButtonRef.GetComponent<Seeds_Info>().ItemID];
            shopItems[3, ButtonRef.GetComponent<Seeds_Info>().ItemID]++;
            CoinTxt.text = "MoonCoins: " + coins.ToString();
            ButtonRef.GetComponent<Seeds_Info>().QuantityTxt.text = shopItems[3, ButtonRef.GetComponent<Seeds_Info>().ItemID].ToString();
        }

    }
    public void Sell()
    {
        GameObject ButtonSell = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonSell.GetComponent<Seeds_Info>().ItemID])
        {
            coins -= shopItems[2, ButtonSell.GetComponent<Seeds_Info>().ItemID];
            shopItems[3, ButtonSell.GetComponent<Seeds_Info>().ItemID]--;
            CoinTxt.text = "MoonCoins: " + coins.ToString();
            ButtonSell.GetComponent<Seeds_Info>().QuantityTxt.text = shopItems[3, ButtonSell.GetComponent<Seeds_Info>().ItemID].ToString();
        }
    }

    }