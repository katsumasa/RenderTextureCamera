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
    // renderTexture:レンダリング解像度を下げたいCameraを設定
    // mainCamera : RenderTextureCameraの上にレンダリングを行うCameraを設定
    // shift : スクリーン解像度に対してshift分右へシフトさせRenderTextureのサイズを決定する


    public class RenderTextureCamera : MonoBehaviour
    {
        [Tooltip("レンダリング先をRenderTextureとするCameraを指定する")]
        // 自身のCamera
        [SerializeField] Camera renderTextureCamera;

        [Tooltip("Render TextureのベースにするCamera(2D Camera,Main Camera... etc.")]
        // mainCamera
        [SerializeField] Camera mainCamera;

        [Tooltip("RenderTextureをmainCameraへBlitするタイミング。※BeforeForwardOpaqueから変更する必要は無い")]
        [SerializeField] CameraEvent mCameraEvent = CameraEvent.BeforeForwardOpaque;

        // RenderTextureのフィルターモード
        [SerializeField][HideInInspector] FilterMode mFilterMode = FilterMode.Point;

        // オリジナルの解像度に対して何ビットシフトさせるか
        [SerializeField][HideInInspector] int mShift;

        // mShiftに変更があったかどうか
        internal bool mIsDirty = false;

        public FilterMode filterMode
        {
            get { return mFilterMode; }
            set
            {
                mFilterMode = value;
                mIsDirty = true;
            }
        }

        // RenderTextureのサイズ(size = screen size >> shift)
        public int shift
        {
            get
            {
                return mShift;
            }

            set
            {
                mShift = value;
                mIsDirty = true;
            }
        }


        // レンダリングターゲットとして使用するRenderTexture
        RenderTexture renderTexture;

        // RenderTextureの内容をmainCameraへBlitする為のコマンドバッファ
        CommandBuffer commandBuffer;


        // Start is called before the first frame update
        void Start()
        {
            if (renderTextureCamera == null)
            {
                renderTextureCamera = GetComponent<Camera>();
            }

            if (renderTextureCamera)
            {
                CreateRenderTexture();
                renderTextureCamera.targetTexture = renderTexture;
            }

            if (mainCamera)
            {
                commandBuffer = new CommandBuffer();
                commandBuffer.name = "RenderTexture Blit to MainCamera";
                commandBuffer.Blit((RenderTargetIdentifier)renderTexture, BuiltinRenderTextureType.CameraTarget);
                mainCamera.AddCommandBuffer(mCameraEvent, commandBuffer);
                mainCamera.cullingMask &= ~renderTextureCamera.cullingMask;
            }
        }


        private void Update()
        {            
            if (mIsDirty)
            {
                CreateRenderTexture();
                renderTextureCamera.targetTexture = renderTexture;
                commandBuffer.Clear();
                commandBuffer.Blit((RenderTargetIdentifier)renderTexture, BuiltinRenderTextureType.CameraTarget);
                mIsDirty = false;
            }

            if (mainCamera != null && renderTextureCamera != null)
            {
                // MainCameraからRenderTextureCameraのターゲットを除く
                mainCamera.cullingMask &= ~renderTextureCamera.cullingMask;
            }
        }


        private void CreateRenderTexture()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                renderTexture = null;
            }

            renderTexture = new RenderTexture(Screen.currentResolution.width >> shift, Screen.currentResolution.height >> shift, 24);
            renderTexture.useMipMap = false;
            renderTexture.filterMode = mFilterMode;
            renderTexture.useDynamicScale = renderTextureCamera.allowDynamicResolution;
            renderTexture.Create();
        }


        private void OnDestroy()
        {
            if (renderTexture)
            {
                renderTexture.Release();
                renderTexture = null;
            }
            if (mainCamera)
            {
                mainCamera.RemoveCommandBuffer(mCameraEvent, commandBuffer);
                if (commandBuffer != null)
                {
                    commandBuffer.Clear();
                }
            }
        }


    }
}