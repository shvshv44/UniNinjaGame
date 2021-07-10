using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{

    public Image containerImg;
    public Image iconImg;
    public Text quantityTxt;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;
    public Sprite iconSprite;

    private bool isSelected;
    private Consumable consumable;
    private int quantity;

    private void Start()
    {
        unselect();
        DisableSlot();
        quantity = 0;
    }

    public void select()
    {
        isSelected = true;
        containerImg.sprite = selectedSprite;
        
    }

    public void unselect()
    {
        isSelected = false;
        containerImg.sprite = unselectedSprite;
    }

    public void Consume()
    {
        if (consumable != null && quantity > 0)
        {
            consumable.Consume();
            quantity--;

            if (quantity == 0)
            {
                DisableSlot();
                unselect();
            } else
            {
                quantityTxt.text = "x" + quantity;
            }
        }
    }

    public bool IsEmpty()
    {
        return quantity == 0;
    }

    public string GetItemType()
    {
        return consumable.name;
    }

    public void AddOne()
    {
        quantity++;
        quantityTxt.text = "x" + quantity;
    }

    public void AddNewItem(Consumable c)
    {
        consumable = c;
        iconImg.sprite = c.icon;
        quantity = 1;
        quantityTxt.text = "x" + quantity;
        EnableSlot();
    }

    public void DisableSlot()
    {
        iconImg.enabled = false;
        quantityTxt.enabled = false;
    }

    public void EnableSlot()
    {
        quantityTxt.enabled = true;
        iconImg.enabled = true;
    }


}
