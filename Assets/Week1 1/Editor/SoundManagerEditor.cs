using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    private int lastCount = 0;

    private void OnEnable()
    {
        SoundManager manager = (SoundManager)target;
        if (manager.audioSources != null)
        {
            lastCount = manager.audioSources.Count;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SoundManager manager = (SoundManager)target;
        if (manager.audioSources != null)
        {
            if (manager.audioSources.Count > lastCount)
            {
                // New elements added
                for (int i = lastCount; i < manager.audioSources.Count; i++)
                {
                    manager.audioSources[i].EnsureDefaultValuesSet();
                }
            }
            lastCount = manager.audioSources.Count;
        }
    }
}