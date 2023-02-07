using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shop_Script : MonoBehaviour
{
    public int[,] shopItems = new int[6, 6];
    [SerializeField] public TextMeshProUGUI CoinTxt;
    public Shop_Items_Script shopItemsScript;
    public int currentShop = 0;
    public Button nextButton;
    public Button backButton;
    public Sprite[] itemPhotos;
    public GameObject notEnoughMoneyPanel;
    public GameObject notEnoughVegetablesPanel;
    public int[] quantityArray;
    public int moonCoins;

    protected void Awake(){
        moonCoins = PlayerPrefs.GetInt("MoonCoins", 0);
    }

    void Start()
    {    
        StaticClass.moonCoins = moonCoins;

        notEnoughMoneyPanel.SetActive(false);
        notEnoughVegetablesPanel.SetActive(false);

        nextButton.onClick.AddListener(NextShop);
        backButton.onClick.AddListener(BackShop);

    }

    public void Update()
    {
        CoinTxt.text = "MoonCoins: " + moonCoins.ToString();

        if (currentShop == 0) //seeds
        {
            

            //Id's
            shopItems[1, 0] = 0;
            shopItems[1, 1] = 1;
            shopItems[1, 2] = 2;
            shopItems[1, 3] = 3;
            shopItems[1, 4] = 4;

            //Price
            shopItems[2, 0] = 1;
            shopItems[2, 1] = 2;
            shopItems[2, 2] = 20;
            shopItems[2, 3] = 7;
            shopItems[2, 4] = 15;

            //Quantity
            shopItems[3, 0] = 0;
            shopItems[3, 1] = 0;
            shopItems[3, 2] = 0;
            shopItems[3, 3] = 0;
            shopItems[3, 4] = 0;

        }


        else if (currentShop == 1) //vegetables
        {


            //Id's
            shopItems[1, 0] = 0;
            shopItems[1, 1] = 1;
            shopItems[1, 2] = 2;
            shopItems[1, 3] = 3;
            shopItems[1, 4] = 4;

            //Price
            shopItems[2, 0] = 3;
            shopItems[2, 1] = 5;
            shopItems[2, 2] = 25;
            shopItems[2, 3] = 10;
            shopItems[2, 4] = 18;

            //Quantity
            shopItems[3, 0] = 0;
            shopItems[3, 1] = 0;
            shopItems[3, 2] = 0;
            shopItems[3, 3] = 0;
            shopItems[3, 4] = 0;

               
        }
        else if (currentShop == 2) //weapons
        {


            //Id's
            shopItems[1, 0] = 0;
            shopItems[1, 1] = 1;
            shopItems[1, 2] = 2;
            shopItems[1, 3] = 3;
            shopItems[1, 4] = 4;

            //Price
            shopItems[2, 0] = 35;
            shopItems[2, 1] = 50;
            shopItems[2, 2] = 45;
            shopItems[2, 3] = 30;
            shopItems[2, 4] = 55;

            //Quantity
            shopItems[3, 0] = 0;
            shopItems[3, 1] = 0;
            shopItems[3, 2] = 0;
            shopItems[3, 3] = 0;
            shopItems[3, 4] = 0;
        }
    }

    public void Transaction()
    {
        if ( currentShop == 1)
        {
            Sell();

        }
        else
        {
            Buy();
        }
    }
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (moonCoins >= shopItems[2, ButtonRef.GetComponent<Shop_Items_Script>().ItemID])
        {
            moonCoins -= shopItems[2, ButtonRef.GetComponent<Shop_Items_Script>().ItemID];
            shopItems[3, ButtonRef.GetComponent<Shop_Items_Script>().ItemID]++;
            CoinTxt.text = "MoonCoins: " + moonCoins.ToString();
            Debug.Log(ButtonRef.GetComponent<Shop_Items_Script>().ItemID);
            
            if (currentShop == 0)
            {
                Debug.Log(quantityArray[(int)(ButtonRef.GetComponent<Shop_Items_Script>().ItemID)]);
                quantityArray[ButtonRef.GetComponent<Shop_Items_Script>().ItemID] = quantityArray[ButtonRef.GetComponent<Shop_Items_Script>().ItemID] + 1;
                
            } else if (currentShop == 2 && quantityArray[ButtonRef.GetComponent<Shop_Items_Script>().ItemID + 10] == 0)
            {
                quantityArray[ButtonRef.GetComponent<Shop_Items_Script>().ItemID + 10] = quantityArray[ButtonRef.GetComponent<Shop_Items_Script>().ItemID + 10] + 1;
            }

        }
        else
        {
            notEnoughMoneyPanel.SetActive(true);
            Invoke("HidePanel", 0.8f); //hide the panel after 0.8 seconds
        }
    }

    public void Sell()
    {
        GameObject ButtonSell = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (moonCoins >= shopItems[2, ButtonSell.GetComponent<Shop_Items_Script>().ItemID] && quantityArray[ButtonSell.GetComponent<Shop_Items_Script>().ItemID + 5] > 0)
        {
            moonCoins += shopItems[2, ButtonSell.GetComponent<Shop_Items_Script>().ItemID];
            shopItems[3, ButtonSell.GetComponent<Shop_Items_Script>().ItemID]--;
            CoinTxt.text = "MoonCoins: " + moonCoins.ToString();

            if (currentShop == 1 && quantityArray[ButtonSell.GetComponent<Shop_Items_Script>().ItemID + 5] > 0)
            {

                quantityArray[ButtonSell.GetComponent<Shop_Items_Script>().ItemID + 5] = quantityArray[ButtonSell.GetComponent<Shop_Items_Script>().ItemID +5] - 1;

            } 
           

            ButtonSell.GetComponent<Shop_Items_Script>().QuantityTxt.text = shopItems[3, ButtonSell.GetComponent<Shop_Items_Script>().ItemID].ToString();
        }
        else
        {
            notEnoughVegetablesPanel.SetActive(true);
            Invoke("HidePanel", 0.8f); //hide the panel after 0.8 seconds
        }
    }

    private void HidePanel()
    {
        notEnoughMoneyPanel.SetActive(false);
        notEnoughVegetablesPanel.SetActive(false);
    }

    public void NextShop()
    {

        if (currentShop < 5)
        {
            currentShop++;
            Update();
        }
    }
    public void BackShop()
    {

        if (currentShop > 0)
        {
            currentShop--;
            Update();
        }
    }

    
}


