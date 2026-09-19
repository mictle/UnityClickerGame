# CookieClickerTest

Unity 6000.3.24f1で作成した、2Dクリックゲームの検証プロジェクトです。

## ローカル開発

1. Unity 6000.3.24f1でプロジェクトを開く。
2. `Assets/Scenes/SampleScene.unity`を開く。
3. Unity Pipelineが有効なEditorでは、CodexからEditorを操作できます。
4. WebGL確認時は`Tools > Build > WebGL Release`を実行します。

生成先は`BuildWebGL/`です。Unity標準のWebGL成果物はfile://ではなくHTTPサーバー経由で確認してください。

## GitHub Actions

Pull RequestではWebGLビルドとArtifact生成を行います。`main`へのpushではビルド後にGitHub Pagesへ公開します。

GitHubリポジトリのSecretsに、利用するUnity CI認証情報を登録してください。

- `UNITY_EMAIL`
- `UNITY_PASSWORD`
- `UNITY_SERIAL`

リポジトリのSettings > Pages > Build and deploymentで、SourceをGitHub Actionsに設定します。

公開後のURLは通常、次の形式です。

`https://<ユーザー名>.github.io/<リポジトリ名>/`

## 注意

- `Library/`、`Temp/`、`Logs/`、`BuildWebGL/`などの生成物はGit管理しません。
- Unityライセンス情報をファイルへコミットしないでください。
- GitHub Pagesは静的ホスティングのため、オンラインセーブやランキングのバックエンドは別途必要です。
