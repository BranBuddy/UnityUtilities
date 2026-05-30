using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;

[System.Serializable]
public class AudioSourceInformation
{
    public AudioSource audioSource;
    public string name; // name for the audio source
    public List<AudioClip> audioClips; // All the audio clips for this audio source

    [Header("Default Audio Source Configuration")]

    [Tooltip("Default volume for the audio source (0.0 to 1.0)"), Range(0f, 1f), SerializeField]
    public float volume; // default volume for the audio source
    [Tooltip("Default loop setting for the audio source"), SerializeField]
    public bool loop; // default loop setting for the audio source
    [Tooltip("Default play on awake setting for the audio source"), SerializeField]
    public bool playOnAwake; // default play on awake setting for the audio source
    [Tooltip("Default pitch for the audio source (0.1 to 3.0)"), Range(0.1f, 3f), SerializeField]
    public float pitch; // default pitch for the audio source
    [Tooltip("Default priority for the audio source (0 to 256)"), Range(0, 256), SerializeField]
    public int priority; // default priority for the audio source
    [Tooltip("Default spatial blend for the audio source (0.0 for 2D, 1.0 for 3D)"), Range(0f, 1f), SerializeField]
    public float spatialBlend; // default spatial blend for the audio source
    [Tooltip("Default reverb zone mix for the audio source (0.0 to 1.1)"), Range(0f, 1.1f), SerializeField]
    public float reverbZoneMix; // default reverb zone mix for the audio source
    [Tooltip("Default doppler level for the audio source (0.0 to 5.0)"), Range(0f, 5f), SerializeField]
    public float dopplerLevel; // default doppler level for the audio source
    [Tooltip("Default stereo pan for the audio source (-1.0 for left, 1.0 for right)"), Range(-1f, 1f), SerializeField]
    public float stereoPan; // default stereo pan for the audio source

    internal Dictionary<int, AudioClip> audioClipDictionary; // Dictionary to store audio clips with their corresponding indices
    
    public void InitializeAudioClipDictionary()
    {
        audioClipDictionary = new Dictionary<int, AudioClip>();
        for (int i = 0; i < audioClips.Count; i++)
        {
            audioClipDictionary.Add(i, audioClips[i]);
        }
    }

    public void EnsureDefaultValuesSet()
    {
        volume = 1;
        loop = false;
        playOnAwake = false;
        pitch = 1;
        priority = 128;
        spatialBlend = 0;
        reverbZoneMix = 1;
        dopplerLevel = 1;
        stereoPan = 0;
    }

    public AudioSourceInformation ()
    {
        EnsureDefaultValuesSet();
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.playOnAwake = playOnAwake;
            audioSource.pitch = pitch;
            audioSource.priority = priority;
            audioSource.spatialBlend = spatialBlend;
            audioSource.reverbZoneMix = reverbZoneMix;
            audioSource.dopplerLevel = dopplerLevel;
            audioSource.panStereo = stereoPan;
        }
    }

    public void AssignValuesToAudioSource()
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.playOnAwake = playOnAwake;
            audioSource.pitch = pitch;
            audioSource.priority = priority;
            audioSource.spatialBlend = spatialBlend;
            audioSource.reverbZoneMix = reverbZoneMix;
            audioSource.dopplerLevel = dopplerLevel;
            audioSource.panStereo = stereoPan;
        }
    }
}

public class SoundManager : MonoBehaviour
{
    public List<AudioSourceInformation> audioSources;

    [SerializeField] private bool useDefaultValuesForAll = false;

