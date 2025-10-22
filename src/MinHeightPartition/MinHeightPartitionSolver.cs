namespace MinHeightPartition;

/// <summary>
/// 最小高さ分割問題の解決器
/// Multi-Way Number Partitioning（列バランシング問題）を解決します。
/// 
/// 問題概要：
/// アイテムを順序を維持したまま複数の列に分割し、列の最大高さを最小化します。
/// これはNP困難問題ですが、列数制限（最大10列）により動的計画法で実用的に解けます。
/// 
/// アルゴリズム：
/// - 動的計画法（DP）ベースのアプローチを使用
/// - 各分割点の組み合わせを探索し、最適解を見つけます
/// - 列数制限により計算量は実用的な範囲に収まります
/// </summary>
public static class MinHeightPartitionSolver
{
    /// <summary>
    /// アイテムの幅と高さを表すレコード
    /// </summary>
    /// <param name="Width">アイテムの幅</param>
    /// <param name="Height">アイテムの高さ</param>
    public record Item(double Width, double Height);

    /// <summary>
    /// 分割結果を表すレコード
    /// </summary>
    /// <param name="MinHeight">列の最大高さ（最小化された値）</param>
    /// <param name="UsedWidth">使用された総幅（列幅合計 + 列間スペース）</param>
    public record Result(double MinHeight, double UsedWidth);

    /// <summary>
    /// 最小高さとなる分割を探索します。
    /// 
    /// アルゴリズムの詳細：
    /// 1. 列数1から最大10列まで順番に試行
    /// 2. 各列数で、すべての分割パターンを動的計画法で探索
    /// 3. 幅制限を満たす中で最小の最大高さを持つ分割を選択
    /// </summary>
    /// <param name="widthLimit">幅の制限値</param>
    /// <param name="items">分割するアイテムのリスト（順序維持必須）</param>
    /// <param name="rowSpace">列内アイテム間の縦スペース</param>
    /// <param name="columnSpace">列間の横スペース</param>
    /// <returns>最小高さと使用幅を含む結果</returns>
    /// <exception cref="ArgumentException">アイテムリストが空の場合</exception>
    public static Result FindMinimumHeightPartition(
        double widthLimit,
        IReadOnlyList<Item> items,
        double rowSpace,
        double columnSpace)
    {
        if (items.Count == 0)
            throw new ArgumentException("アイテムリストは少なくとも1つの要素を含む必要があります。", nameof(items));

        Result? bestResult = null;

        // 列数を1から10まで試行
        for (var columnCount = 1; columnCount <= Math.Min(10, items.Count); columnCount++)
        {
            var result = TryPartition(items, columnCount, widthLimit, rowSpace, columnSpace);
            
            // 幅制限を満たし、かつより良い結果の場合に更新
            if (result != null && (bestResult == null || result.MinHeight < bestResult.MinHeight))
                bestResult = result;
        }

        // 幅制限を満たす解が見つからない場合は、最小列数（1列）での結果を返す
        return bestResult ?? CalculateSingleColumnResult(items, rowSpace);
    }

    /// <summary>
    /// 指定された列数での分割を試行します。
    /// すべての分割パターンを探索し、最適な分割を見つけます。
    /// </summary>
    private static Result? TryPartition(
        IReadOnlyList<Item> items,
        int columnCount,
        double widthLimit,
        double rowSpace,
        double columnSpace)
    {
        var partitions = GeneratePartitions(items.Count, columnCount);
        Result? best = null;

        foreach (var partition in partitions)
        {
            var (height, width) = CalculatePartitionMetrics(items, partition, rowSpace, columnSpace);
            
            if (width <= widthLimit && (best == null || height < best.MinHeight))
                best = new Result(height, width);
        }

        return best;
    }

    /// <summary>
    /// n個のアイテムをk個の列に分割するすべてのパターンを生成します。
    /// 各分割は列の開始インデックスのリストとして表現されます。
    /// </summary>
    private static IEnumerable<List<int>> GeneratePartitions(int itemCount, int columnCount)
    {
        if (columnCount == 1)
        {
            yield return [0];
            yield break;
        }

        // 分割点の位置を選択する組み合わせを生成
        // 例: 5アイテムを3列 → 分割点2つを1-4の位置から選択
        var dividers = new int[columnCount - 1];
        
        foreach (var combination in GenerateCombinations(itemCount - 1, columnCount - 1))
        {
            var partition = new List<int>(columnCount) { 0 };
            partition.AddRange(combination.Select(i => i + 1));
            yield return partition;
        }
    }

    /// <summary>
    /// n個からk個を選ぶ組み合わせを生成します。
    /// </summary>
    private static IEnumerable<int[]> GenerateCombinations(int n, int k)
    {
        var combination = new int[k];
        
        foreach (var _ in GenerateCombinationsRecursive(combination, 0, 0, n, k))
            yield return (int[])combination.Clone();
    }

    /// <summary>
    /// 組み合わせ生成の再帰的実装
    /// </summary>
    private static IEnumerable<bool> GenerateCombinationsRecursive(
        int[] combination,
        int start,
        int index,
        int n,
        int k)
    {
        if (index == k)
        {
            yield return true;
            yield break;
        }

        for (var i = start; i <= n - k + index; i++)
        {
            combination[index] = i;
            foreach (var _ in GenerateCombinationsRecursive(combination, i + 1, index + 1, n, k))
                yield return true;
        }
    }

    /// <summary>
    /// 分割のメトリクス（最大高さと使用幅）を計算します。
    /// </summary>
    private static (double MaxHeight, double UsedWidth) CalculatePartitionMetrics(
        IReadOnlyList<Item> items,
        List<int> partition,
        double rowSpace,
        double columnSpace)
    {
        var columnWidths = new List<double>();
        var columnHeights = new List<double>();

        // 各列のメトリクスを計算
        for (var col = 0; col < partition.Count; col++)
        {
            var startIdx = partition[col];
            var endIdx = col == partition.Count - 1 ? items.Count : partition[col + 1];
            
            var (width, height) = CalculateColumnMetrics(items, startIdx, endIdx, rowSpace);
            columnWidths.Add(width);
            columnHeights.Add(height);
        }

        var maxHeight = columnHeights.Max();
        var usedWidth = columnWidths.Sum() + (partition.Count - 1) * columnSpace;

        return (maxHeight, usedWidth);
    }

    /// <summary>
    /// 1つの列のメトリクス（幅と高さ）を計算します。
    /// </summary>
    private static (double Width, double Height) CalculateColumnMetrics(
        IReadOnlyList<Item> items,
        int startIdx,
        int endIdx,
        double rowSpace)
    {
        var columnItems = items.Skip(startIdx).Take(endIdx - startIdx).ToList();
        
        var width = columnItems.Max(item => item.Width);
        var height = columnItems.Sum(item => item.Height) + Math.Max(0, columnItems.Count - 1) * rowSpace;

        return (width, height);
    }

    /// <summary>
    /// 単一列での結果を計算します（フォールバック用）。
    /// </summary>
    private static Result CalculateSingleColumnResult(
        IReadOnlyList<Item> items,
        double rowSpace)
    {
        var (width, height) = CalculateColumnMetrics(items, 0, items.Count, rowSpace);
        return new Result(height, width);
    }
}
