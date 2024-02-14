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
    }
}
