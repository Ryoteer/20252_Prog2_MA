using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    #region Instance
    public static AudioManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Parameters
    [Header("<color=orange>Audio</color>")]
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private string _masterGroupName = "Master";
    [Range(0.0f, 1.0f)][SerializeField] private float _masterGroupVolume = 1.0f;
    public float MasterGroupVolume { get { return _masterGroupVolume; } }
    [SerializeField] private string _musicGroupName = "Music";
    [Range(0.0f, 1.0f)][SerializeField] private float _musicGroupVolume = 0.5f;
    public float MusicGroupVolume { get { return _musicGroupVolume; } }
    [SerializeField] private string _sfxGroupName = "SFX";
    [Range(0.0f, 1.0f)][SerializeField] private float _sfxGroupVolume = 1.0f;
    public float SfxGroupVolume { get { return _sfxGroupVolume; } }
    [SerializeField] private string _uiGroupName = "UI";
    [Range(0.0f, 1.0f)][SerializeField] private float _uiGroupVolume = 1.0f;
    public float UiGroupVolume { get { return _uiGroupVolume; } }

    private AudioSource _source;
    #endregion

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.outputAudioMixerGroup = _mixer.FindMatchingGroups("Master/Music")[0];

        SetMasterVolume(_masterGroupVolume);
        SetMusicVolume(_musicGroupVolume);
        SetSFXVolume(_sfxGroupVolume);
        SetUIVolume(_uiGroupVolume);
    }

    public void SetMasterVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;

        _mixer.SetFloat(_masterGroupName, Mathf.Log10(value) * 20.0f);
    }

    public void SetMusicVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;

        _mixer.SetFloat(_musicGroupName, Mathf.Log10(value) * 20.0f);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;

        _mixer.SetFloat(_sfxGroupName, Mathf.Log10(value) * 20.0f);
    }

    public void SetUIVolume(float value)
    {
        if (value <= 0.0f) value = 0.0001f;

        _mixer.SetFloat(_uiGroupName, Mathf.Log10(value) * 20.0f);
    }

    public void PlayAudioClip(AudioClip clip)
    {
        if (_source.clip && _source.clip == clip) return;

        if (_source.isPlaying)
        {
            _source.Stop();
        }

        _source.clip = clip;

        _source.Play();
    }
}
