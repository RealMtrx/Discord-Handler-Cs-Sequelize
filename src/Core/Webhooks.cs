using System.Text.Json;

namespace DiscordHandler.Core;

public class WebhookField
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool Inline { get; set; }
}

public class WebhookFooter
{
    public string Text { get; set; } = string.Empty;
}

public class WebhookThumbnail
{
    public string Url { get; set; } = string.Empty;
}

public class WebhookEmbed
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Color { get; set; }
    public List<WebhookField> Fields { get; set; } = [];
    public WebhookFooter? Footer { get; set; }
    public string? Timestamp { get; set; }
    public WebhookThumbnail? Thumbnail { get; set; }

    public Dictionary<string, object> ToDict()
    {
        var d = new Dictionary<string, object>
        {
            ["title"] = Title ?? "",
            ["description"] = Description ?? "",
            ["color"] = Color,
            ["timestamp"] = Timestamp ?? MakeTimestamp()
        };

        if (Fields.Count > 0)
            d["fields"] = Fields.Select(f => new Dictionary<string, object>
            {
                ["name"] = f.Name,
                ["value"] = f.Value,
                ["inline"] = f.Inline
            }).ToList();

        if (Footer != null)
            d["footer"] = new Dictionary<string, object> { ["text"] = Footer.Text };

        if (Thumbnail != null)
            d["thumbnail"] = new Dictionary<string, object> { ["url"] = Thumbnail.Url };

        return d;
    }
}

public static class WebhookSender
{
    private static readonly HttpClient _http = new() { Timeout = TimeSpan.FromSeconds(10) };

    public static async Task<bool> SendWebhook(string url, WebhookEmbed embed)
    {
        if (string.IsNullOrEmpty(url) || url == "#")
            return false;

        var payload = new Dictionary<string, object>
        {
            ["embeds"] = new List<object> { embed.ToDict() }
        };

        try
        {
            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = await _http.PostAsync(url, content);
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public static string MakeTimestamp() => DateTime.UtcNow.ToString("o");

    public static string FooterText(string botName, string suffix) => $"{botName} \u2022 {suffix}";
}
