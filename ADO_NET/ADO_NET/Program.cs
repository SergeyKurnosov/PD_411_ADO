//ADO_NET
//ADO (ActiveX Data Object) JSON, XML, MS SQL Server, MySQL, Oracle....
//#define SCALAR_CHECK
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//ADO.NET:
using System.Data.SqlClient;

namespace ADO_NET
{
	class Program
	{
		static string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Movies;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
		static SqlConnection connection;
	
		static void Main(string[] args)
		{
			//1) Создаем подключение к Базе данных на Сервере:
			Console.WriteLine(connectionString);
			connection = new SqlConnection();
			connection.ConnectionString = connectionString;

			//Select("SELECT * FROM Directors");
			//Select("SELECT * FROM Movies");
			Select("*", "Directors");
			Select("movie_name,release_date,first_name+last_name AS Режиссер", "Movies,Directors", "director=director_id");

#if SCALAR_CHECK
			connection.Open();
			string cmd = "SELECT COUNT(*) FROM Directors";
			SqlCommand command = new SqlCommand(cmd, connection);
			Console.WriteLine($"Количество режиссереов:\t{command.ExecuteScalar()}");

			command.CommandText = "SELECT COUNT(*) FROM Movies";
			Console.WriteLine($"Количество киношек:\t{command.ExecuteScalar()}");

			command.CommandText = "SELECT last_name FROM Directors WHERE first_name=N'James'";
			Console.WriteLine(command.ExecuteScalar());

			connection.Close();

			Console.WriteLine(Scalar("SELECT last_name FROM Directors WHERE first_name=N'James'"));
			Console.WriteLine(Scalar("SELECT COUNT(*) FROM Movies")); 
#endif

			Console.Write("Введите имя: ");
			string first_name = Console.ReadLine();

			Console.Write("Введите фамилию: ");
			string last_name = Console.ReadLine();

			string cmd = 
$"INSERT Directors(director_id,first_name,last_name) VALUES ({Convert.ToInt32(Scalar("SELECT MAX(director_id) FROM Directors")) + 1},N'{first_name}',N'{last_name}');";

			SqlCommand command = new SqlCommand(cmd, connection);

			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();

			Select("*", "Directors");

		}

		static void Select(string fields, string tables, string condition="")
		{
			//2) Открываем соединение:	(После того как подключение создано, оно не является отрытым, т.е., подключение всегда открывается вручную при необходимости.)
			connection.Open();

			///////////////////////////////////////////////////////////////////

			//3) Создаем 'SqlCommand':
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			cmd += ";";
			SqlCommand command = new SqlCommand(cmd, connection);

			//4) Создаем 'Reader':
			SqlDataReader reader = command.ExecuteReader();
			for (int i = 0; i < reader.FieldCount; i++)
			{
				Console.Write(reader.GetName(i) + "\t");
			}
			Console.WriteLine();
			while (reader.Read())
			{
				//Console.WriteLine($"{reader[0]}\t{reader[1]}\t{reader[2]}");
				for (int i = 0; i < reader.FieldCount; i++)
					Console.Write(reader[i] + "\t\t");
				Console.WriteLine();
			}
			reader.Close();

			///////////////////////////////////////////////////////////////////
			//?) !!! Подключения обязытельно нужно закрывать !!!
			connection.Close();
		}

		static object Scalar(string cmd)
		{
			connection.Open();
			SqlCommand command = new SqlCommand(cmd, connection);
			object obj = command.ExecuteScalar();
			connection.Close();
			return obj;
		}
	}
}
/*
CREATE TABLE [dbo].[Movies] (
    [movie_id]    INT            NOT NULL,
    [movie_name]  NVARCHAR (256) NOT NULL,
    [relese_date] DATE           NOT NULL,
    [director]    INT            NOT NULL,
    CONSTRAINT [FK_Movies_Directors] FOREIGN KEY ([director]) REFERENCES [dbo].[Directors] ([director_id]), 
    CONSTRAINT [PK_Movies] PRIMARY KEY ([movie_id])
);


 */
