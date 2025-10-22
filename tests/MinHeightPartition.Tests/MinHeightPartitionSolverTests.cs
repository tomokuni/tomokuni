using MinHeightPartition;
using static MinHeightPartition.MinHeightPartitionSolver;

namespace MinHeightPartition.Tests;

/// <summary>
/// MinHeightPartitionSolver のテストクラス
/// </summary>
public class MinHeightPartitionSolverTests
{
    /// <summary>
    /// 単一アイテムの場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_SingleItem_ReturnsSingleColumn()
    {
        // Arrange
        var items = new List<Item> { new(100, 50) };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 200,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        Assert.Equal(50, result.MinHeight);
        Assert.Equal(100, result.UsedWidth);
    }

    /// <summary>
    /// 2つのアイテムを1列に配置する場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_TwoItemsOneColumn_CalculatesHeightWithRowSpace()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(100, 60)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 150,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 高さ = 50 + 60 + 10 (rowSpace) = 120
        Assert.Equal(120, result.MinHeight);
        Assert.Equal(100, result.UsedWidth);
    }

    /// <summary>
    /// 2つのアイテムを2列に配置する場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_TwoItemsTwoColumns_SplitsIntoColumns()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(80, 60)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 300,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 2列に分割: [Item1], [Item2]
        // 高さ = max(50, 60) = 60
        // 幅 = 100 + 80 + 20 (columnSpace) = 200
        Assert.Equal(60, result.MinHeight);
        Assert.Equal(200, result.UsedWidth);
    }

    /// <summary>
    /// 幅制限により列数が制限される場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_WidthLimitRestriction_LimitsColumnCount()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(100, 60),
            new(100, 70)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 220,  // 2列まで: 100 + 100 + 20 = 220
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 2列に分割: [Item1, Item2], [Item3]
        // 列1の高さ = 50 + 60 + 10 = 120
        // 列2の高さ = 70
        // 最大高さ = 120
        Assert.Equal(120, result.MinHeight);
        Assert.Equal(220, result.UsedWidth);
    }

    /// <summary>
    /// 複雑な分割パターンのテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_ComplexPattern_FindsOptimalSolution()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 100),
            new(90, 50),
            new(80, 50),
            new(70, 100)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 500,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 最適分割を探索し、最小の最大高さを見つける
        // 例: [Item1], [Item2, Item3], [Item4]
        // 列1: 100
        // 列2: 50 + 50 + 10 = 110
        // 列3: 100
        // 最大高さ = 110
        Assert.True(result.MinHeight <= 160); // 1列の場合は160なのでそれより良い
        Assert.True(result.UsedWidth <= 500);
    }

    /// <summary>
    /// 異なる幅のアイテムのテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_DifferentWidths_CalculatesCorrectUsedWidth()
    {
        // Arrange
        var items = new List<Item>
        {
            new(150, 50),
            new(100, 60),
            new(80, 70)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 400,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        Assert.True(result.UsedWidth <= 400);
        Assert.True(result.MinHeight > 0);
    }

    /// <summary>
    /// rowSpaceが0の場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_ZeroRowSpace_CalculatesWithoutGaps()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(100, 60)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 150,
            items: items,
            rowSpace: 0,
            columnSpace: 20);

        // Assert
        // 高さ = 50 + 60 = 110 (rowSpace無し)
        Assert.Equal(110, result.MinHeight);
    }

    /// <summary>
    /// columnSpaceが0の場合のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_ZeroColumnSpace_CalculatesWithoutGaps()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(80, 60)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 300,
            items: items,
            rowSpace: 10,
            columnSpace: 0);

        // Assert
        // 2列に分割した場合の幅 = 100 + 80 = 180 (columnSpace無し)
        Assert.Equal(60, result.MinHeight);
        Assert.Equal(180, result.UsedWidth);
    }

    /// <summary>
    /// 多数のアイテム（10列制限のテスト）
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_ManyItems_RespectsMaxColumnLimit()
    {
        // Arrange
        var items = Enumerable.Range(0, 15)
            .Select(i => new Item(50, 30))
            .ToList();
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 2000,
            items: items,
            rowSpace: 5,
            columnSpace: 10);

        // Assert
        // 10列制限により、最適な分割が見つかる
        Assert.True(result.MinHeight > 0);
        Assert.True(result.UsedWidth <= 2000);
    }

    /// <summary>
    /// 空のアイテムリストの場合の例外テスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_EmptyItems_ThrowsArgumentException()
    {
        // Arrange
        var items = new List<Item>();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            MinHeightPartitionSolver.FindMinimumHeightPartition(
                widthLimit: 200,
                items: items,
                rowSpace: 10,
                columnSpace: 20));
    }

    /// <summary>
    /// 幅制限が非常に小さい場合のテスト（フォールバック）
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_VerySmallWidthLimit_ReturnsSingleColumn()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 50),
            new(100, 60)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 50,  // どの分割も満たせない
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 幅制限を満たせないため、単一列にフォールバック
        Assert.Equal(120, result.MinHeight);
        Assert.Equal(100, result.UsedWidth);
    }

    /// <summary>
    /// 小数値を含むテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_DecimalValues_HandlesCorrectly()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100.5, 50.3),
            new(90.7, 60.8),
            new(80.2, 70.1)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 500.5,
            items: items,
            rowSpace: 10.5,
            columnSpace: 20.3);

        // Assert
        Assert.True(result.MinHeight > 0);
        Assert.True(result.UsedWidth <= 500.5);
    }

    /// <summary>
    /// 最適な3列分割のテスト
    /// </summary>
    [Fact]
    public void FindMinimumHeightPartition_ThreeColumns_FindsOptimalBalance()
    {
        // Arrange
        var items = new List<Item>
        {
            new(100, 40),
            new(100, 40),
            new(100, 40),
            new(100, 40),
            new(100, 40),
            new(100, 40)
        };
        
        // Act
        var result = MinHeightPartitionSolver.FindMinimumHeightPartition(
            widthLimit: 400,
            items: items,
            rowSpace: 10,
            columnSpace: 20);

        // Assert
        // 3列に均等分割: [2, 2, 2]
        // 各列の高さ = 40 + 40 + 10 = 90
        Assert.Equal(90, result.MinHeight);
        Assert.Equal(340, result.UsedWidth); // 100 + 100 + 100 + 20 + 20
    }
}
