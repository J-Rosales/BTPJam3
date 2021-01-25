using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentTooltipControl : MonoBehaviour
{
    public CanvasGroup tooltipCanvas;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI row1Label;
    public TextMeshProUGUI row2Label;
    public TextMeshProUGUI descriptionLabel;

    public void SetVisibility(bool value)
    {
        tooltipCanvas.alpha = value ? 1 : 0;
    }

    public void SetLabels(
            string itemName, string text1,
            string text2, string description)
    {
        nameLabel.text = itemName;
        row1Label.text = text1;
        row2Label.text = text2;
        descriptionLabel.text = description;
    }
}
