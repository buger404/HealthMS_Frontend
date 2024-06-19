using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Milease.Core.Animator;
using Milease.Core.UI;
using Milease.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SortModeItem : MilListViewItem
{
    public TMP_Text Content;

    protected override IEnumerable<MilStateParameter> ConfigDefaultState()
        => new[]
        {
            Content.MilState("color", ColorUtils.RGB(127f, 127f, 127f))
        };
    
    protected override IEnumerable<MilStateParameter> ConfigSelectedState()
        => new[]
        {
            Content.MilState("color", ColorUtils.RGB(235,68,80))
        };

    public override void OnSelect(PointerEventData eventData)
    {
        SortChaperone(Index);
    }

    protected override void OnInitialize()
    {

    }

    protected override void OnTerminate()
    {

    }

    protected override MilInstantAnimator ConfigClickAnimation()
        => null;

    public override void UpdateAppearance()
    {
        Content.text = Binding as string;
    }

    public override void AdjustAppearance(float pos)
    {

    }

    public static void SortChaperone(int index = -1)
    {
        if (ChaperoneController.Instance.SortMode.SelectedIndex <= 0)
        {
            return;
        }
        var items = ChaperoneController.Instance.ListView.Items.Cast<ChaperoneModel>().ToList();
        switch (index == -1 ? ChaperoneController.Instance.SortMode.SelectedIndex : index)
        {
            case 1:
                // 评分最高
                break;
            case 2:
                // 价格最低
                items.Sort((x, y) => x.price.CompareTo(y.price));
                break;
            case 3:
                // 最多人约
                items.Sort((x, y) => x.reserved.CompareTo(y.reserved));
                break;
        }

        ChaperoneController.Instance.ListView.Clear();
        foreach (var item in items)
        {
            ChaperoneController.Instance.ListView.Add(item);
        }
    }
}
