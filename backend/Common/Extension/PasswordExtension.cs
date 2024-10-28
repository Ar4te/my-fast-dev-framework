using System.Text.RegularExpressions;

namespace Common.Extension;

public static class PasswordExtension
{
    public static string GetPasswordHash(this Password password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(this Password password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}

public struct Password
{
    private readonly string _value;

    public Password(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw new ArgumentException("Password value cannot be null or empty.");
        }
        if (!IsValid(value))
        {
            throw new ArgumentException("Password value is Invalid.");
        }
        _value = value;
    }

    private static bool IsValid(string value)
    {
        if (value.Length < 8)
        {
            return false;
        }

        int matchCount = 0;
        var hasUpperCase = new Regex(@"[A-Z]");
        var hasLowerCase = new Regex(@"[a-z]");
        var hasDigit = new Regex(@"[0-9]");
        var hasSpecialChar = new Regex(@"[^A-Za-z0-9]");

        if (hasUpperCase.IsMatch(value)) matchCount++;
        if (hasLowerCase.IsMatch(value)) matchCount++;
        if (hasDigit.IsMatch(value)) matchCount++;
        if (hasSpecialChar.IsMatch(value)) matchCount++;

        return matchCount >= 3;
    }

    public override string ToString()
    {
        return _value;
    }

    public static implicit operator string(Password password) => password._value;
    public static implicit operator Password(string value) => new(value);
}