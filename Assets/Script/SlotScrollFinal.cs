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
    [SerializeField] Ease startEase,ease,endEase;
    [SerializeField] float time = 5,endtime = 5f,startTime = 2f;
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
        //Debug.Log(ran);
        var target = -iconHeight * transform.childCount /*- firstIconPos*/;
        //rect.DOAnchorPosY((-iconHeight * transform.childCount), endtime).SetEase(ease).OnUpdate(ReUseIcon);
        seq.Append(rect.DOAnchorPosY((-iconHeight * 4), startTime).SetEase(startEase).OnUpdate(ReUseIcon));
        seq.Append(rect.DOAnchorPosY(((target * numberofLoops) + (iconHeight * 0.9f)), time).SetEase(ease).OnUpdate(ReUseIcon));
        seq.Append(rect.DOAnchorPosY((target * numberofLoops), endtime).SetEase(endEase).OnUpdate(ReUseIcon));

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
            var child = transform.GetChild(transform.childCount - 1);
            child.GetComponent<RectTransform>().anchoredPosition = new Vector2(child.GetComponent<RectTransform>().anchoredPosition.x, firstIconPos + (iconHeight * iconCounter));
            child.SetAsFirstSibling();
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
