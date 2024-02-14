using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using Npgsql;
using Dapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    
    public class UsersController : ControllerBase
    {
        private string CONNECTIONSTRING = "Host=localhost;Port=5432;Database=postgres;User Id=postgres;Password=7257320;";
        [HttpGet]
        public List<Products> GetDapper()
        {
            using(NpgsqlConnection connection  = new NpgsqlConnection(CONNECTIONSTRING))
            {   
                string query = "select * from products";
                return connection.Query<Products>(query).ToList();
            }
        }

        [HttpPost]
        public string PostDapper(string Name, string Description, string PhotoPath)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = $"insert into products(name, description, photopath) values (@name, @description, @photopath);";
                int status = connection.Execute(query, new {name = Name, 
                   description = Description, photopath = PhotoPath});
                return $"Post status [=> {status}";
            }
        }
        [HttpPut]
        public string PutDapper(int id, Products products)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection (CONNECTIONSTRING))
            {
                string query = $"update products set name = @name, description = @description, photopath = @photopath where id = @id";
                int status = connection.Execute(query, new {id = id, 
                    name = products.Name, 
                    description = products.Description, 
                    photopath = products.PhotoPath});
            return $"Update Status [=> {status}";
            }
        }
        [HttpPatch]
        public string PatchDapper(int id, string Name) 
        {
            using(NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING)) 
            {
                string query = $"update products set name = @name where id = @id";
                int status= connection.Execute(query, new {id = id, name = Name});
                 
            return $"Patch status [=> {status}";
            }


        }
        [HttpDelete]
        public string DeleteDapper(int id)
        {
            using(NpgsqlConnection connection=new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = $"delete from products where id = @id";
                int status = connection.Execute(query, new {id = id  });
                return $"Delete status [=> {status}";
            }
        }

        [HttpGet]
        public List<Products> GetADO()
        {
            using(NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = "select * from products";
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                var res = command.ExecuteReader();
                List<Products>? products = new List<Products>();
                while(res.Read()) { 

                 products.Add(new Products
                 {
                     Id = (int)res[0],
                     Name = (string)res[1],
                     Description = (string)res[2],
                     PhotoPath = (string)res[3]
                 });

                }
                return products;
            }
        }
        [HttpPost]
        public string PostADO(string name, string description, string PhotoPath)
        {
            using(NpgsqlConnection connection=new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = $"insert into products(name, description, photopath) values (@name, @description, @photopath)";
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand( query, connection);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("description", description);
                command.Parameters.AddWithValue("photopath", PhotoPath);
                int status = command.ExecuteNonQuery();
                return $"Post status [=> {status}";

            }
        }
        [HttpPut]
        public string PutADO(int id, string name, string description, string photopath)
        {
            using(NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = $"update products set name = @name, description = @description, photopath = @photopath where id = @id";
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("name", name); 
                command.Parameters.AddWithValue("description", description);
                command.Parameters.AddWithValue("photopath", photopath);
                int status = command.ExecuteNonQuery();
                return $"Put status [=> {status}";
            }
        }
        [HttpPatch]
        public string PatchADO(int id, string name)
        {
            using(NpgsqlConnection connection = new NpgsqlConnection(CONNECTIONSTRING))
            {
                string query = $"update products set name = @name where id = @id";
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(query, connection);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("id", id);
                int status = command.ExecuteNonQuery();
                return $"Patch status [=> {status}";
            }
        }
    }
}
