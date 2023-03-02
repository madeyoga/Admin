using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MyAdmin.Admin;

public class PaginatedList<T> : List<T>
{
    public int PageIndex { get; private set; }
    public int TotalPages { get; private set; }

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);

        this.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;

    public bool HasNextPage => PageIndex < TotalPages;

    public List<Dictionary<string, object?>> ToDictionary()
    {
        List<Dictionary<string, object?>> data = new();
        if (this != null)
        {
            foreach (var record in this)
            {
                Dictionary<string, object?> temp = new();
                foreach (var prop in record!.GetType().GetProperties())
                {
                    temp.Add(prop.Name, prop.GetValue(record));
                }
                data.Add(temp);
            }
        }
        return data;
    }

    public JsonArray ToJsonArray()
    {
        JsonArray dataset = new();

        foreach (var item in this)
        {
            // Serialize the model to a JSON string
            var jsonString = JsonSerializer.Serialize(item);

            // Parse the JSON string to a JSON object
            var jsonObject = JsonSerializer.Deserialize<JsonElement>(jsonString);

            dataset.Add(jsonObject);
        }
        return dataset;
    }

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}