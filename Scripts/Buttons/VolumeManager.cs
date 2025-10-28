using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] Slider VolumeSlider;

    private void Start()
    {
        AudioListener.volume = 1f;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = VolumeSlider.value;
    }
}
