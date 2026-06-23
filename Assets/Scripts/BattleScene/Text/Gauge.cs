using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image frame;

    public void SetGauge(CharacterBase character)
    {
        int current, max = 0;

        if (this.gameObject.name == "HPGauge")
        {
            current = character.GetHP();
            max = character.GetMaxHP();

            if (max <= 0) return;

            float ratio = current / max;

            frame.fillAmount = Mathf.Clamp01(ratio);
        }
        else if (this.gameObject.name == "MPGauge")
        {
            current = character.GetMP();
            max = character.GetMaxMP();

            if (max <= 0) return;

            float ratio = current / max;

            frame.fillAmount = Mathf.Clamp01(ratio);
        }
    }
}

