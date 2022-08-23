using System;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    //Step 1 create your products
    private const string ordinaryVipChest = "ordinary_vip_chest";
    private const string superVipPack = "super_vip_set";

    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(ordinaryVipChest, ProductType.Consumable);
        builder.AddProduct(superVipPack, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    //custom methods outer
    public void BuyOrdinaryVipChest()
    {
        BuyProductID(ordinaryVipChest);
    }
    public void BuySuperVipPack()
    {
        BuyProductID(superVipPack);
    }

    //purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        PlayerPrefs.SetInt("Coins", 4500);

        if (String.Equals(args.purchasedProduct.definition.id, ordinaryVipChest, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("Coins", 5000);

            PlayerPrefs.SetInt("VipChestCount", PlayerPrefs.GetInt("VipChestCount") + 1);
            PlayerPrefs.SetInt("0_OFFER", 1);

            PurchaseChest[] gos = FindObjectsOfType<PurchaseChest>();
            if (gos.Length > 0)
            {
                foreach (PurchaseChest go in gos)
                {
                    if (go.isOrdinaryVIP)
                        go.TurnColliders(true);
                }
            }
        }
        else if (String.Equals(args.purchasedProduct.definition.id, superVipPack, StringComparison.Ordinal))
        {
            PlayerPrefs.SetInt("VipChestCount", PlayerPrefs.GetInt("VipChestCount") + 5);
            PlayerPrefs.SetString("ShowAds", "false");
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 5000);
            PlayerPrefs.SetInt("1_OFFER", 1);

            //changing button
            PurchaseChest[] gos = FindObjectsOfType<PurchaseChest>();
            if (gos.Length > 0)
            {
                foreach (PurchaseChest go in gos)
                {
                    if (!go.isOrdinaryVIP)
                    {
                        go.TurnColliders(true);
                        go.ChangeSuperVipPackButton();
                    }
                        
                }
            }
        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }

    ////////////////////////

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    void BuyProductID(string productId)
    {
        PlayerPrefs.SetInt("Coins", 4000);

        if (IsInitialized())
        {
            PlayerPrefs.SetInt("Coins", 4100);

            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                PlayerPrefs.SetInt("Coins", 4200);
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                PlayerPrefs.SetInt("Coins", 9);
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            PlayerPrefs.SetInt("Coins", 9999);
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}