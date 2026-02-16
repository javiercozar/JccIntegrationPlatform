namespace Jcc.Commons.Platform.Contracts.Search;

public sealed record Occupancy(
    int Adults,
    int[]? ChildrenAges = null
);