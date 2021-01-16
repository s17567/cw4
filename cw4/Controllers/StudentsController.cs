using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using cw4.services;
using Microsoft.AspNetCore.Mvc;

namespace cw4.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s17567;Integrated Security=True";
        private IStudentsDal _dbService;
        public StudentsController(IStudentsDal dbService)
        {

            _dbService = dbService;


        }



        // var conBuilder = new SqlConnectionBuilder(); conBuilder.InitialCatalog ="pgago"  string conStr= conBuiilder.ConnectionString;
        
        
        
        [HttpGet]

        public IActionResult GetStudents([FromServices] IStudentsDal dbService)
        {


            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString)) //przy using bez ";"
            //con.Open();

            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select* from student1;";
                con.Open();
                SqlDataReader dr = com.ExecuteReader();  // wczytuje strumieniem danych
                while (dr.Read())
                {

                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString(); /// w nawiasie kwadratowym jest to indekser
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    list.Add(st);



                }
            }
            // con.Dispose();


            return Ok();


        }
        [HttpGet("{indexNumber}")]

        public IActionResult GetStudent(string indexNumber)
        {

            using (SqlConnection con = new SqlConnection(ConString)) //przy using bez ";"
            //con.Open();

            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select* from students where indexnumber=@index";
                SqlParameter par = new SqlParameter();
                par.Value = indexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);  // przekazanie parametru do comand aby ograniczyc mozliowsci sql injection 
                con.Open();


                //com.Parameters.AddWithValue("index", indexNumber);  >>> taka i powyzsza komenda powoduje ze przyjete komendy z sqlinjection sa przelozone na string a nie na polecenia, 


                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var st = new Student();

                    st.IndexNumber = dr["IndexNumber"].ToString(); /// w nawiasie kwadratowym jest to indekser
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    // parsowanie z wartosciami null moze powoduac blad
                    /*
                     *  if (dr["IndexNumber"] == DBNull.Value) {} << sprawdzenie czy w bazie jet null 
                     */
                }
            }
            return Ok();
        }



        [HttpGet("{indexNumber23}")]

        public IActionResult GetStudent2(string indexNumber)
        {

            using (SqlConnection con = new SqlConnection(ConString)) //przy using bez ";"
            //con.Open();

            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "TestProc3";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("LastName", "Kowalski");
                var dr = com.ExecuteReader();



            }
            return NotFound();
        }


        [HttpGet("{ex232}")]

        public IActionResult GetStudent3(string indexNumber)
        {

            using (SqlConnection con = new SqlConnection(ConString)) //przy using bez ";"
            //con.Open();

            using (SqlCommand com = new SqlCommand())
            {

                com.Connection = con;
                com.CommandText = "insert into Student(FirstName) values (@firstName)";
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();
                try
                {
                    int affectedRows = com.ExecuteNonQuery();
                    com.CommandText = "update into..";
                    com.ExecuteNonQuery();
                    transaction.Commit(); /// wszystkie zmiany powyzej wprowadzone bez tej linijki nie zostana zatwierdzone

                }
                catch (Exception exc)
                { transaction.Rollback();// jak zlapie jakikolwiek blad robie rollbacka 
                }
             



            }
            return Ok();
        }



    }
}
