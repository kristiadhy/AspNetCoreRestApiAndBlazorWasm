using System.Security.Claims;
using System.Text.Json;

namespace Extension.Services;

//This code can be found in the Code Maze article:
//https://code-maze.com/blazor-webassembly-authentication-aspnetcore-identity/

//You can find this code originally in the Steve Sanderson’s Mission Control project.
//Please refer to this link : https://github.com/SteveSandersonMS/presentation-2019-06-NDCOslo

public static class JwtParser
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];

        var jsonBytes = ParseBase64WithoutPadding(payload);

        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        if (keyValuePairs is null)
            return [];

        ExtractRolesFromJWT(claims, keyValuePairs);

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!)));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    private static void ExtractRolesFromJWT(List<Claim> claims, Dictionary<string, object> keyValuePairs)
    {
        keyValuePairs.TryGetValue(ClaimTypes.Role, out object? roles);

        if (roles != null)
        {
            var parsedRoles = roles?.ToString()?.Trim().TrimStart('[').TrimEnd(']').Split(',');

            if (parsedRoles?.Length > 1)
            {
                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole.Trim('"')));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, parsedRoles?[0]!));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }
    }
}
