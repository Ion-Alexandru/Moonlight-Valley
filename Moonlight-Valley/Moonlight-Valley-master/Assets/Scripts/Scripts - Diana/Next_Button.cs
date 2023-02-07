using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Next_Button : MonoBehaviour
{
    public Shop_Script shopScript;


    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => shopScript.NextShop());
    }

    public void Back()
    {
        GetComponent<Button>().onClick.AddListener(() => shopScript.BackShop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
