using System;
using System.Data.SqlClient;
using System.IO.Packaging;
using System.Windows;
namespace POE_part2
{
    public class tasks
    {
        string connection = @"Data source=(localdb)\task;Database=task";

        //createing method to test connection to the database
        public void test_connection()
        {
            /*SQLConnection- used to make connection to the database
             * SQLCommand- used to run quries, all of them
             * SQLDataReader- used to read what is collectrd by the SQLCommand and show user data
             */

            //connect to the database
            SqlConnection connect = new SqlConnection(connection);
            //try and catch any error that will throw
            try
            {
                //Open the connection and close the connection 
                connect.Open();
                //put the database query and run it 
                MessageBox.Show("connected...");
                //then close it after you are done
                connect.Close();
            }
            catch (Exception error)
            {
                //show massage error
                MessageBox.Show(error.Message);
            }
        }


        //method to insert task 
        public void insert_task(string name, string descrpition, string dueDate, string status)
        {
            //create a connection for instance
            SqlConnection connects = new SqlConnection(connection);
            //use try and catch 
            try
            {
                connects.Open();
                string query = $"insert into demo_tasks values('{name}', '{descrpition}','{dueDate}','{status}')";
                //use sqlcommand to run query
                SqlCommand run_query = new SqlCommand(query, connects);
                //run the query as a nonExecuteQuery()
                run_query.ExecuteNonQuery();
                connects.Close();
            }
            catch (Exception error)
            {
                //show massage error
                MessageBox.Show(error.Message);
            }


        }

    }
}