using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Classicmodels
{
	class EmployeesTable
	{
		public int id { get; set; }
		public string firstname { get; set; }
		public string lastname { get; set; }
		public string email { get; set; }

	}

	class ProductsTable
	{
		public string name { get; set; }
		public string type { get; set; }
		public int quantity { get; set; }
		public double price { get; set; }
	}

	class Program
	{
		static string connString = "server=localhost;database=classicmodels;uid=teszt;pwd=abc123";
		static MySqlConnection conn = new MySqlConnection(connString);

		static void Main(string[] args)
		{
			Console.WriteLine("Kapcsolat létrehozása..");
			conn.Open();
			Console.WriteLine("Kapcsolat létrehozva!");

			Employees();
			Products();

			Console.ReadKey();
		}

		static void Employees()
		{
			List<EmployeesTable> employees = new List<EmployeesTable>();
			string query = "SELECT * FROM employees";
			MySqlCommand command = new MySqlCommand(query, conn);
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				var employee = new EmployeesTable
				{
					id = Convert.ToInt32(reader["employeeNumber"]),
					firstname = Convert.ToString(reader["firstName"]),
					lastname = Convert.ToString(reader["lastName"]),
					email = Convert.ToString(reader["email"])
				};
				employees.Add(employee);
			}
			reader.Close();

			foreach (var item in employees)
			{
				Console.WriteLine($"{item.id} | {item.firstname} | {item.lastname} | {item.email}");
			}

		}

		static void Products()
		{
			List<ProductsTable> products = new List<ProductsTable>();
			string query = "SELECT * FROM products";
			MySqlCommand command = new MySqlCommand(query, conn);
			MySqlDataReader reader = command.ExecuteReader();

			while (reader.Read())
			{
				var product = new ProductsTable
				{
					name = Convert.ToString(reader["productName"]),
					type = Convert.ToString(reader["productLine"]),
					quantity = Convert.ToInt32(reader["quantityInStock"]),
					price = Convert.ToDouble(reader["buyPrice"])
				};
				products.Add(product);
			}
			reader.Close();

			//1. hány darab termék
			Console.WriteLine("1. feladat");
			Console.WriteLine($"Összesen {products.Count} darab termék van.");

			//2. típusonként hány darab
			Console.WriteLine("2. feladat");
			var darab = from sor in products
						group sor by sor.type;

			var darab2 = products.GroupBy(x => x.type);

			foreach (var item in darab2)
			{
				Console.WriteLine($"\t{item.Key} - {item.Count()}");
			}

			//3. csak a megadott tipusúakat írja
			Console.WriteLine("3. feladat");
			string tipusneve = "Planes";

			var tipus = from p in products
						where p.type == tipusneve
						select p;

			var tipus2 = products.Where(p => p.type == tipusneve);

			if (tipus.Any())
			{
				foreach (var item in tipus2)
				{
					Console.WriteLine(item.name + " " + item.price + " " + item.type);
				}
			}
			else
			{
				Console.WriteLine("Nincs ilyen termék");
			}

			//4. összes Cars-ra végződő tipusokat írja ki
			Console.WriteLine("4. feladat");
			var cars = from p in products
					   where p.type.EndsWith("Cars")
					   select p;

			var cars_lambda = products.Where(x => x.type.EndsWith("Cars"));

			if (cars.Any())
			{
				foreach (var item in cars)
				{
					Console.WriteLine(item.name + " " + item.price + " " + item.type);
				}
			}
			else
			{
				Console.WriteLine("Nincs ilyen termék");
			}

			//5. legdrágább jármű adatai (1 db)
			Console.WriteLine("5. feladat");
			var legtobb = (from sor in products
						   orderby sor.price
						   select sor).Last();
			Console.WriteLine(legtobb.name + " ár: " + legtobb.price + "$ " + legtobb.type);

			//6. legdrágább jármű adatai (több is lehet)
			Console.WriteLine("6. feladat");
			double max = (from p in products
						  select p.price).Max();

			var legdragabbak = from sor in products
							   where sor.price == max
							   select sor;

			foreach (var item in legdragabbak)
			{
				Console.WriteLine(item.name + " ár: " + item.price + "$ " + item.type);
			}

			//7. típusonként legdrágább ár
			Console.WriteLine("7. feladat");
			var legdragabbtipus = from sor in products
								  group sor.price by sor.type;

			foreach (var item in legdragabbtipus)
			{
				Console.WriteLine($"\t{item.Key} - {item.Max()}");
			}
			//8. How many distinct products does ClassicModels sell?
			Console.WriteLine("8. feladat");
			var dis = (from sor in products
					   select sor.type).Distinct();

			var dis_lambda = products.Select(x => x.type).Distinct();

			Console.WriteLine(dis.Count());
			Console.WriteLine(dis_lambda.Count());

			foreach (var item in dis_lambda)
			{
				Console.WriteLine(item);
			}

		}
	}
}
