# csharp_ouyoukadai （C#応用課題）

github  
https://github.com/zappays/csharp_ouyoukadai

## タスク管理システム

基本的なCRUD機能を備えたTodoアプリ のようなもの

## 開発環境
* Microsoft Visual Studio Community 2022 Version 17.3.5
* C# 10.0
* .NET 6.0.9
* MySQL 8.0.30
---
## 起動前準備
### データベース接続準備（MySQLのみ有効）
1. appsettings.jsonフォルダに接続先DBの情報を記載する（先にMySQLでログインユーザ、パスワード、データベースを作成しておく）

2. コマンドプロンプトを起動しMySQLにログイン（例）
```
mysql -ushinoza -ppppp ouyou
```

3. ログイン後、テーブルを削除
```
DROP TABLE IF EXISTS user, taskitem, status, priority, auth, __efmigrationshistory;
```

4. Migrationsフォルダがあればフォルダごと削除

5. マイグレーションを実行　--Visual Studioの左下のウィンドウにある「パッケージマネージャーコンソール(PM)」からコマンドを入力
```
Add-Migration InitialCreateMySQLDB
```

6. マイグレーションで作成されたファイルでデータベースを更新　--「パッケージマネージャーコンソール(PM)」からコマンドを実行
```
Update-Database
```

## 起動
Visual Studioの上部にある緑色の ▶Ouyoukadai を押下でアプリケーションが起動する

(起動時にSeedDataが投入される)

SeedData（初期データ）が入っているのでそのユーザを利用してログインする

* Id : 1
* Pass : aaaa

※テストデータが大量に入った初期データで始めたい場合 **Program.cs** の **SeedData** を **SeedData2** に変更して[ 3. ログイン後、テーブルを削除 ]から再度実行する

---

### テーブル定義書
![](./提出物/%E3%83%86%E3%83%BC%E3%83%96%E3%83%AB%E5%AE%9A%E7%BE%A9%E6%9B%B8.drawio.svg)

### ER図
![](./提出物/ER%E5%9B%B3.drawio.svg)

### 画面遷移図
![](./提出物/%E7%94%BB%E9%9D%A2%E9%81%B7%E7%A7%BB%E5%9B%B3.drawio.svg)