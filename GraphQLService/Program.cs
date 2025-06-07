using Jakub_Zajega_14987.GraphQLService.GraphQL;
using Jakub_Zajega_14987.Infrastructure;
using Jakub_Zajega_14987.Infrastructure.Persistence;

namespace Jakub_Zajega_14987.GraphQLService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services
                .AddGraphQLServer()
                .AddQueryType<Query>()
                .AddProjections()
                .AddFiltering()
                .AddSorting();

            var app = builder.Build();

            app.MapGraphQL("/graphql");

            if (app.Environment.IsDevelopment())
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            app.Run();
        }
    }
}
