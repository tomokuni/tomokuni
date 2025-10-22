using MinHeightPartition;
using static MinHeightPartition.MinHeightPartitionSolver;

Console.WriteLine("=== 最小高さ分割アルゴリズム デモンストレーション ===");
Console.WriteLine();

// 例1: 基本的な使用例
Console.WriteLine("【例1】基本的な2アイテムの分割");
Console.WriteLine("アイテム: (幅100, 高さ50), (幅90, 高さ60)");
Console.WriteLine("幅制限: 300, 縦スペース: 10, 横スペース: 20");

var items1 = new List<Item>
{
    new(Width: 100, Height: 50),
    new(Width: 90, Height: 60)
};

var result1 = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 300,
    items: items1,
    rowSpace: 10,
    columnSpace: 20);

Console.WriteLine($"結果: 最小高さ = {result1.MinHeight}, 使用幅 = {result1.UsedWidth}");
Console.WriteLine($"解釈: 2列に分割され、各列に1アイテムずつ配置されます");
Console.WriteLine();

// 例2: 複雑な分割パターン
Console.WriteLine("【例2】複雑な分割パターン");
Console.WriteLine("アイテム: 4つの異なるサイズのアイテム");
Console.WriteLine("幅制限: 500, 縦スペース: 10, 横スペース: 20");

var items2 = new List<Item>
{
    new(Width: 100, Height: 100),
    new(Width: 90, Height: 50),
    new(Width: 80, Height: 50),
    new(Width: 70, Height: 100)
};

var result2 = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 500,
    items: items2,
    rowSpace: 10,
    columnSpace: 20);

Console.WriteLine($"結果: 最小高さ = {result2.MinHeight}, 使用幅 = {result2.UsedWidth}");
Console.WriteLine($"解釈: アルゴリズムが最適な列数と分割を見つけます");
Console.WriteLine();

// 例3: 幅制限による制約
Console.WriteLine("【例3】幅制限による列数の制約");
Console.WriteLine("アイテム: 3つの同じサイズのアイテム (幅100, 高さ50)");
Console.WriteLine("幅制限: 220 (2列まで), 縦スペース: 10, 横スペース: 20");

var items3 = new List<Item>
{
    new(Width: 100, Height: 50),
    new(Width: 100, Height: 60),
    new(Width: 100, Height: 70)
};

var result3 = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 220,  // 2列まで: 100 + 100 + 20 = 220
    items: items3,
    rowSpace: 10,
    columnSpace: 20);

Console.WriteLine($"結果: 最小高さ = {result3.MinHeight}, 使用幅 = {result3.UsedWidth}");
Console.WriteLine($"解釈: 幅制限により2列に制限され、[Item1, Item2], [Item3]と分割されます");
Console.WriteLine();

// 例4: 多数のアイテムの均等分割
Console.WriteLine("【例4】多数のアイテムの均等分割");
Console.WriteLine("アイテム: 12個の同サイズアイテム (幅50, 高さ30)");
Console.WriteLine("幅制限: 2000, 縦スペース: 5, 横スペース: 10");

var items4 = Enumerable.Range(0, 12)
    .Select(i => new Item(Width: 50, Height: 30))
    .ToList();

var result4 = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 2000,
    items: items4,
    rowSpace: 5,
    columnSpace: 10);

Console.WriteLine($"結果: 最小高さ = {result4.MinHeight}, 使用幅 = {result4.UsedWidth}");
Console.WriteLine($"解釈: 12個のアイテムが複数列に均等に分割されます");
Console.WriteLine();

// 例5: GUI レイアウトシミュレーション
Console.WriteLine("【例5】GUIレイアウトシミュレーション");
Console.WriteLine("シナリオ: ツールボックスのボタンを最小の高さで配置");

var toolButtons = new List<Item>
{
    new(Width: 80, Height: 40),   // 「新規作成」ボタン
    new(Width: 80, Height: 40),   // 「開く」ボタン
    new(Width: 80, Height: 40),   // 「保存」ボタン
    new(Width: 80, Height: 60),   // 「印刷」ボタン（大きめ）
    new(Width: 80, Height: 40),   // 「コピー」ボタン
    new(Width: 80, Height: 40),   // 「貼り付け」ボタン
    new(Width: 80, Height: 40),   // 「切り取り」ボタン
    new(Width: 80, Height: 40)    // 「削除」ボタン
};

var layoutResult = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 350,  // ツールボックスの幅制限
    items: toolButtons,
    rowSpace: 8,      // ボタン間のスペース
    columnSpace: 12); // 列間のスペース

Console.WriteLine($"結果: 最小高さ = {layoutResult.MinHeight}, 使用幅 = {layoutResult.UsedWidth}");
Console.WriteLine($"解釈: ツールボックスが幅{layoutResult.UsedWidth}で高さ{layoutResult.MinHeight}のレイアウトになります");
Console.WriteLine();

// 例6: 異なる幅のアイテム
Console.WriteLine("【例6】異なる幅のアイテムの最適配置");
Console.WriteLine("アイテム: (150,50), (100,60), (80,70), (60,40), (120,55)");
Console.WriteLine("幅制限: 400, 縦スペース: 10, 横スペース: 15");

var items6 = new List<Item>
{
    new(Width: 150, Height: 50),
    new(Width: 100, Height: 60),
    new(Width: 80, Height: 70),
    new(Width: 60, Height: 40),
    new(Width: 120, Height: 55)
};

var result6 = MinHeightPartitionSolver.FindMinimumHeightPartition(
    widthLimit: 400,
    items: items6,
    rowSpace: 10,
    columnSpace: 15);

Console.WriteLine($"結果: 最小高さ = {result6.MinHeight}, 使用幅 = {result6.UsedWidth}");
Console.WriteLine($"解釈: 各列の幅は列内の最大アイテム幅で決まります");
Console.WriteLine();

Console.WriteLine("=== デモンストレーション完了 ===");
Console.WriteLine();
Console.WriteLine("このアルゴリズムは以下のシーンで活用できます：");
Console.WriteLine("- GUIツールボックスのレイアウト最適化");
Console.WriteLine("- ダッシュボードウィジェットの配置");
Console.WriteLine("- 印刷レイアウトの段組み最適化");
Console.WriteLine("- レスポンシブデザインの列数決定");
Console.WriteLine("- タイルレイアウトの高さ最小化");
