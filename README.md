# Tool and Assets
Unity Version: 6000.3.8f1

Art Assets: https://retrostylegames.itch.io/crowns-of-kemet-free

# Slot Machine UI

A simple slot machine animation system built in Unity. The main focus of this project was creating a smooth scrolling effect while keeping the implementation lightweight and performance-friendly.

## Demo

Here is a gameplay demonstration:

https://github.com/user-attachments/assets/a135cd34-561f-44f8-aa42-66d3fcf6da12

## How It Works

The main logic is handled by two scripts inside the `Assets/Scripts` folder:

* `SlotController.cs`
* `SlotScrollFinal.cs`

Only these two scripts are responsible for the slot animation and scrolling logic.

### SlotController

`SlotController` works as the master controller for all the slots. It handles the overall flow and sends events to the individual slot components.

When the player clicks the **Spin** button, it calls `ResetSpin()`. This resets the position of the slots and their icons before starting a new spin.

```cs
public void ResetSpin()
{
    scrollBtn.interactable = false;

    foreach (var slotScroll in slots)
    {
        slotScroll.ResetSlot();
    }

    StartCoroutine(StartSpinCoroutine());
}
```

### SlotScrollFinal

`SlotScrollFinal` handles the actual scrolling of each slot.

I'm using **DOTween** to create the animation sequence:

```cs
public void StartSpin()
{
    Sequence seq = DOTween.Sequence();

    var ran = Random.Range(0, 10);
    var target = -iconHeight * transform.childCount;

    seq.Append(
        rect.DOAnchorPosY(-iconHeight * 4, startTime)
            .SetEase(startEase)
            .OnUpdate(ReUseIcon)
    );

    seq.Append(
        rect.DOAnchorPosY(
            (target * numberofLoops) + (iconHeight * 0.9f),
            time
        )
        .SetEase(ease)
        .OnUpdate(ReUseIcon)
    );

    seq.Append(
        rect.DOAnchorPosY(target * numberofLoops, endtime)
            .SetEase(endEase)
            .OnUpdate(ReUseIcon)
    );

    seq.OnComplete(() =>
    {
        scrollFinish?.Invoke();
    });
}
```

## Infinite Scrolling

For the infinite scrolling effect, I'm using a simple pooling approach.

Instead of creating and destroying icons while the slot is moving, I reuse the existing icons by changing their positions and sibling order.

```cs
public void ReUseIcon()
{
    if (rect.anchoredPosition.y <= -(iconHeight * iconCounter))
    {
        var child = transform.GetChild(transform.childCount - 1);

        child.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(
                child.GetComponent<RectTransform>().anchoredPosition.x,
                firstIconPos + (iconHeight * iconCounter)
            );

        child.SetAsFirstSibling();

        iconCounter++;
    }
}
```

The important part here is that I'm calculating the position of the **slot container**, rather than individually moving every icon.

The main goal of this project was to keep the UI animation smooth. Because of that, I kept the implementation focused on a fixed icon spacing.

If different spacing between icons is required, it can be added by introducing another variable and making a small adjustment to the position calculation.

## Optimization

There can be 45+ slot icons in total, depending on the number of slots and icons.

Moving every icon individually would require updating the position of a large number of UI elements every frame.

Instead, I'm moving the **entire slot container** and only repositioning an icon when it needs to be reused.

This means:

* The slot container handles the main movement.
* Icons are reused instead of being created or destroyed.
* Only the icon that moves outside the required area gets repositioned.
* There is no need to continuously calculate the movement of every individual icon.

This keeps the implementation simple while reducing unnecessary UI updates during the animation.

## Main Focus

The main focus of this project was:

* Smooth slot scrolling
* Infinite/reusable icons
* Minimal object movement
* Simple and reusable code
* Performance-friendly UI animation
* DOTween-based animation sequencing
