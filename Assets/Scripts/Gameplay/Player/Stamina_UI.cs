using UnityEngine;
using UnityEngine.UI;

public class Stamina_UI : MonoBehaviour
{
    [SerializeField]
    private PlayerSystems _playerSystems;

    [SerializeField]
    private Slider _staminaSlider;

    private void Start()
    {
        _staminaSlider.maxValue = _playerSystems.CurrentStamina;

        _playerSystems.OnTakeStamina.AddListener(CanvasUpateHealth);
        CanvasUpateHealth();
    }

    private void OnDestroy()
    {
        _playerSystems.OnTakeStamina.RemoveListener(CanvasUpateHealth);
    }

    public void CanvasUpateHealth()
    {
        _staminaSlider.value = _playerSystems.CurrentStamina;
    }
}