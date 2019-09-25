# PBL2019_Robotics
ここでは、どうやってVSでGithubを利用してソースを管理するのを簡単に説明します。

简单写一下怎么在VS中使用Github管理项目。
## Installation
まず、Github機能を拡張するパーツをダウンロードしてインストールします。（インストールする時VSは閉じらなければなりません）

下载VS的Github插件并安装。（安装时需关闭VS）
- Github extension for Visual Studio: [Homepage](https://visualstudio.github.com/)
## Clone repository to local
VSをオープンして、`コードなしで続行`を選びます。右側の`チームエクスプローラー`をクリックして`GitHub`にログインします。

打开VS，选择`继续但无需代码`，在右侧`团队资源管理器`窗口中找到`GitHub`，点击左下角`连接...`，用Github账户登录。

`複製`を選んで、リポジトリに入れば`nzy142625/PBL2019_Robotics`が出てきます。パスを適当にして`複製`をクリックすればローカルにリポジトリを複製し始まります。

选择`克隆`，如果成功加入项目组此时会在列表中看到`nzy142625/PBL2019_Robotics`，选择合适的本地路径后点`克隆`就可以把项目复制到本地了。
## Add reference
ソリューションを開いたら、参照コンポーネントが見つからないメッセージが出てきます。改めて追加する必要があります。

打开项目后会发现引用缺失，需要重新添加，分为两步来操作。
### Add NuGet Package
まず右側の`ソリューションエクスプローラー`中の`ソリューション'PBL2019_Robotics'`に右クリックして`NuGetパッケージの復元`を選びます。

在右侧`解决方案资源管理器`中`PBL2019_Robotics`上右键，选择`还原 NuGet 程序包`。
### Add BeautoRover
次に前と同じように`BeautoRover.dll`をソリューションのディレクトリにコピーして参照を追加すれば良いです。

跟之前一样，只要把`BeautoRover.dll`文件复制到项目目录中，并添加引用即可。

### Just in case
念のためですが、もしUECのネットを使う場合、Github extensionがGitHubにつなげないなら、下記の方法をやってみてください。

このパスを開きます。（インストール場所によりますが）

    C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\mingw32\etc
    
`gitconfig`を開いて、`[http]`の行の下に

    proxy = http://proxy.uec.ac.jp:8080
    
を追加すると使えるようになるかもしれません。
  
