namespace DB_Project.Models
{
    public interface IDatabaseExecutor
    {
        void ExecuteQuery(string query);
    }
}
