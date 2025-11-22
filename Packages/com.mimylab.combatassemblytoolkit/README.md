# Combat Assembly Toolkit

## 概要

## 導入手順

### リポジトリーのインポート

> [!NOTE]
> 既に MimyLab のリポジトリーをインポートしている場合はこの手順をスキップできます。[VPM パッケージのインポート](#vpmパッケージのインポート)へ進んでください。

VCC(VRChat Creator Companion)または [ALCOM](https://vrc-get.anatawa12.com/ja/alcom/) から、以下の**どちらか一つ**の手順を行うことで MimyLab のリポジトリーをインポートできます。  
(ALCOM 利用者は適宜読み替えてください)  

- <https://vpm.mimylab.com/> へアクセスし、「Add to VCC」から `https://vpm.mimylab.com/index.json` を追加
- VCC のウィンドウで `Setting -> Packages -> Add Repository` の順に開き、`https://vpm.mimylab.com/index.json` を追加

<img width="1863" height="1112" alt="image" src="https://github.com/user-attachments/assets/1608fa5e-c621-4cfe-8731-bb96bcbe9be8" />

[VPM CLI](https://vcc.docs.vrchat.com/vpm/cli/) を使用してインポートする場合、コマンドラインを開き以下のコマンドを入力してください。

```text
vpm add repo https://vpm.mimylab.com/index.json
```

### VPMパッケージのインポート

VCC から任意のプロジェクトを選択し、「Manage Project」から Manage Packages 画面に移動します。  
読み込んだパッケージが一覧に出てくるので、 **Combat Assembly Toolkit** の右にある「＋」ボタンを押すか「Installed Version」から直接バージョンを選ぶことで、プロジェクトにインポートします。  
このとき一覧に出てこない場合は、右上の [Selected Repos] から MimyLab リポジトリーのチェックが外れていないか確認してください。  

リポジトリーを使わずに導入したい場合は、[Release](https://github.com/mimyquality/CombatAssemblyToolkit/releases) から unitypackage ファイルをダウンロードして、プロジェクトにインポートしてください。  

## ライセンス

[LICENSE](LICENSE.md)

## 更新履歴

[CHANGELOG](CHANGELOG.md)
