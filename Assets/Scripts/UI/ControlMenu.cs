using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlMenu : MonoBehaviour
{
    [SerializeField]
    private Slider aimSlider;
    [SerializeField]
    private Slider gravSlider;

    [SerializeField]
    private Text aimNumber;
    [SerializeField]
    private Text gravNumber;

    private void OnEnable()
    {
        UpdateAimText();
        UpdateGravText();
    }

    public void UpdateAimText()
    {
        aimNumber.text = aimSlider.value.ToString();
    }

    public void UpdateGravText()
    {
        gravNumber.text = gravSlider.value.ToString();
    }
}
