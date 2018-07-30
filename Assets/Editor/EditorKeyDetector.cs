 using UnityEngine;
 using UnityEditor;
[CustomEditor(typeof(EditorControllerLive))]
[CanEditMultipleObjects]
public class EditorKeyDetector : Editor {
    void OnSceneGUI() {
        EditorControllerLive script = (EditorControllerLive)target;
        Event e = Event.current;
        switch (e.type) {
            case EventType.KeyDown:
                Debug.Log("Key Detected");
                script.UpdateLive(e);
                break;
        }
    }
}
