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
    private static readonly Dictionary<EItemCategory, int> _numberOfItemsByCategory = new Dictionary<EItemCategory, int>
    {
        { EItemCategory.See, 5 }, { EItemCategory.Hear, 4 }, { EItemCategory.Smell, 3 }, { EItemCategory.Taste, 2 },
        { EItemCategory.Touch, 1 }
    };

    public static int GetNumberOfItemsInCategory(this EItemCategory itemCategory)
    {
        return _numberOfItemsByCategory[itemCategory];
    }
}