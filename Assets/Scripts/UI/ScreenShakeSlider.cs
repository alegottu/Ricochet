using UnityEngine;
using UnityEngine.UI;

public class ScreenShakeSlider : MonoBehaviour
{
	[SerializeField] private Slider slider = null;

	private void Awake()
	{
		GameStateManager.cameraShakeDamp = slider.value;
	}

    public void OnValueChanged()
    {
		GameStateManager.cameraShakeDamp = slider.value;
    }   
}
