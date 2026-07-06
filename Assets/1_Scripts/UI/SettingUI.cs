using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider bgmVolumeSlider;
    public Slider sfxVolumeSlider;
    public Text masterVolumeText;
    public Text bgmVolumeText;
    public Text sfxVolumeText;
    public Button masterMuteButton;
    public Button bgmMuteButton;
    public Button sfxMuteButton;
    public Image[] muteOnImages = new Image[3];
    public Image[] muteOffImage = new Image[3];

    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        bgmVolumeSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        masterMuteButton.onClick.AddListener(() =>
        {
            OnMuteButtonClicked(SoundType.Master);
            MuteCheck(SoundType.Master);
        });
        bgmMuteButton.onClick.AddListener(() =>
        {
            OnMuteButtonClicked(SoundType.BGM);
            MuteCheck(SoundType.BGM);
        });
        sfxMuteButton.onClick.AddListener(() =>
        {
            OnMuteButtonClicked(SoundType.SFX);
            MuteCheck(SoundType.SFX);
        });
    }

    public void Start()
    {
        gameObject.SetActive(false);
    }

    private void MuteCheck(SoundType type)
    {
        if (type == SoundType.Master)
        {
            if (SoundManager.Instance.MuteCheck(SoundType.Master))
            {
                muteOnImages[0].gameObject.SetActive(false);
                muteOffImage[0].gameObject.SetActive(true);
            }
            else
            {
                muteOnImages[0].gameObject.SetActive(true);
                muteOffImage[0].gameObject.SetActive(false);
            }
        }else if (type == SoundType.BGM)
        {
            if (SoundManager.Instance.MuteCheck(SoundType.BGM))
            {
                muteOnImages[1].gameObject.SetActive(false);
                muteOffImage[1].gameObject.SetActive(true);
            }
            else
            {
                muteOnImages[1].gameObject.SetActive(true);
                muteOffImage[1].gameObject.SetActive(false);
            }
        }else if (type == SoundType.SFX)
        {
            if (SoundManager.Instance.MuteCheck(SoundType.SFX))
            {
                muteOnImages[2].gameObject.SetActive(false);
                muteOffImage[2].gameObject.SetActive(true);
            }
            else
            {
                muteOnImages[2].gameObject.SetActive(true);
                muteOffImage[2].gameObject.SetActive(false);
            }
        }
    }

    private void OnMasterVolumeChanged(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundType.Master, value);
        muteOnImages[0].gameObject.SetActive(true);
        muteOffImage[0].gameObject.SetActive(false);
        masterVolumeText.text = $"{(value * 100f):0}%";
    }

    private void OnBGMVolumeChanged(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundType.BGM, value);
        muteOnImages[1].gameObject.SetActive(true);
        muteOffImage[1].gameObject.SetActive(false);
        bgmVolumeText.text = $"{(value * 100f):0}%";
    }

    private void OnSFXVolumeChanged(float value)
    {
        SoundManager.Instance.ChangeVolume(SoundType.SFX, value);
        muteOnImages[2].gameObject.SetActive(true);
        muteOffImage[2].gameObject.SetActive(false);
        sfxVolumeText.text = $"{(value * 100f):0}%";
    }

    private void OnMuteButtonClicked(SoundType type)
    {
        SoundManager.Instance.Mute(type);
    }
}
