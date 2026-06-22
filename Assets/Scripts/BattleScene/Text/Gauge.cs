using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gauge : MonoBehaviour
{
    public enum GaugeType { HP, MP }
    public GaugeType gaugeType;

    private UnityEngine.UI.Slider slider;

    [SerializeField]
    private StatusText m_StatusText;

    private void Start()
    {
        slider = GetComponent<UnityEngine.UI.Slider>();

        

        if (slider != null)
        {
            slider.maxValue = (gaugeType == GaugeType.HP) ? m_StatusText.GetHP() : m_StatusText.GetMP();
        }
    }

    private void Update()
    {
        if (slider == null)
        {
            // ”O‚Ì‚½‚ßA‚à‚¤ˆê“xæ“¾‚ğ‚İ‚é
            slider = GetComponent<UnityEngine.UI.Slider>();
            if (slider == null) return;
        }

        if (gaugeType == GaugeType.HP)
        {
            slider.value = m_StatusText.GetHP();
        }
        else
        {
            slider.value = m_StatusText.GetMP();
        }
    }

}

