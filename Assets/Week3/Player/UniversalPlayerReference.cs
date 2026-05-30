/*
    Singleton class that allows for easy grabbing of the player
*/

using UnityEngine;

public class UniversalPlayerReference : MonoBehaviour
{
     #region Singleton Pattern
    public static UniversalPlayerReference Instance { get; private set; }

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

    [SerializeField] public GameObject player;
}
