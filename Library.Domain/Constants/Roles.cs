namespace Library.Domain.Constants;

public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Librarian = "Librarian";
    public const string Staff = "Staff";

    public static readonly string[] All = { Administrator, Librarian, Staff };
}