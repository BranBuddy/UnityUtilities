/*
    Master singleton holder for all UI elements
*/

using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialougeUIController : MonoBehaviour
{
    #region Singleton Pattern
    public static DialougeUIController Instance { get; private set; }

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

    [SerializeField] public GameObject dialougeContainer;
    [SerializeField] public TextMeshProUGUI dialougeText;
    [SerializeField] public GameObject responseButtonContainter;
    [SerializeField] public GameObject responseButtonPrefab;
    [SerializeField] public Image characterImage;
    [SerializeField] public TextMeshProUGUI characterName;

    public void UIVisibility(bool turnOn)
    {
        dialougeContainer.SetActive(turnOn);
    }
}
