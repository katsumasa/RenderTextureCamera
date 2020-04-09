using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RenderTextureCamera : MonoBehaviour
{
    // 自身のCamera
    [SerializeField] Camera renderTextureCamera;
    
    // mainCamera
    [SerializeField] Camera mainCamera;

    // オリジナルの解像度に対して何ビットシフトさせるか
    [SerializeField] int shift = 1;

    // レンダリングターゲットとして使用するRenderTexture
    RenderTexture renderTexture;

    // RenderTextureの内容をmainCameraへBlitする為のコマンドバッファ
    CommandBuffer commandBuffer;


    // Start is called before the first frame update
    void Start()
    {
        if (renderTextureCamera)
        {
            renderTexture = new RenderTexture(Screen.currentResolution.width >> shift, Screen.currentResolution.height >> shift, 24);
            renderTexture.useMipMap = false;
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.useDynamicScale = renderTextureCamera.allowDynamicResolution;
            renderTexture.Create();
            renderTextureCamera.targetTexture = renderTexture;

            // MainCameraの前にレンダリング
            // renderTextureCamera.depth = mainCamera.depth - 1;
            // RenderCameraのレンダリング対象(UI以外は全てレンダリング)
            renderTextureCamera.cullingMask = ~(1 << LayerMask.NameToLayer("UI"));


        }
        if (mainCamera)
        {
            commandBuffer = new CommandBuffer();
            commandBuffer.name = "RenderTexture Blit to MainCamera";
            commandBuffer.Blit((RenderTargetIdentifier)renderTexture, BuiltinRenderTextureType.CameraTarget);
            mainCamera.AddCommandBuffer(CameraEvent.AfterEverything, commandBuffer);

            // Main CameraはUIのみ
            mainCamera.clearFlags = CameraClearFlags.Depth;
            mainCamera.cullingMask = 1 << LayerMask.NameToLayer("UI");
        }
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
            mainCamera.RemoveCommandBuffer(CameraEvent.AfterEverything, commandBuffer);
            if (commandBuffer != null)
            {
                commandBuffer.Clear();
            }
        }
    }


}
