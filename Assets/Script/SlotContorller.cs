using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotContorller : MonoBehaviour
{
    [SerializeField] float delayBetweenSlots;
    int slotCount;
    SlotScrollFinal[] slots;

    [SerializeField] Button scrollBtn;
   

    public void StartSpin()
    {
       
    }

    private void OnEnable()
    {
        SlotScrollFinal.scrollFinish += ScrollComplete;
    }

    private void OnDisable()
    {
        SlotScrollFinal.scrollFinish -= ScrollComplete;
    }

    public void ScrollComplete()
    {
        slotCount++;
        if(slotCount == slots.Length)
        {
            Debug.Log("Finish");
            slotCount = 0;
            scrollBtn.interactable = true;
        }
    }

    public void ResetSpin()
    {
        slots = transform.GetComponentsInChildren<SlotScrollFinal>();
        scrollBtn.interactable = false;
        foreach (var slotScroll in slots)
        {
            slotScroll.ResetSlot();
            
        }
        StartCoroutine(StartSpinCoroutine());
    }

    IEnumerator StartSpinCoroutine()
    {
        slots = transform.GetComponentsInChildren<SlotScrollFinal>();
        foreach(var slotScroll in slots)
        {
            slotScroll.StartSpin();
            yield return new WaitForSeconds(delayBetweenSlots);
        }
    }
}
