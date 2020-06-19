using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Dapper;
using MySql.Data.MySqlClient;

namespace BaseClassDepencyManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            FetchAllUsers fetch = new FetchAllUsers();
            var userList = fetch.Execute();
            //
            foreach (var user in userList)
            {
                Console.WriteLine($"Name: {user.First_Name}");
            }
        }
    }

    public abstract class DbExecutor<T> where T : new()
    {
        private readonly string _connectionString = "Server=localhost;Database=sql_workflow;Uid=sqltracking;Pwd=sqltracking;";

        public DbExecutor(string connectionString)
        {
            _connectionString = connectionString;
        }
        
        public DbExecutor()
        {
            _connection = new MySqlConnection(_connectionString);
        }
        //
        private MySqlConnection _connection;
        //
        public virtual T Execute()
        {
            return new T();
        }

        public MySqlConnection GetConnection()
        {
            return _connection;
        }
    }

    public class FetchAllUsers : DbExecutor<List<User>>
    {
        public override List<User> Execute()
        {
            var users = GetConnection().Query<User>("SELECT * FROM users");
            GetConnection().Close();
            return users.ToList();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string First_Name { get; set; }
    }
}