#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;


namespace UTJ.RenderTextureCamera
{
    [CustomEditor(typeof(RenderTextureCamera))]
    public class RenderTextureCameraEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var renderTextureCamera = target as RenderTextureCamera;

            EditorGUI.BeginChangeCheck();
            renderTextureCamera.mShift = EditorGUILayout.IntSlider("Shit",renderTextureCamera.shift, 0, 6);
            renderTextureCamera.mFilterMode = (FilterMode)EditorGUILayout.EnumPopup("FilterMode",renderTextureCamera.mFilterMode);

            if (EditorGUI.EndChangeCheck())
            {
                renderTextureCamera.mIsDirty = true;
            }
        }
    }
}
#endif
