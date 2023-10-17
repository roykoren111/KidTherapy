using System.Collections.Generic;

public enum EItemCategory
{
    See,
    Hear,
    Smell,
    Taste,
    Touch,
}

public static class ItemCategoryExtensions
{
    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToSelectByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 5 }, { EItemCategory.Hear, 4 }, { EItemCategory.Smell, 3 }, { EItemCategory.Taste, 2 },
            { EItemCategory.Touch, 1 }
        };

    private static readonly Dictionary<EItemCategory, int> _numberOfItemsToAppearByCategory =
        new Dictionary<EItemCategory, int>
        {
            { EItemCategory.See, 8 }, { EItemCategory.Hear, 8 }, { EItemCategory.Smell, 8 }, { EItemCategory.Taste, 8 },
            { EItemCategory.Touch, 8 }
        };

    public static int GetNumberOfItemsToSelect(this EItemCategory itemCategory)
    {
        return _numberOfItemsToSelectByCategory[itemCategory];
    }

    public static int GetNumberOfItemsToAppear(this EItemCategory itemCategory)
    {
        return _numberOfItemsToAppearByCategory[itemCategory];
    }
}