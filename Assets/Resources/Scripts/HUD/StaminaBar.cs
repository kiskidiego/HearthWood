using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private Image _staminaFill;
    private StaminaComponent _staminaComponent;
    void Awake()
    {
        _staminaFill = GetComponent<Image>();
        _staminaComponent = FindAnyObjectByType<StaminaComponent>();
    }

    void OnEnable()
    {
        _staminaComponent.OnStaminaChanged += UpdateStaminaBar;
    }
    void OnDisable()
    {
        _staminaComponent.OnStaminaChanged -= UpdateStaminaBar;
    }

    void UpdateStaminaBar(StaminaComponent.StaminaChangeInfo info)
    {
        _staminaFill.fillAmount = info.CurrentStamina / info.MaxStamina;
    }
}