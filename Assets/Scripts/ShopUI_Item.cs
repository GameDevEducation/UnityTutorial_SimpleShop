using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ShopUI_Item : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ItemName;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] TextMeshProUGUI Price;
    [SerializeField] Image BackgroundPanel;
    [SerializeField] Color DefaultColour;
    [SerializeField] Color SelectedColour;

    UnityAction<ShopItem> OnSelectedFn;

    ShopItem Item;

    public void Bind(ShopItem item, UnityAction<ShopItem> onSelectedFn)
    {
        Item = item;
        OnSelectedFn = onSelectedFn;

        ItemName.text = Item.Name;
        Description.text = Item.Description;
        Price.text = $"{(Item.Cost / 100f):0.00}";

        SetIsSelected(false);
    }

    public void SetIsSelected(bool selected)
    {
        BackgroundPanel.color = selected ? SelectedColour : DefaultColour;
    }

    public void OnClicked()
    {
        OnSelectedFn.Invoke(Item);
    }

    public void SetCanAfford(bool canAfford)
    {
        Price.fontStyle = canAfford ? FontStyles.Normal : FontStyles.Strikethrough;
        Price.color = canAfford ? Color.white : Color.red;
    }
}
