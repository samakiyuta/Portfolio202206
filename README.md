# Portfolio202206
自主制作ゲームのポートフォリオ

## Overview
自主制作ゲームの中で使用されているスクリプトの一部をポートフォリオとして公開したもの。

## Description
### Enemy/AICharacterControl.cs
敵がプレイヤーを追いかけている状態を制御するクラス。

### Enemy/Capturer.cs
敵がプレイヤーを検知して、追跡するかどうかを判定するクラス。

### Environment/FocusableObject/FocusableObject.cs
プレイヤーがフォーカス可能なオブジェクトの基底クラス。

### Environment/FocusableObject/FocusableObjectsContainer.cs
プレイヤーがフォーカスする対象となったFocusableObjectを格納するクラス。

### Environment/FocusableObject/FocusEventDispatcher.cs
プレイヤーがFocusableObjectにフォーカスしたことを検知して、FocusableObjectに伝えるクラス。

### Environment/Drawer.cs
FocusableObjectの派生クラス。引き出しと引き出しの中のアイテムを制御するクラス。

### Environment/FireDoorSwitch.cs
FocusableObjectの派生クラス。防火戸のスイッチを制御するクラス

### Environment/Key.cs
FocusableObjectの派生クラス。鍵となるアイテムのクラス。

### Environment/PeepingDoor.cs
覗き窓を制御するクラス。

### Environment/Switchboard.cs
FocusableObjectの派生クラス。分電盤を制御するクラス。

### Environment/DeathCamera.cs
死亡シーンのカメラを制御するクラス。

### UI/ActionButton.cs
アクションボタンの制御クラス。

### UI/Inventory.cs
インベントリの制御クラス。
