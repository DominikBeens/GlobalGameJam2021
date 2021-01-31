using UnityEngine;
using UnityEditor;

public class DebugWindow : EditorWindow {

    [MenuItem("Tools/Debug Window")]
    static void Initialize() {
        DebugWindow window = (DebugWindow)GetWindow(typeof(DebugWindow));
        window.Show();
    }

    private void OnGUI() {
        if (GUILayout.Button("Mark selection dirty")) {
            Object[] objects = Selection.objects;
            foreach (Object obj in objects) {
                EditorUtility.SetDirty(obj);
            }
        }

        if (GUILayout.Button("Save project")) {
            AssetDatabase.SaveAssets();
        }

    }
}