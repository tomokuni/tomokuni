# 最小高さ分割アルゴリズム (Minimum Height Partition Algorithm)

## 問題の概要

このライブラリは、**Multi-Way Number Partitioning（多方向数分割問題）** または **Column Balancing Problem（列バランシング問題）** として知られる問題を解決します。

### 問題の定義

アイテムのリストを複数の列に分割し、各列の高さの最大値を最小化する問題です。

- **入力**:
  - `widthLimit`: 幅の制限値
  - `items`: アイテムリスト（各アイテムは幅と高さを持つ）
  - `rowSpace`: 列内のアイテム間の縦スペース
  - `columnSpace`: 列間の横スペース

- **出力**:
  - `MinHeight`: 列の最大高さ（最小化された値）
  - `UsedWidth`: 使用された総幅（列幅の合計 + 列間スペース）

- **制約**:
  - アイテムの順序は維持される（インデックス順に処理）
  - 最大列数は10
  - 使用幅は `widthLimit` 以下でなければならない
  - アイテム数は1以上

### 計算方法

- **列の高さ** = (列内アイテムの高さ合計) + (列内アイテム数 - 1) × rowSpace
- **列の幅** = 列内アイテムの幅の最大値
- **使用幅** = (列幅の合計) + (列数 - 1) × columnSpace

## アルゴリズムの説明

### 問題の分類

この問題は **NP困難** に分類されますが、以下の理由により実用的に解けます：

1. **列数制限**: 最大10列という制約により、探索空間が限定される
2. **順序維持**: アイテムの順序を維持するため、分割点の組み合わせのみを考慮すればよい

### 採用アルゴリズム

**動的計画法（Dynamic Programming）ベースの全探索**

1. **列数のイテレーション**: 1列から10列まで順番に試行
2. **分割パターンの生成**: 各列数について、すべての分割パターンを生成
   - n個のアイテムをk列に分割する場合、n-1個の位置からk-1個の分割点を選ぶ組み合わせを生成
3. **メトリクスの計算**: 各分割パターンについて、最大高さと使用幅を計算
4. **最適解の選択**: 幅制限を満たす中で、最小の最大高さを持つ分割を選択

### 計算量

- **時間計算量**: O(列数 × C(n-1, 列数-1) × n)
  - n: アイテム数
  - C(n-1, k-1): 組み合わせの数
  - 列数は最大10なので、実用的な範囲
- **空間計算量**: O(n)

## 使用方法

### 基本的な使用例

```csharp
using MinHeightPartition;
using static MinHeightPartition.MinHeightPartitionSolver;

// アイテムの作成
var items = new List<Item>
{
    new(Width: 100, Height: 50),
    new(Width: 90, Height: 60),
    new(Width: 80, Height: 70)
};

// 最小高さ分割の探索
var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 400,
    items: items,
    rowSpace: 10,
    columnSpace: 20);

Console.WriteLine($"最小高さ: {result.MinHeight}");
Console.WriteLine($"使用幅: {result.UsedWidth}");
```

### GUI操作での利用例

```csharp
// リアルタイムでのレイアウト計算
public void UpdateLayout(IReadOnlyList<Item> items, double availableWidth)
{
    var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
        widthLimit: availableWidth,
        items: items,
        rowSpace: 5,
        columnSpace: 10);
    
    // 結果を使用してUIを更新
    ApplyLayout(result);
}
```

### 複雑なシナリオ

```csharp
// 可変サイズのアイテム
var items = new List<Item>
{
    new(150, 100),  // 大きなアイテム
    new(100, 50),
    new(100, 50),
    new(80, 40),    // 小さなアイテム
    new(80, 40)
};

// スペースを考慮した最適化
var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 500,
    items: items,
    rowSpace: 15,      // アイテム間の縦スペース
    columnSpace: 25);  // 列間の横スペース

// 結果:
// - 最適な列数で分割される
// - 各列の高さがバランスされる
// - 幅制限が守られる
```

## プロジェクト構成

```
MinHeightPartition/
├── src/
│   └── MinHeightPartition/
│       ├── MinHeightPartition.csproj
│       └── MinHeightPartitionSolver.cs     # メインアルゴリズム実装
├── tests/
│   └── MinHeightPartition.Tests/
│       ├── MinHeightPartition.Tests.csproj
│       └── MinHeightPartitionSolverTests.cs # 包括的なテスト
└── MinHeightPartition.sln                   # ソリューションファイル
```

## ビルドとテスト

### ビルド

```bash
dotnet build MinHeightPartition.sln
```

### テスト実行

```bash
dotnet test MinHeightPartition.sln
```

### テストカバレッジ

以下のシナリオがテストされています：

- ✅ 単一アイテム
- ✅ 複数アイテムの単一列配置
- ✅ 複数列への分割
- ✅ 幅制限による列数制限
- ✅ 複雑な分割パターン
- ✅ 異なる幅のアイテム
- ✅ スペース値が0の場合
- ✅ 多数のアイテム（10列制限）
- ✅ 空のアイテムリスト（例外処理）
- ✅ 非常に小さい幅制限（フォールバック）
- ✅ 小数値の処理
- ✅ 最適な3列分割

## 技術的特徴

### C# 14の機能活用

- **Record型**: `Item` と `Result` の定義に使用
- **コレクション初期化子**: `[0]` などの簡潔な構文
- **LINQ**: データ処理と集計に活用
- **パターンマッチング**: `null` チェックなどで使用
- **ラムダ式**: デリゲートやクエリで使用
- **Null条件演算子**: `?.` による安全なアクセス

### コード品質

- ✅ **単一責任の原則**: 各メソッドが明確な単一機能を持つ
- ✅ **可読性**: 詳細なコメントとドキュメントコメント
- ✅ **簡潔性**: 冗長なコードの排除
- ✅ **モダンな書き方**: C# 14の最新機能を活用
- ✅ **包括的なテスト**: 13のテストケースで様々なシナリオをカバー

## パフォーマンス特性

### 適用範囲

- **最適**: アイテム数が少ない（～100個）場合
- **良好**: アイテム数が中程度（～1000個）の場合
- **GUI操作**: リアルタイム計算に十分な速度

### 最適化のポイント

1. **列数制限**: 最大10列により計算量を制限
2. **早期終了**: 幅制限を満たす最適解が見つかり次第、それ以上の探索を省略可能
3. **効率的な組み合わせ生成**: 再帰的アルゴリズムで無駄な計算を削減

## ライセンス

このコードは教育および商用利用が可能です。

## 貢献

改善提案やバグ報告は歓迎します。

## 参考文献

### 関連する問題

- **Bin Packing Problem**: 容器詰め問題
- **Multi-Processor Scheduling**: マルチプロセッサスケジューリング
- **Partition Problem**: 分割問題
- **Column Generation**: 列生成法

### 理論的背景

この問題は組み合わせ最適化問題の一種であり、以下の分野に関連します：

- **動的計画法**: 部分問題の最適解を組み合わせて全体の最適解を求める
- **NP完全性理論**: 最適解を多項式時間で見つけることは困難
- **近似アルゴリズム**: 制約により実用的な厳密解が可能
