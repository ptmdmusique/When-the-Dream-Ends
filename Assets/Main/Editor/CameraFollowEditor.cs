using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera2DFollowNormal))]
public class CameraFollowEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        DrawDefaultInspector();

        Camera2DFollowNormal cameraFollow = (Camera2DFollowNormal)target;
        if (GUILayout.Button("Set Min Cam Pos") == true) {
            cameraFollow.SetMinCamPosition();
        }

        if (GUILayout.Button("Set Max Cam Pos") == true) {
            cameraFollow.SetMaxCamPosition();
        }
    }
}
