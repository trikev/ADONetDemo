using Microsoft.Data.SqlClient;
using System.Transactions;
using System.Data;
using Microsoft.Identity.Client;
namespace ADONetDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
        public static void GettingMoviesWithSqlCommandString()
        {
            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            Console.WriteLine("Insert first name on actor:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Insert last name on actor:");
            string lastName = Console.ReadLine();



            string queryInsert = @"select film.title 
                                    from film 
                                    inner join film_actor on film.film_id=film_actor.film_id 
                                    inner join actor on film_actor.actor_id=actor.actor_id 
                                    where actor.first_name = @FirstName and actor.last_name = @LastName ";

            var command = new SqlCommand(queryInsert, connection);


            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);

            try
            {
                connection.Open();
                var rec = command.ExecuteReader();
                if (rec.HasRows)
                {
                    Console.WriteLine($"The actor {firstName} {lastName} is part of the following movies:");
                    while (rec.Read())
                    {
                        Console.WriteLine("- " + rec.GetString(0));
                    }
                }
                else
                {
                    Console.WriteLine($"There is no actor with the name {firstName} {lastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong\n" + ex);
            }
            finally
            {
                connection.Close();
            }
        }
        public static void GettingMoviesWithStoredProcedure()
        {
            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            Console.WriteLine("Insert first name on actor:");
            string firstName = Console.ReadLine();
            Console.WriteLine("Insert last name on actor:");
            string lastName = Console.ReadLine();

            var command = new SqlCommand("GetMoviesBasedOnActor", connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@FirstName", firstName);
            command.Parameters.AddWithValue("@LastName", lastName);

            try
            {
                connection.Open();
                var rec = command.ExecuteReader();
                if (rec.HasRows)
                {
                    Console.WriteLine($"The actor {firstName} {lastName} is part of the following movies:");
                    while (rec.Read())
                    {
                        Console.WriteLine("- " + rec["title"].ToString());
                    }
                }
                else
                {
                    Console.WriteLine($"There is no actor with the name {firstName} {lastName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Something went wrong\n" + ex);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
