# RenderTextureCamera

<img width="800" alt="d96cf94a72c254422bbb572cace0e7ad" src="https://user-images.githubusercontent.com/29646672/138978362-9483df9c-a29f-4c8e-9971-ef9267df881d.gif">

## 概要

特定のカメラのレンダリング解像度を下げる事が出来るClassです。
UIなどの2Dのレンダリング解像度はそのままに、3Dのレンダリング解像度を下げることで、見た目のクオリティを維持したまま、処理負荷を軽減することが可能です。

## Dynamic Resolutionとの比較

Unity2018以降のバージョンでは[Dynamic Resolution](https://docs.unity3d.com/ja/2018.4/Manual/DynamicResolution.html)が実装されています。
Dynamic Resolutionはプラットフォーム及びグラフィックAPIに依存しますが、Dynamic Resolutionを使用出来る場合、そちらを使用した方が解像度切り替え時のオーバーヘッドが低いと思われます。
Dynamic Resolutionの使用サンプルは[こちら](https://github.com/katsumasa/SimpleSampleDynamicResolution)になります。

## 使用環境

- Unity2017.4以降
- Android端末で確認を行っていますが、基本全てのプラットフォームで動作する筈です。

## 使い方

[RenderTextureCamera.cs](https://github.com/katsumasa/RenderTextureCamera/blob/master/Runtime/Scripts/RenderTextureCamera.cs)をレンダリング解像度を変更したいCameraのGameObjectにADDして下さい。

### renderTextureCamera

レンダリング解像度を下げたいカメラを指定します。
3Dをレンダリングするカメラを想定しています。

### mainCamera

renderTextureCameraの上にレンダリングを行うカメラを指定します。
UIなど2Dをレンダリングするカメラを想定しています。

### cameraEvent

renderTextureCameraでレンダリングした内容をmainCameraへBlitするタイミングを指定します。
基本的に`Before Forward Opaque`から変更する必要はありません。

### shift

RenderTextureのサイズを指定する為に使用します。
0の場合、ScreenSizeのままとなります。

```
RenderTextureのサイズ = Screenサイズ >> shift 
```

#### filterMode

RenderTextureのフィルターモードを指定します。

※下記のサンプル表示は全てレンダリング解像度は`shift=5`に設定しています、

#### Point

![image](https://user-images.githubusercontent.com/29646672/138979993-a1b9bdd8-4938-4e4a-be5a-2ebf59213b06.png)

#### Bilinear

![image](https://user-images.githubusercontent.com/29646672/138980101-763dea0f-309f-4738-be30-8d6aceb29c1c.png)


#### Trilinear

![image](https://user-images.githubusercontent.com/29646672/138980170-97e67d4b-99cd-4e6b-9f85-599df7f7161e.png)
