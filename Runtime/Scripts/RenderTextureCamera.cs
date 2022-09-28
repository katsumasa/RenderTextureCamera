using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace UTJ.RenderTextureCamera
{
    // レンダリング先にRenderTextureを使用し解像度を下げる
    // 使い方
    // m_RenderTextureCamera:レンダリング解像度を下げたいCameraを設定
    // mainCamera : RenderTextureCameraの上にレンダリングを行うCameraを設定
    // shift : スクリーン解像度に対してshift分右へシフトさせRenderTextureのサイズを決定する


    public class RenderTextureCamera : MonoBehaviour
    {
        [Tooltip("レンダリング解像度を下げたいCamera(レンダリング先をRenderTextureとするCamera)")]
        // 自身のCamera
        [SerializeField] Camera m_RenderTextureCamera;

        [Tooltip("RenderTextureを描画(Blit)するCamera(2D Camera,Main Camera... etc.")]
        // mainCamera
        [SerializeField] Camera m_BlitCamera;

        [Tooltip("RenderTextureをBlitCameraへBlitするタイミング。※基本的にはBeforeForwardOpaqueから変更する必要はありません")]
        [SerializeField] CameraEvent m_CameraEvent = CameraEvent.BeforeForwardOpaque;

        // RenderTextureのフィルターモード
        [SerializeField][HideInInspector] FilterMode m_FilterMode = FilterMode.Point;

        // オリジナルの解像度に対して何ビットシフトさせるか
        [SerializeField][HideInInspector] int m_Shift;

        // mShiftに変更があったかどうか
        internal bool m_IsDirty = false;

        public Camera renderTextureCamera
        {
            get { return m_RenderTextureCamera; }
        }

        public Camera blitCamera
        {
            get { return m_BlitCamera; }
        }

        public FilterMode filterMode
        {
            get { return m_FilterMode; }
            set
            {
                m_FilterMode = value;
                m_IsDirty = true;
            }
        }

        // RenderTextureのサイズ(size = screen size >> shift)
        public int shift
        {
            get
            {
                return m_Shift;
            }

            set
            {
                m_Shift = value;
                m_IsDirty = true;
            }
        }


        // レンダリングターゲットとして使用するRenderTexture
        RenderTexture m_RenderTexture;

        // RenderTextureの内容をmainCameraへBlitする為のコマンドバッファ
        CommandBuffer m_CommandBuffer;


        // Start is called before the first frame update
        void Start()
        {
            if (m_RenderTextureCamera == null)
            {
                m_RenderTextureCamera = GetComponent<Camera>();
            }

            if (m_RenderTextureCamera)
            {
                CreateRenderTexture();
                m_RenderTextureCamera.targetTexture = m_RenderTexture;
            }

            if (m_BlitCamera)
            {
                m_CommandBuffer = new CommandBuffer();
                m_CommandBuffer.name = "Blit  RenderTexture";
                m_CommandBuffer.Blit((RenderTargetIdentifier)m_RenderTexture, BuiltinRenderTextureType.CameraTarget);
                m_BlitCamera.AddCommandBuffer(m_CameraEvent, m_CommandBuffer);
                m_BlitCamera.cullingMask &= ~m_RenderTextureCamera.cullingMask;
            }
        }


        private void Update()
        {            
            if (m_IsDirty)
            {
                CreateRenderTexture();
                m_RenderTextureCamera.targetTexture = m_RenderTexture;
                m_CommandBuffer.Clear();
                m_CommandBuffer.Blit((RenderTargetIdentifier)m_RenderTexture, BuiltinRenderTextureType.CameraTarget);
                m_IsDirty = false;
            }

            if (m_BlitCamera != null && m_RenderTextureCamera != null)
            {
                // MainCameraからRenderTextureCameraのターゲットを除く
                m_BlitCamera.cullingMask &= ~m_RenderTextureCamera.cullingMask;
            }
        }


        private void CreateRenderTexture()
        {
            if (m_RenderTexture != null)
            {
                m_RenderTexture.Release();
                m_RenderTexture = null;
            }

            m_RenderTexture = new RenderTexture(Screen.currentResolution.width >> shift, Screen.currentResolution.height >> shift, 24);
            m_RenderTexture.useMipMap = false;
            m_RenderTexture.filterMode = m_FilterMode;
            m_RenderTexture.useDynamicScale = m_RenderTextureCamera.allowDynamicResolution;
            m_RenderTexture.Create();
        }


        private void OnDestroy()
        {
            if (m_RenderTexture)
            {
                m_RenderTexture.Release();
                m_RenderTexture = null;
            }
            if (m_BlitCamera)
            {
                m_BlitCamera.RemoveCommandBuffer(m_CameraEvent, m_CommandBuffer);
                if (m_CommandBuffer != null)
                {
                    m_CommandBuffer.Clear();
                }
            }
        }


    }
}