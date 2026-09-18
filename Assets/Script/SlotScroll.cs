using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotScroll : MonoBehaviour
{
    [SerializeField] float iconHeight;
    [SerializeField] int iconCount = 1;
    [SerializeField] float height;

    [SerializeField] int numberOfSpin = 5;
    [SerializeField] float spinSpeed = 100f;

    [SerializeField] Ease easeType;

    [SerializeField] RectTransform rect;

    public static Action FinishScroll;

    [SerializeField] float lastIconPos;

   

    IEnumerator Start()
    {
        yield return null;
        rect = GetComponent<RectTransform>();
        iconHeight = transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        int childcount = transform.childCount;
        lastIconPos = transform.GetChild(childcount-1).GetComponent<RectTransform>().anchoredPosition.y;
        StartSpin();
    
    }

    public void StartSpin()
    {
        int ran = Random.Range(0, 10);
        height = rect.rect.height;
        var moveDistance = height * numberOfSpin;
        Debug.Log(moveDistance % iconHeight);
        var target = moveDistance + (iconHeight * ran);
        Sequence sequence = DOTween.Sequence();
        rect.DOAnchorPosY(-target, spinSpeed).SetSpeedBased().SetEase(easeType);
    }

    public void ResetPos()
    {
        for(int i = 0;i<transform.childCount;i++)
        {

        }
    }

   
    public void ReUseIcon()
    {
        if (transform.GetComponent<RectTransform>().anchoredPosition.y <= (iconHeight * iconCount))
        {

            transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.x, (lastIconPos - (iconHeight * iconCount)));
            iconCount++;
            transform.GetChild(0).SetAsLastSibling();
        }
    }

    private void Update()
    {
        ReUseIcon();
    }
}
