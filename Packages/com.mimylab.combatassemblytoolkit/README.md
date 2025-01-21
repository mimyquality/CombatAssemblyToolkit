# Combat Assembly Toolkit

## 概要

## 導入手順

### リポジトリーのインポート

VCC(VRChat Creator Companion)または [ALCOM](https://vrc-get.anatawa12.com/ja/alcom/) をインストール済みの場合、以下の**どちらか一つ**の手順を行うことでMimyLabのリポジトリーをインポートできます。  
(ALCOM の場合、適宜読み替えてください。)  

> [!NOTE]
> リポジトリーが既にインポート済みの場合、この手順をスキップできます。[VPMパッケージのインポート](#vpmパッケージのインポート)へ進んでください。

- <https://vpm.mimylab.com/> へアクセスし、「Add to VCC」から`https://vpm.mimylab.com/index.json`を追加
- VCCのウィンドウで`Setting - Packages - Add Repository`の順に開き、`https://vpm.mimylab.com/index.json`を追加

[VPM CLI](https://vcc.docs.vrchat.com/vpm/cli/)を使用してインポートする場合、コマンドラインを開き以下のコマンドを入力してください。

```text
vpm add repo https://vpm.mimylab.com/index.json
```

### VPMパッケージのインポート

VCC から任意のプロジェクトを選択し、「Manage Project」から Manage Packages 画面に移動します。  
読み込んだパッケージが一覧に出てくるので、 **Combat Assembly Toolkit** の右にある「＋」ボタンを押すか「Installed Version」から直接バージョンを選ぶことで、プロジェクトにインポートします。  

リポジトリーを使わずに導入したい場合は、[Release](https://github.com/mimyquality/CombatAssemblyToolkit/releases) から unitypackage ファイルをダウンロードして、プロジェクトにインポートしてください。  

## ライセンス

[LICENSE](LICENSE.md)

## 更新履歴

[CHANGELOG](CHANGELOG.md)
