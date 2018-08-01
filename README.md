# ChatBotSample
Azure Bot FrameworkとQnA Maker Serviceを用いて作成したChatBotのサンプルコード(C#版)です。Visual Studioを用いず、Azure App Service Editor上でコードをコピー&ペーストするだけで動かすことが出来ます。

# Azure Bot Serviceの作成

1. Azureポータルでリソースの作成から**Web App Bot**をデプロイしてください。この際、ボットテンプレートを選択できますが、**Question and Answer(C#)** をお選びください。

<a href="https://imgur.com/3eyj8uB"><img src="https://i.imgur.com/3eyj8uB.png" title="source: imgur.com" /></a>

2. デプロイが完了したら、Web App Botをお開きください。

3. Web App Botを開いたら、画面左側のリストから**アプリケーション設定**を選択してください。

<a href="https://imgur.com/7HL2hCV"><img src="https://i.imgur.com/7HL2hCV.png" title="source: imgur.com" /></a>

# キーの設定

1. AzureポータルのWeb App Botのアプリケーション設定の以下の欄に、**QnA Makerポータルで発行された3種類のキー** (QnAAuthKey, QnAEndpointHostName, QnAKnowledgebaseId)を貼り付けてください。

<a href="https://imgur.com/fCyBe4z"><img src="https://i.imgur.com/fCyBe4z.png" title="source: imgur.com" /></a>

<a href="https://imgur.com/7ALidSR"><img src="https://i.imgur.com/7ALidSR.png" title="source: imgur.com" /></a>

2. 入力が終わりましたら、画面上部の**保存**をクリックしてください。



# ソースコードの編集

1. Azureポータル上で、Web App Botと共にデプロイされた**App Service** をお開きください。

2. 画面左側のリストから、**App Service Editor (プレビュー)** を選択して頂き、**移動** をクリックしてください(ブラウザで別ウィンドウが開きます)。

<a href="https://imgur.com/xNjbowM"><img src="https://i.imgur.com/xNjbowM.png" title="source: imgur.com" /></a>

3. App Service Editorの画面にて、画面左側に複数個のフォルダとファイルがあるかと思います。ここで、**Dialogs** の左側にある▷をクリックして頂き、**BasicQnAMakerDialog.cs** を開いてください。

<a href="https://imgur.com/4fNUoW1"><img src="https://i.imgur.com/4fNUoW1.png" title="source: imgur.com" /></a>

4. BasicQnAMakerDialog.csに書かれているコードを全て消し、リポジトリのコードに書き換えてください(変更内容は自動で保存されます)。

5. 次に、**Global.asax.cs** を編集します。こちらも既存で書かれているコードを全て消し、リポジトリに上がっているコードに書き換えてください(変更内容は自動で保存されます)。ここに書かれているコードによって、App Service上にChatLogを保存することができるようになります。

6. コードの編集が完了したら、画面左側の下から二番目のボタンをクリックしてください。遷移した画面で**build.cmd**と入力し、エンターを押してください。これによって、ソースコードのコンパイルが始まります。コンパイルが正常に終了すると、以下のような画面が見られるかと思います。

<a href="https://imgur.com/DtGOV9C"><img src="https://i.imgur.com/DtGOV9C.png" title="source: imgur.com" /></a>

# 動作確認

1. Azureポータル上でWeb App Botを開いて頂き、画面左側のリストから**Webチャットでテスト**を選択してください。ここで、ChatBotの動作確認をして頂けます。

<a href="https://imgur.com/QzcD42Q"><img src="https://i.imgur.com/QzcD42Q.png" title="source: imgur.com" /></a>

