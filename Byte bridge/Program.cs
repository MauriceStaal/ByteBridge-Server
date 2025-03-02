using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte_bridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Voeg minimal API services toe
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // API Endpoint: Haalt een bericht op
            app.MapGet("/api/message", () => Results.Ok("Hallo van de API-server!"));

            // Start de server
            app.Run("http://localhost:5000");
        }
    }
}