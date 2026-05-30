using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CreateMusicBox : MonoBehaviour
{
    [Header("Music Box Type")]
    public MusicBoxType musicBoxType;

    [Header("Box Size Settings")]
    [SerializeField] internal Vector2 boxSize2D = new Vector2(1f, 1f);
    [SerializeField] internal Vector3 boxSize3D = new Vector3(1f, 1f, 1f);

    [Header("Using SoundManager, grab the index of the AudioSource and the song you want to play")]
    [SerializeField] private int AudioSourceIndex = 0;
    [SerializeField] private int songIndex = 0;

    [Header("Optional: Assign an AudioSource and AudioClip directly")]
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip songClip = null;

    public void SpawnMusicBox()
    {
        GameObject musicBox = new GameObject("MusicBox");
        musicBox.transform.position = transform.position;

        if (musicBoxType == MusicBoxType.Type2D)
        {
            BoxCollider2D collider2D = musicBox.AddComponent<BoxCollider2D>();
            Rigidbody2D rb2D = musicBox.AddComponent<Rigidbody2D>();
            rb2D.bodyType = RigidbodyType2D.Kinematic;
            rb2D.gravityScale = 0f;
            collider2D.size = boxSize2D;
            collider2D.isTrigger = true;
        }
        else if (musicBoxType == MusicBoxType.Type3D)
        {
            BoxCollider collider3D = musicBox.AddComponent<BoxCollider>();
            Rigidbody rb3D = musicBox.AddComponent<Rigidbody>();
            rb3D.isKinematic = true;
            rb3D.useGravity = false;
            collider3D.size = boxSize3D;
            collider3D.isTrigger = true;
        }

        MusicBoxController controller = musicBox.AddComponent<MusicBoxController>();
        if (controller == null)
        {
            Debug.LogError("Failed to add MusicBoxController to MusicBox. Check that the MusicBoxController script compiles and the class name matches the file name.");
            return;
        }

        controller.audioSourceIndex = AudioSourceIndex;
        controller.songIndex = songIndex;
        controller.assignedAudioSource = audioSource;
        controller.assignedAudioClip = songClip;

    }
}

public enum MusicBoxType
{
    Type2D,
    Type3D
}

#if UNITY_EDITOR
[CustomEditor(typeof(CreateMusicBox))]
public class CreateMusicBoxEditor : Editor
{
    SerializedProperty musicBoxTypeProp;
    SerializedProperty boxSize2DProp;
    SerializedProperty boxSize3DProp;

    void OnEnable()
    {
        musicBoxTypeProp = serializedObject.FindProperty("musicBoxType");
        boxSize2DProp = serializedObject.FindProperty("boxSize2D");
        boxSize3DProp = serializedObject.FindProperty("boxSize3D");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(musicBoxTypeProp);

        MusicBoxType musicBoxType = (MusicBoxType)musicBoxTypeProp.enumValueIndex;

        EditorGUILayout.Space(10);

        if (musicBoxType == MusicBoxType.Type2D)
        {
            EditorGUILayout.PropertyField(boxSize2DProp, new GUIContent("Box Size 2D"));
        }
        else if (musicBoxType == MusicBoxType.Type3D)
        {
            EditorGUILayout.PropertyField(boxSize3DProp, new GUIContent("Box Size 3D"));
        }

        EditorGUILayout.Space(10);
        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AudioSourceIndex"), new GUIContent("AudioSource Index"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("songIndex"), new GUIContent("Song Index"));

        EditorGUILayout.Space(10);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("audioSource"), new GUIContent("Assigned AudioSource"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("songClip"), new GUIContent("Assigned AudioClip"));

        EditorGUILayout.Space(10);
    
        if (GUILayout.Button("Spawn Music Box"))
        {
            CreateMusicBox creator = (CreateMusicBox)target;
            creator.SpawnMusicBox();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
