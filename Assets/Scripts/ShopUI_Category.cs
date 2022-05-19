using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ShopUI_Category : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI CategoryName;
    [SerializeField] Image BackgroundPanel;
    [SerializeField] Color DefaultColour;
    [SerializeField] Color SelectedColour;

    ShopItemCategory Category;
    UnityAction<ShopItemCategory> OnSelectedFn;
    
    public void Bind(ShopItemCategory category, UnityAction<ShopItemCategory> onSelectedFn)
    {
        Category = category;
        CategoryName.text = Category.Name;
        OnSelectedFn = onSelectedFn;

        SetIsSelected(false);
    }

    public void SetIsSelected(bool selected)
    {
        BackgroundPanel.color = selected ? SelectedColour : DefaultColour;
    }

    public void OnClicked()
    {
        OnSelectedFn.Invoke(Category);
    }
}
