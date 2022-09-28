# RenderTextureCamera

<img width="800" alt="d96cf94a72c254422bbb572cace0e7ad" src="https://user-images.githubusercontent.com/29646672/138978362-9483df9c-a29f-4c8e-9971-ef9267df881d.gif">

## 概要

UIなどの2Dのレンダリング解像度はそのままに、3Dのレンダリング解像度を下げることで、見た目のクオリティを維持したまま、処理負荷を軽減することが期待できます。
本コンポーネントは特定のカメラのレンダリング解像度を動的に変更する事が可能です。

## Dynamic Resolutionとの比較

Unity2018以降のバージョンでは[Dynamic Resolution](https://docs.unity3d.com/ja/2018.4/Manual/DynamicResolution.html)が実装されています。
Dynamic Resolutionはプラットフォーム及びグラフィックAPIに依存しますが、Dynamic Resolutionを使用出来る場合、そちらを使用した方が解像度切り替え時のオーバーヘッドが低いと思われます。
Dynamic Resolutionの使用サンプルは[こちら](https://github.com/katsumasa/SimpleSampleDynamicResolution)になります。

## 使用環境

- Unity2017.4以降
- Android端末で確認を行っていますが、基本全てのプラットフォームで動作する筈です。問題のあるプラットフォームやグラフィックAPIがありましたら、フィードバックをお待ちしております。

## 使い方

1. レンダリング解像度を変更する為のCameraを生成します。（既に描画対象毎にCameraが別れている場合は、新たに生成する必要はありません。）
2. このCameraが何をレンダリングするかを明示的に指定する為に[cullingMask](https://docs.unity3d.com/ja/current/ScriptReference/Camera-cullingMask.html)を設定します。
3. このCamera Objectへ[RenderTextureCamera Component](https://github.com/katsumasa/RenderTextureCamera/blob/master/Runtime/Scripts/RenderTextureCamera.cs)をにADDして下さい。
4. RenderTextureCamer Componentの`renderTextureCamera` プロパティへ1で生成したCamera Objectを設定します。
5. 同じく`blitCamera`プロパティへRenderTexure Cameraでレンダリングした内容を描画したいCameraを設定します。
6. `blitCamera`プロパティへ設定したCamera Componentの`cullingMask`から2で設定したレイヤーを除外して下さい。
7. `shift`プロパティや`filterMode`プロパティで解像度やフィルターモードを必要に応じて指定します。

### ヒント

既に3Dオブジェクトと2DオブジェクトでCameraが別れているプロジェクトで3Dのレンダリング解像度を下げたい場合、3Dオブジェクト用のCameraにRenderTextureCamera ComponentをADDし、`renderTextureCamera`プロパティへ3Dオブジェクトのレンダリング用Camera、`blitCameraプロパティへ2Dオブジェクトレンダリング用のCameraを設定します。

## プロパティ

### renderTextureCamera

レンダリング解像度を下げたいカメラを指定します。
3Dをレンダリングするカメラを想定しています。

### blitCamera

renderTextureCameraの上にレンダリングを行うカメラを指定します。
UIなど2Dをレンダリングするカメラを想定しています。

### cameraEvent

renderTextureCameraでレンダリングした内容をmainCameraへBlitするタイミングを指定します。
基本的に`Before Forward Opaque`から変更する必要はありません。

### shift

RenderTextureのサイズを指定する為に使用します。
0の場合、ScreenSizeのままとなります。

```:
RenderTextureのサイズ = Screenサイズ >> shift 
```

#### filterMode

RenderTextureのフィルターモードを指定します。

※下記のサンプル表示は全てレンダリング解像度は`shift=5`に設定しています、

##### Point

![image](https://user-images.githubusercontent.com/29646672/138979993-a1b9bdd8-4938-4e4a-be5a-2ebf59213b06.png)

##### Bilinear

![image](https://user-images.githubusercontent.com/29646672/138980101-763dea0f-309f-4738-be30-8d6aceb29c1c.png)

##### Trilinear

![image](https://user-images.githubusercontent.com/29646672/138980170-97e67d4b-99cd-4e6b-9f85-599df7f7161e.png)

## その他

フィードバックをお待ちしております。

以上❣
