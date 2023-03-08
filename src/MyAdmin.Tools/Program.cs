// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        Type dbContextType = typeof(DbContext);
        foreach (Type t in types)
        {
            if (dbContextType.IsAssignableFrom(t))
            {
                // DbContext type found
                
            }
        }
    }
}