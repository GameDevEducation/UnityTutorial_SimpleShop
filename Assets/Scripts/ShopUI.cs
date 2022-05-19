using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI AvailableFunds;
    [SerializeField] Transform CategoryUIRoot;
    [SerializeField] Transform ItemUIRoot;

    [SerializeField] Button PurchaseButton;

    [SerializeField] GameObject CategoryUIPrefab;
    [SerializeField] GameObject ItemUIPrefab;

    [SerializeField] List<ShopItem> AvailableItems;

    IPurchaser CurrentPurchaser;
    ShopItemCategory SelectedCategory;
    ShopItem SelectedItem;

    List<ShopItemCategory> ShopCategories;
    Dictionary<ShopItemCategory, ShopUI_Category> ShopCategoryToUIMap;
    Dictionary<ShopItem, ShopUI_Item> ShopItemToUIMap;

    // Start is called before the first frame update
    void Start()
    {
        // BEGIN TESTING CODE
        CurrentPurchaser = FindObjectOfType<Purchaser>();
        // END TESTING CODE

        RefreshShopUI_Common();
        RefreshShopUI_Categories();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshShopUI_Common()
    {
        if (CurrentPurchaser != null)
            AvailableFunds.text = $"{(CurrentPurchaser.GetCurrentFunds() / 100f):0.00}";
        else
            AvailableFunds.text = string.Empty;

        if (CurrentPurchaser != null && SelectedItem != null &&
            CurrentPurchaser.GetCurrentFunds() >= SelectedItem.Cost)
        {
            PurchaseButton.interactable = true;
        }
        else
            PurchaseButton.interactable = false;

        if (ShopItemToUIMap != null)
        {
            foreach (var kvp in ShopItemToUIMap)
            {
                var item = kvp.Key;
                var itemUI = kvp.Value;

                if (CurrentPurchaser != null)
                    itemUI.SetCanAfford(item.Cost <= CurrentPurchaser.GetCurrentFunds());
                else
                    itemUI.SetCanAfford(false);
            }
        }
    }

    void RefreshShopUI_Categories()
    {
        // clear the current UI
        for (int childIndex = CategoryUIRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            var childGO = CategoryUIRoot.GetChild(childIndex).gameObject;

            Destroy(childGO);
        }

        ShopCategories = new List<ShopItemCategory>();
        ShopCategoryToUIMap = new Dictionary<ShopItemCategory, ShopUI_Category>();
        
        // determine our category list
        foreach(var item in AvailableItems)
        {
            if (!ShopCategories.Contains(item.Category))
                ShopCategories.Add(item.Category);
        }

        ShopCategories.Sort((lhs, rhs) => lhs.Name.CompareTo(rhs.Name));

        // instantiate the categories
        foreach(var category in ShopCategories)
        {
            var categoryGO = Instantiate(CategoryUIPrefab, CategoryUIRoot);
            var categoryUI = categoryGO.GetComponent<ShopUI_Category>();

            categoryUI.Bind(category, OnCategorySelected);
            ShopCategoryToUIMap[category] = categoryUI;
        }

        if (!ShopCategories.Contains(SelectedCategory))
            SelectedCategory = null;

        OnCategorySelected(SelectedCategory);
    }

    void RefreshShopUI_Items()
    {
        // clear the current UI
        for (int childIndex = ItemUIRoot.childCount - 1; childIndex >= 0; childIndex--)
        {
            var childGO = ItemUIRoot.GetChild(childIndex).gameObject;

            Destroy(childGO);
        }

        ShopItemToUIMap = new Dictionary<ShopItem, ShopUI_Item>();

        foreach(var item in AvailableItems)
        {
            if (item.Category != SelectedCategory)
                continue;

            var itemGO = Instantiate(ItemUIPrefab, ItemUIRoot);
            var itemUI = itemGO.GetComponent<ShopUI_Item>();

            itemUI.Bind(item, OnItemSelected);
            ShopItemToUIMap[item] = itemUI;
        }

        RefreshShopUI_Common();
    }

    void OnCategorySelected(ShopItemCategory newlySelectedCategory)
    {
        // clear the selected item
        if (SelectedCategory != null && newlySelectedCategory != null && SelectedCategory != newlySelectedCategory)
        {
            SelectedItem = null;
        }

        // update the selection
        SelectedCategory = newlySelectedCategory;
        foreach(var category in ShopCategories)
        {
            ShopCategoryToUIMap[category].SetIsSelected(category == SelectedCategory);
        }

        RefreshShopUI_Items();
    }

    void OnItemSelected(ShopItem newlySelectedItem)
    {
        // update the selection
        SelectedItem = newlySelectedItem;
        foreach(var kvp in ShopItemToUIMap)
        {
            var item = kvp.Key;
            var itemUI = kvp.Value;

            itemUI.SetIsSelected(item == SelectedItem);
        }

        RefreshShopUI_Common();
    }

    public void OnClickedPurchase()
    {
        CurrentPurchaser.SpendFunds(SelectedItem.Cost);
        RefreshShopUI_Common();
    }

    public void OnClickedExit()
    {

    }
}
