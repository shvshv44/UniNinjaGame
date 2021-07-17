using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public Slot[] slots;
    public PlayerStats stats;
    public AudioSource audioSrc;
    public AudioClip usePotionSound;

    private int selected;

    void Start()
    {
        selected = -1;
    }

    public int CountNonEmpty()
    {
        int count = 0;

        for (int index = 0; index < slots.Length; index++)
        {
            if (! slots[index].IsEmpty())
            {
                count++;
            }
        }

        return count;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Consume();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChooseNextSlot();
        }
    }

    public void Consume()
    {
        if(selected > -1)
        {
            Slot currentSlot = slots[selected];
            if(!currentSlot.IsEmpty())
            {
                audioSrc.PlayOneShot(usePotionSound, 0.7f);
                currentSlot.Consume(stats);
                if (currentSlot.IsEmpty())
                {
                    ChooseNextSlot();
                }
            }
        }
    }

    public void ChooseNextSlot()
    {
        int numOfSlots = slots.Length;
        int iterationCount = 0;
        int currentSlotToCheck = selected + 1;

        if(selected != -1)
        {
            slots[selected].unselect();
        }

        while(iterationCount < numOfSlots)
        {
            iterationCount++;
            if (!slots[currentSlotToCheck].IsEmpty())
            {
                break;
            }

            currentSlotToCheck++;
            if (currentSlotToCheck >= numOfSlots)
            {
                currentSlotToCheck = 0;
            }
        }

        if(iterationCount == numOfSlots)
        {
            NothingSelected();
        } else
        {
            Select(currentSlotToCheck);
        }
    }

    public void AddItem(Consumable c)
    {
        bool isNew = true;

        // Find if exists
        for(int index = 0; index < slots.Length; index++)
        {
            if (!slots[index].IsEmpty() && slots[index].GetItemType().Equals(c.name))
            {
                isNew = false;
                slots[index].AddOne();
                break;
            }
        }

        // Add if not exists
        if (isNew)
        {
            for (int index = 0; index < slots.Length; index++)
            {
                if (slots[index].IsEmpty())
                {
                    slots[index].AddNewItem(c);
                    if(CountNonEmpty() == 1)
                    {
                        Select(index);
                    }

                    break;
                }
            }
        }
    }

    public void NothingSelected()
    {
        selected = -1;
    }

    public void Select(int index)
    {
        selected = index;
        slots[index].select();
    }




}
