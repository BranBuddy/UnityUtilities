using System.Collections;
using UnityEngine;
public class MusicBoxController : MonoBehaviour
{
    private static MusicBoxController currentInstance;
    public static MusicBoxController CurrentInstance => currentInstance;

    [SerializeField] private string tagToActivateWith = "Player";

    [HideInInspector]
    public int audioSourceIndex;
    [HideInInspector]
    public int songIndex;
    [Tooltip("If using audio source index, this will appear filled at runtime. If assigned directly, it will override any audio source index settings.")]
    public AudioSource assignedAudioSource;
    [Tooltip("If using song index, this will appear filled at runtime. If assigned directly, it will override any song index settings.")]
    public AudioClip assignedAudioClip;

    [Space(10), Header("Fade Settings")]
    [SerializeField] private bool useFadeIn = true;
    [SerializeField] private bool useFadeOut = true;
    [SerializeField] private float fadeInDuration = 1.5f;
    [SerializeField] private float fadeOutDuration = 1.5f;

    [Space(10), Header("Debugging")]
    public bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.cyan;
    [SerializeField] private Color innerColor = new Color(0f, 1f, 1f, 0.25f);
    
    private AudioSourceInformation audioSourceInfo;

    private void Start()
    {
        StartCoroutine(WaitForSoundManagerAndInitialize());
    }

    private IEnumerator WaitForSoundManagerAndInitialize()
    {
        // Wait for SoundManager to initialize
        while (SoundManager.Instance == null)
        {
            yield return null;
        }

        FindAudioSourceInformation();
        AssignAudioSource();
    }

    private void AssignAudioSource()
    {
        if (audioSourceInfo == null)
        {
            Debug.LogError("audioSourceInfo is null. Cannot assign audio source.");
            return;
        }
        
        if (assignedAudioSource != null)
        {
            Debug.LogWarning("An AudioSource is already assigned directly. This will override the AudioSource index settings.");
            return;
        }

        assignedAudioSource = audioSourceInfo.audioSource;
    }

    private void FindAudioSourceInformation()
    {
        if (SoundManager.Instance != null && SoundManager.Instance.audioSources.Count > audioSourceIndex)
        {
            audioSourceInfo = SoundManager.Instance.audioSources[audioSourceIndex];
        }
        else
        {
            Debug.LogWarning("Invalid AudioSource index or SoundManager instance not found.");
        }
    }

    private void ActivateMusicBox()
    {

        if (audioSourceInfo != null)
        {
            Debug.Log($"audioSourceInfo: {audioSourceInfo.audioSource}, clip: {audioSourceInfo.audioClips[songIndex]}");
            if (assignedAudioClip != null)
            {
                SoundManager.Instance.PlayAudioClipLazy(assignedAudioClip, assignedAudioSource);
            }
            else
            {
                SoundManager.Instance.PlayAudioClipByIndex(audioSourceIndex, songIndex);
            }

            if (useFadeIn)
            {
                StartCoroutine(SoundManager.Instance.FadeInVolumeInAudioSource(audioSourceInfo, fadeInDuration));
            }
        }
        else
        {
            Debug.LogWarning("No valid AudioSource or AudioClip assigned to the MusicBox.");
        }
    }

    private void DeactivateMusicBox()
    {
        if (audioSourceInfo != null && useFadeOut)
        {
            StartCoroutine(SoundManager.Instance.FadeOutVolumeInAudioSource(audioSourceInfo, fadeOutDuration, stopAfterFade: true));
        }
        else
        {
            Debug.LogWarning("No valid AudioSourceInfo to fade out.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToActivateWith) && currentInstance == null)
        {
            Debug.Log("Player entered MusicBox trigger.");
            ActivateMusicBox();
            currentInstance = this;
        }
        else
        {
            Debug.Log("Non-player object entered MusicBox trigger: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagToActivateWith) && currentInstance == this)
        {
            Debug.Log("Player exited MusicBox trigger.");
            DeactivateMusicBox();
            currentInstance = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagToActivateWith) && currentInstance == null)
        {
            ActivateMusicBox();
            currentInstance = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(tagToActivateWith) && currentInstance == this)
        {
            DeactivateMusicBox();
            currentInstance = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;
        if (TryGetComponent<BoxCollider2D>(out BoxCollider2D collider2D))
        {
            Gizmos.DrawWireCube(transform.position, collider2D.size);
            Gizmos.color = innerColor;
            Gizmos.DrawCube(transform.position, collider2D.size * 1f);
        }
        else if (TryGetComponent<BoxCollider>(out BoxCollider collider3D))
        {
            Gizmos.DrawWireCube(transform.position, collider3D.size);
            Gizmos.color = innerColor;
            Gizmos.DrawCube(transform.position, collider3D.size * 1f);
        }
    }
}
