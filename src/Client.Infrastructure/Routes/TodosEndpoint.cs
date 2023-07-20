
namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class TodosEndpoint
    {
        public static string GetAll = "api/v1/todos";
        public static string Save = "api/v1/todos";
        public static string Delete = "api/v1/todos";
        public static string GetById(int todoId)
        {
            return $"api/v1/todos/{todoId}";
        }
    }
}