    #region Singleton Pattern
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
    
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }
    #endregion


    private void Start()
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            audioSourceInfo.InitializeAudioClipDictionary();
            audioSourceInfo.AssignValuesToAudioSource();
        }

        if (useDefaultValuesForAll)
        {
            foreach (AudioSourceInformation info in audioSources)
            {
                info.EnsureDefaultValuesSet();
                Debug.Log("Defaulting");
            }
        }
    }
    
    #region Playing Audio Clips by Index
    public void PlayAudioClipByIndex(int audioSourceIndex, int audioClipIndex)
    {
        Debug.Log($"Attempting to play audio clip at index {audioClipIndex} from audio source at index {audioSourceIndex}");

        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        AudioSourceInformation audioSourceInfo = audioSources[audioSourceIndex];
        
        if (!audioSourceInfo.audioClipDictionary.ContainsKey(audioClipIndex))
        {
            Debug.LogError("Audio clip index out of range for the specified audio source.");
            return;
        }

        AudioClip clipToPlay = audioSourceInfo.audioClipDictionary[audioClipIndex];
        audioSourceInfo.audioSource.clip = clipToPlay;
        audioSourceInfo.audioSource.Play();
        Debug.Log($"Playing clip '{clipToPlay.name}' from audio source '{audioSourceInfo.name}'");
    }

    public void PlayOneShotClipByIndex(int audioSourceIndex, int audioClipIndex)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        AudioSourceInformation audioSourceInfo = audioSources[audioSourceIndex];
        
        if (!audioSourceInfo.audioClipDictionary.ContainsKey(audioClipIndex))
        {
            Debug.LogError("Audio clip index out of range for the specified audio source.");
            return;
        }

        AudioClip clipToPlay = audioSourceInfo.audioClipDictionary[audioClipIndex];
        audioSourceInfo.audioSource.volume = audioSourceInfo.volume;
        audioSourceInfo.audioSource.PlayOneShot(clipToPlay);
    }
    #endregion

    #region Playing Audio Clips by Reference
    public void PlayAudioClipLazy(AudioClip audioClip, AudioSource audioSource)
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            if (audioSourceInfo.audioSource == audioSource)
            {
                audioSourceInfo.audioSource.clip = audioClip;
                audioSourceInfo.audioSource.Play();
                return;
            }
        }
        Debug.LogError("Audio source not found in the audio sources list.");
    }

    public void PlayOneShotClipLazy(AudioClip audioClip, AudioSource audioSource)
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            if (audioSourceInfo.audioSource == audioSource)
            {
                audioSourceInfo.audioSource.PlayOneShot(audioClip);
                return;
            }
        }
        Debug.LogError("Audio source not found in the audio sources list.");
    }

    #endregion

    #region Stopping and Pausing/Unpausing Audio Sources
    public void StopAudioSource(int audioSourceIndex)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        audioSources[audioSourceIndex].audioSource.Stop();
    }

    public void PauseAudioSource(int audioSourceIndex)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        audioSources[audioSourceIndex].audioSource.Pause();
    }

    public void StopAllAudioSources()
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            audioSourceInfo.audioSource.Stop();
        }
    }

    public void PauseAllAudioSources()
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            audioSourceInfo.audioSource.Pause();
        }
    }

    public void UnPauseAllAudioSources()
    {
        foreach (AudioSourceInformation audioSourceInfo in audioSources)
        {
            audioSourceInfo.audioSource.UnPause();
        }
    }

    public void UnPauseAudioSource(int audioSourceIndex)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        audioSources[audioSourceIndex].audioSource.UnPause();
    }
    #endregion

    #region Adjusting Volume and Looping
    public void SetAudioSourceVolume(int audioSourceIndex, float volume)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        audioSources[audioSourceIndex].audioSource.volume = volume;
        audioSources[audioSourceIndex].volume = volume; // Update the default volume in the AudioSourceInformation
    }

    public void SetAudioSourceLoop(int audioSourceIndex, bool loop)
    {
        if (audioSourceIndex < 0 || audioSourceIndex >= audioSources.Count)
        {
            Debug.LogError("Audio source index out of range.");
            return;
        }

        audioSources[audioSourceIndex].audioSource.loop = loop;
        audioSources[audioSourceIndex].loop = loop; // Update the default loop setting in the AudioSourceInformation
    }

    #endregion

    #region Fading Audio Sources
    public IEnumerator FadeOutVolumeInAudioSource(AudioSourceInformation audioSourceInfo, float fadeDuration, bool stopAfterFade = false)
    {
        Debug.Log($"FadeOut started for {audioSourceInfo.name}, starting volume: {audioSourceInfo.audioSource.volume}");
        float startVolume = audioSourceInfo.audioSource.volume;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSourceInfo.audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        audioSourceInfo.audioSource.volume = 0f; // Ensure volume is set to 0 at the end of the fade

        yield return null; // Wait a frame to ensure the volume change takes effect before stopping

        if (stopAfterFade)
        {
            audioSourceInfo.audioSource.Stop();
            audioSourceInfo.audioSource.volume = startVolume; // Reset volume to original value for future use
        }
    }

    public IEnumerator FadeInVolumeInAudioSource(AudioSourceInformation audioSourceInfo, float fadeDuration)
    {
        float targetVolume = audioSourceInfo.volume; // Use the default volume from AudioSourceInformation
        audioSourceInfo.audioSource.volume = 0f; // Start with volume at 0
        float elapsedTime = 0f;
        
        Debug.Log($"FadeIn started for {audioSourceInfo.name}, target volume: {targetVolume}");

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            audioSourceInfo.audioSource.volume = Mathf.Lerp(0f, targetVolume, elapsedTime / fadeDuration);
            yield return null;
        }

        audioSourceInfo.audioSource.volume = targetVolume; // Ensure volume is set to target value at the end of the fade
        Debug.Log($"FadeIn completed for {audioSourceInfo.name}");
    }
    #endregion

    #region Helper Methods
    public void UpdateAllAudioSourceValues(AudioSourceInformation audioSourceInfo)
    {
        audioSourceInfo.audioSource.volume = audioSourceInfo.volume;
        audioSourceInfo.audioSource.loop = audioSourceInfo.loop;
        audioSourceInfo.audioSource.playOnAwake = audioSourceInfo.playOnAwake;
        audioSourceInfo.audioSource.pitch = audioSourceInfo.pitch;
        audioSourceInfo.audioSource.priority = audioSourceInfo.priority;
        audioSourceInfo.audioSource.spatialBlend = audioSourceInfo.spatialBlend;
        audioSourceInfo.audioSource.reverbZoneMix = audioSourceInfo.reverbZoneMix;
        audioSourceInfo.audioSource.dopplerLevel = audioSourceInfo.dopplerLevel;
        audioSourceInfo.audioSource.panStereo = audioSourceInfo.stereoPan;
    }
    #endregion
}
