//ADO_NET
//ADO (ActiveX Data Object) JSON, XML, MS SQL Server, MySQL, Oracle....
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
		static void Main(string[] args)
		{
			//1) Создаем подключение к Базе данных на Сервере:
			string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Movies;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"; ;
			Console.WriteLine(connectionString);
			SqlConnection connection = new SqlConnection();
			connection.ConnectionString = connectionString;
			
			//2) Открываем соединение:	(После того как подключение создано, оно не является отрытым, т.е., подключение всегда открывается вручную при необходимости.)
			connection.Open();

			///////////////////////////////////////////////////////////////////

			//3) Создаем 'SqlCommand':
			string cmd = "SELECT * FROM Directors;";
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
					Console.Write(reader[i]+"\t\t");
				Console.WriteLine();
			}
			reader.Close();

			///////////////////////////////////////////////////////////////////
			//?) !!! Подключения обязытельно нужно закрывать !!!
			connection.Close();
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
