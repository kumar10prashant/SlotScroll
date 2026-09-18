using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlotScrollFinal : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] float iconHeight;
    [SerializeField] float rectHeight;
    [SerializeField] int numberofLoops;
    [SerializeField] Ease ease,endEase;
    [SerializeField] float time = 5,endtime = 5f;
    [SerializeField] int iconCounter;
    [SerializeField] float firstIconPos;
    
    public static Action scrollFinish;


   
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        iconHeight = transform.GetChild(0).GetComponent<RectTransform>().rect.height;
        firstIconPos = rect.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y;

    }


    public void StartSpin()
    {
        Sequence seq = DOTween.Sequence();
        var ran = Random.Range(0, 10);
        Debug.Log(ran);
        var target = -firstIconPos - (ran * iconHeight);
        seq.Append(rect.DOAnchorPosY(target + iconHeight * 0.15f, time).SetEase(ease).OnUpdate(ReUseIcon).SetLoops(numberofLoops, LoopType.Incremental));
        seq.Append(rect.DOAnchorPosY((target * (numberofLoops)), endtime).SetEase(endEase).OnUpdate(ReUseIcon));
        seq.OnComplete(() =>
        {
            scrollFinish?.Invoke();
        });
    }
    public void ReUseIcon()
    {

        if(rect.anchoredPosition.y <= -(iconHeight * iconCounter))
        {
           // Debug.Log(iconHeight * iconCounter);
            transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition = new Vector2(transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>().anchoredPosition.x, firstIconPos + (iconHeight * iconCounter));
            transform.GetChild(transform.childCount - 1).SetAsFirstSibling();
            iconCounter++;
            
        }
    }

   

    public void ResetSlot()
    {
        DOTween.Kill(rect);

        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x,0);

        iconCounter = 1;

   
     /*   for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetSiblingIndex(i);
        }
*/
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform icon =
                transform.GetChild(i).GetComponent<RectTransform>();

            icon.anchoredPosition = new Vector2(icon.anchoredPosition.x,firstIconPos - (i * iconHeight));
        }
    }
}
