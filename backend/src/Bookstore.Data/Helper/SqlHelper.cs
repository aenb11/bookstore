namespace Bookstore.Data.Helper;

public static class SqlHelper
{
    public const string NotDeleted = "is_deleted = false";

    public const string IsDefault = "is_default = true";

    public static string EnumIn<TEnum>(string column)
        where TEnum : struct, Enum =>
        $"{column} IN ('{string.Join("','", Enum.GetNames<TEnum>())}')";
}
