# RenderTextureCamera

<img width="800" alt="d96cf94a72c254422bbb572cace0e7ad" src="https://user-images.githubusercontent.com/29646672/138978362-9483df9c-a29f-4c8e-9971-ef9267df881d.gif">

## Summary

This component allows you to change the rendering resolution of particular camera dynamically.
You can maintain the load process and 2D rendering resolution (such as UI) by decreasing the 3D rendering resolution.


## Comparison with Dynamic Resolution

[Dynamic Resolution](https://docs.unity3d.com/ja/2018.4/Manual/DynamicResolution.html)has been implemented in Unity version 2018 and onwards.
Dynamic Resolution depends on the platfrom and graphics API, but it would be better to use that instead since it has less overhead when switching resolutions.
[Here](https://github.com/katsumasa/SimpleSampleDynamicResolution)is the sampl data for Dynamic Resolution.

## Operating Environment

- Version Unity2017.4 and onward
- We tested it only on Android devices, but it should work on all platforms. If there are problems in any platforms or graphics APIs, please let us know. We would appreciate any feedback.

## How to use

ADD [RenderTextureCamera.cs](https://github.com/katsumasa/RenderTextureCamera/blob/master/Runtime/Scripts/RenderTextureCamera.cs)to the Camera Object you wish to change resolution. 

### renderTextureCamera

Specify the camera you wish to reduce the rendering resolution.
Imagine if this is a camera that'll render in 3D.

### mainCamera

Specify tge canera ti rebder on top of renderTextureCamera.
It's for camera that renders 2D element such as UI.

### cameraEvent

Specify the timing to Blit the contents rendered by renderTextureCamera to the mainCamera.
There is no need to change from `Before Forward Opaque`

### shift

Used to specify the size of the RenderTexture.
If the number is set to 0, it remains ScreenSize.

```
RenderTexture Size = Screen Size >> shift 
```

#### filterMode

Specify the filter mode for RenderTexture.

※The resolution of following samples are set to render in`shift=5`

#### Point

![image](https://user-images.githubusercontent.com/29646672/138979993-a1b9bdd8-4938-4e4a-be5a-2ebf59213b06.png)

#### Bilinear

![image](https://user-images.githubusercontent.com/29646672/138980101-763dea0f-309f-4738-be30-8d6aceb29c1c.png)


#### Trilinear

![image](https://user-images.githubusercontent.com/29646672/138980170-97e67d4b-99cd-4e6b-9f85-599df7f7161e.png)

## Other

Appreciate your comments and feedback.

Tha'll be all❣
