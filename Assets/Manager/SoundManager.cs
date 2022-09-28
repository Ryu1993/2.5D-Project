using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    AudioMixer audioMixer;
    [SerializeField]
    Animator windowAnimation;
    [SerializeField]
    Transform audioWindow;
    [SerializeField]
    Slider bgmSlider;
    [SerializeField]
    Slider effectSlider;

    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }
    public void BgmVolumeChange() => audioMixer.SetFloat("Bgm", bgmSlider.value);
    public void EffectVolumeChange() => audioMixer.SetFloat("Effect", effectSlider.value);
    public void WindowPop()
    {
        Time.timeScale = 0f;
        this.gameObject.SetActive(true);
    }
    public void WindowClose() => windowAnimation.SetTrigger("Close");
    public void WindowScaleMax() => audioWindow.localScale = new Vector3(1, 1, 1);
    public void WindowScaleMin()
    {
        audioWindow.localScale = Vector3.zero;
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

}
