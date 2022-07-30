using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{
    public Button[] buttons;
    public Text moneyText;
    public Text[] upgradesText;
    public Text[] priceText;
    public int money;
    public int speedPrice;
    public float speed;
    public int startBackupPrice;
    public int startUnits;
    public int battleBackupPrice;
    public int battleUnits;
    BackToLobby bl;
    GameManager gameManager;
    public GameObject adsButton;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("money")) PlayerPrefs.SetInt("money", 0);

        if (!PlayerPrefs.HasKey("speed")) PlayerPrefs.SetFloat("speed", 1f);     

        if (!PlayerPrefs.HasKey("startUnits")) PlayerPrefs.SetInt("startUnits", 0);

        if (!PlayerPrefs.HasKey("battleUnits")) PlayerPrefs.SetInt("battleUnits", 0);

        if (!PlayerPrefs.HasKey("speedPrice")) PlayerPrefs.SetInt("speedPrice", 150);

        if (PlayerPrefs.GetInt("ifNoAds") == 1) Destroy(adsButton);
    }

    void Start()
    {
        bl = FindObjectOfType<BackToLobby>();

        money = PlayerPrefs.GetInt("money");
        speed = PlayerPrefs.GetFloat("speed");
        startUnits = PlayerPrefs.GetInt("startUnits");
        battleUnits = PlayerPrefs.GetInt("battleUnits");

        speedPrice = (int)PlayerPrefs.GetInt("speedPrice");
       
        if (startUnits < 1) { startBackupPrice = 50; }
        else { startBackupPrice = (int)startUnits * 50 + 50; }
        if (battleUnits < 1) { battleBackupPrice = 210; }
        else { battleBackupPrice = (int)battleUnits * 210 + 210; }


        upgradesText[0].text = speed.ToString();
        priceText[0].text = speedPrice.ToString();
        upgradesText[1].text = startUnits.ToString();
        priceText[1].text = startBackupPrice.ToString();
        upgradesText[2].text = battleUnits.ToString();
        priceText[2].text = battleBackupPrice.ToString();

        print(speedPrice);
        RefreshMoneyText();

        //PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
        //PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        //if(PurchaseManager.CheckBuyState("noads"))
        //{
        //    PlayerPrefs.SetInt("ifNoAds", 1);
        //}
    }

    //private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    //{
    //    if( args.purchasedProduct.definition.id == "50")
    //    {
    //        AddMoney(50);

    //    }   
    //    if (args.purchasedProduct.definition.id == "100")
    //    {
    //        AddMoney(100);
    //    }
    //    if (args.purchasedProduct.definition.id == "500")
    //    {
    //        AddMoney(500);
    //    }
    //    if (args.purchasedProduct.definition.id == "1000")
    //    {
    //        AddMoney(1000);
    //    }
    //    if (args.purchasedProduct.definition.id == "5000")
    //    {
    //        AddMoney(5000);
    //    }
    //    if (args.purchasedProduct.definition.id == "10000")
    //    {
    //        AddMoney(10000);
    //    }

    //    RefreshMoneyText();
    //}

    //private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    //{
    //    if (args.purchasedProduct.definition.id == "noads")
    //    {
    //        Debug.Log("Работает" + args.purchasedProduct.definition.id + "NConsumable");
    //        NoAds();
    //    }
        
    //}

    void Update()
    {
        if (Input.GetKey(KeyCode.N))
        {
            money++;
            RefreshMoneyText();
        }
        if (Input.GetKey(KeyCode.O))
        {
            PlayerPrefs.SetInt("ifNoAds", 0);
        }
        if (Input.GetKey(KeyCode.P))
        {
            money = 0;
            RefreshMoneyText();
        }


    }

    void RefreshMoneyText()
    {
        moneyText.text = money.ToString();
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetInt("startUnits", startUnits);
        PlayerPrefs.SetInt("battleUnits", battleUnits);

        if(speed >= 6f)
        {
            upgradesText[0].text = "MAX";
            priceText[0].text = "MAX";
        }
    }

    public void SpendMoney()
    {
        if(money > 0)
        {
            money--;
        }
    }

    public void BuySpeed()
    {
        if(speed < 6)
        {
            if (money >= speedPrice)
            {
                money -= speedPrice;
                speedPrice += 150;
                speed += .1f;

                PlayerPrefs.SetInt("speedPrice", speedPrice);
                upgradesText[0].text = speed.ToString();
                priceText[0].text = speedPrice.ToString();
                RefreshMoneyText();

            }
            else bl.OpenDonateMenu();
        }
    }


    public void BuyStartBackup()
    {
        if (money >= startBackupPrice)
        {
            money -= startBackupPrice;
            startBackupPrice = (int)startUnits * 50 + 50;
            startUnits++;

            PlayerPrefs.SetInt("startBackupPrice", startBackupPrice);
            upgradesText[1].text = startUnits.ToString();
            priceText[1].text = startBackupPrice.ToString();
            RefreshMoneyText();
        }
        else bl.OpenDonateMenu();
    }
    public void BuyBattleBackup()
    {
        if (money >= battleBackupPrice)
        {
            money -= battleBackupPrice;
            battleBackupPrice = (int)battleUnits * 210 + 210;
            battleUnits++;

            PlayerPrefs.SetInt("battleBackupPrice", battleBackupPrice);
            upgradesText[2].text = battleUnits.ToString();
            priceText[2].text = battleBackupPrice.ToString();
            RefreshMoneyText();
        }
        else bl.OpenDonateMenu();
    }

    public void AddMoney(int howMuch)
    {
        money += howMuch;
        RefreshMoneyText();
    }

    public void NoAds()
    {
        PlayerPrefs.SetInt("ifNoAds", 1);
        print("ok");
    }
}
