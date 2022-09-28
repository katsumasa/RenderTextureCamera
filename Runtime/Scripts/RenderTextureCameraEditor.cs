#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.IO;
using System.Globalization;

namespace UTJ.RenderTextureCamera
{
    [CustomEditor(typeof(RenderTextureCamera))]
    public class RenderTextureCameraEditor : Editor
    {
        static class Styles
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var renderTextureCamera = target as RenderTextureCamera;

            if(renderTextureCamera.renderTextureCamera == null)
            {
                EditorGUILayout.HelpBox("RenderTextureCameraが設定されていません。",MessageType.Error);
            }
            if(renderTextureCamera.blitCamera == null)
            {
                EditorGUILayout.HelpBox("BlitCameraが設定されていません。", MessageType.Error);
            }
            if(renderTextureCamera.renderTextureCamera != null && renderTextureCamera.blitCamera != null)
            {
                if((renderTextureCamera.renderTextureCamera.cullingMask & renderTextureCamera.blitCamera.cullingMask) != 0)
                {
                    EditorGUILayout.HelpBox($"{renderTextureCamera.blitCamera.name}と{renderTextureCamera.renderTextureCamera}のcullingMaskに一致するレイヤーが含まれています。レンダリング対象が重複しないようにcullingMaskの設定を見直して下さい。", MessageType.Error);
                }
            }

            EditorGUI.BeginChangeCheck();
            renderTextureCamera.shift = EditorGUILayout.IntSlider("Shit", renderTextureCamera.shift, 0, 6);
            renderTextureCamera.filterMode = (FilterMode)EditorGUILayout.EnumPopup("FilterMode", renderTextureCamera.filterMode);

            if (EditorGUI.EndChangeCheck())
            {
                renderTextureCamera.m_IsDirty = true;
            }
        }
    }
}
#endif
