using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Items_Script : MonoBehaviour

{
    public int ItemID;
    [SerializeField] public TextMeshProUGUI PriceTxt;
    [SerializeField] public TextMeshProUGUI QuantityTxt;
    public Image ItemPhoto;
    public GameObject ShopManager;
    public Shop_Script shopScript;
    public Button buyButton;
    public Sprite[] buttonIcon;
    public Image soldOut;



    public void Start()
    {

        soldOut.gameObject.SetActive(false);

    }
    public void Update()
    {

        PriceTxt.text = ShopManager.GetComponent<Shop_Script>().shopItems[2, ItemID].ToString();



        if (shopScript.currentShop == 0)
        {
            buyButton.image.sprite = buttonIcon[0];

            ItemPhoto.sprite = ShopManager.GetComponent<Shop_Script>().itemPhotos[ItemID];

            QuantityTxt.text = shopScript.quantityArray[ItemID].ToString();

            soldOut.gameObject.SetActive(false);


        } else if(shopScript.currentShop == 1)
        {
            buyButton.image.sprite = buttonIcon[1];

            ItemPhoto.sprite = ShopManager.GetComponent<Shop_Script>().itemPhotos[ItemID + 5];

            QuantityTxt.text = shopScript.quantityArray[ItemID + 5].ToString();

            soldOut.gameObject.SetActive(false);

        }
        else if (shopScript.currentShop == 2)
        {
            buyButton.image.sprite = buttonIcon[0];

            ItemPhoto.sprite = ShopManager.GetComponent<Shop_Script>().itemPhotos[ItemID + 10];

            QuantityTxt.text = shopScript.quantityArray[ItemID + 10].ToString();

            if (shopScript.quantityArray[ItemID + 10] > 0)
            {
                soldOut.gameObject.SetActive(true);
            }
        }

    }


}


