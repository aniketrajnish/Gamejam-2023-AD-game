/*#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(RaymarchRenderer))]
public class EditorState : Editor
{
    void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.quitting += OnQuitting;
    }

    void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.quitting -= OnQuitting;
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)        
            ((RaymarchRenderer)target).editorStateChange = true;        
    }

    private void OnQuitting()
    {
        ((RaymarchRenderer)target).editorStateChange = true;
    }
}
#endif
*/