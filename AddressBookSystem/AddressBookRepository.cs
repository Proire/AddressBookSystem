using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace AddressBookSystem
{
    internal class AddressBookRepository
    {
        private string connectionString = string.Empty;

        public AddressBookRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Default") ?? throw new Exception("No Connection string found");
            CreateAddressBookTable();
        }

        public SqlConnection GetConnection()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = connectionString;
            return con;
        }


        public void CreateAddressBookTable()
        {
            SqlConnection con = null;
            try
            {
                con = GetConnection();
                SqlCommand cm = new("CREATE TABLE AddressBook ( Id INT PRIMARY KEY IDENTITY, FirstName NVARCHAR(50), LastName NVARCHAR(50), Address NVARCHAR(255), City NVARCHAR(50), Zip BIGINT, State NVARCHAR(50), PhoneNumber BIGINT, Email NVARCHAR(255) )", con);
                // Opening Connection  
                con.Open();
                // Executing the SQL query  
                cm.ExecuteNonQuery();
                // Displaying a message  
                Console.WriteLine("Table created Successfully");
            }
            catch (Exception)
            {
                Console.WriteLine("Table already Exits");
            }
            // Closing the connection  
            finally
            {
                con?.Close();
            }
        }

        public void PutContacts(List<Contact> contacts)
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    // Prepare the SQL query with parameters
                    string sqlQuery = "INSERT INTO AddressBook (FirstName, LastName, Address, City, Zip, State, PhoneNumber, Email) " +
                                      "VALUES ( @FirstName, @LastName, @Address, @City, @Zip, @State, @PhoneNumber, @Email)";

                    // Create a SqlCommand with parameters
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    // Open the connection
                    con.Open();

                    foreach (var contact in contacts)
                    {
                        // Clear previous parameters
                        cmd.Parameters.Clear();

                        // Set the parameters for each contact
                        cmd.Parameters.AddWithValue("@Id", contact.Id);
                        cmd.Parameters.AddWithValue("@FirstName", contact.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", contact.LastName);
                        cmd.Parameters.AddWithValue("@Address", contact.Address);
                        cmd.Parameters.AddWithValue("@City", contact.City);
                        cmd.Parameters.AddWithValue("@Zip", contact.Zip);
                        cmd.Parameters.AddWithValue("@State", contact.State);
                        cmd.Parameters.AddWithValue("@PhoneNumber", contact.PhoneNumber);
                        cmd.Parameters.AddWithValue("@Email", contact.Email);

                        // Execute the SQL query
                        int rowsAffected = cmd.ExecuteNonQuery();

                        // Display success or failure message
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"\nContact {contact.FirstName} {contact.LastName} added successfully.");
                        }
                        else
                        {
                            Console.WriteLine($"\nContact {contact.FirstName} {contact.LastName} could not be added.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nProblem occurred while adding contacts: {ex.Message}");
                }
            }
        }

        public void UpdateContact(Contact updatedContact)
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    // Prepare the SQL query with parameters
                    string sqlQuery = "UPDATE AddressBook " +
                                      "SET FirstName = @FirstName, " +
                                      "    LastName = @LastName, " +
                                      "    Address = @Address, " +
                                      "    City = @City, " +
                                      "    Zip = @Zip, " +
                                      "    State = @State, " +
                                      "    PhoneNumber = @PhoneNumber, " +
                                      "    Email = @Email " +
                                      "WHERE Id = @Id";

                    // Create a SqlCommand with parameters
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    // Open the connection
                    con.Open();

                    // Set the parameters for the updated contact
                    cmd.Parameters.AddWithValue("@Id", updatedContact.Id);
                    cmd.Parameters.AddWithValue("@FirstName", updatedContact.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", updatedContact.LastName);
                    cmd.Parameters.AddWithValue("@Address", updatedContact.Address);
                    cmd.Parameters.AddWithValue("@City", updatedContact.City);
                    cmd.Parameters.AddWithValue("@Zip", updatedContact.Zip);
                    cmd.Parameters.AddWithValue("@State", updatedContact.State);
                    cmd.Parameters.AddWithValue("@PhoneNumber", updatedContact.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", updatedContact.Email);

                    // Execute the SQL query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Display success or failure message
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"\nContact with ID {updatedContact.Id} updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"\nContact with ID {updatedContact.Id} could not be updated.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nProblem occurred while updating contact: {ex.Message}");
                }
            }
        }

        public void DeleteContactByName(string firstName, string lastName)
        {
            using (SqlConnection con = GetConnection())
            {
                try
                {
                    // Prepare the SQL query with parameters
                    string sqlQuery = "DELETE FROM AddressBook WHERE FirstName = @FirstName AND LastName = @LastName";

                    // Create a SqlCommand with parameters
                    SqlCommand cmd = new SqlCommand(sqlQuery, con);

                    // Open the connection
                    con.Open();

                    // Set the parameters for the contact to be deleted
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);

                    // Execute the SQL query
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Display success or failure message
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($"\nContact with name {firstName} {lastName} deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"\nContact with name {firstName} {lastName} not found or could not be deleted.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nProblem occurred while deleting contact: {ex.Message}");
                }
            }
        }


        public List<Contact> GetContact()
        {
            List<Contact> contacts = new List<Contact>();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM AddressBook", con);

                // Open the connection
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader(); // Returns object of SqlDataReader

                if (rdr.HasRows)
                {
                    while (rdr.Read())
                    {
                        Contact contact = new Contact
                        {
                            Id = Convert.ToInt32(rdr["Id"]),
                            FirstName = rdr["FirstName"].ToString(),
                            LastName = rdr["LastName"].ToString(),
                            Address = rdr["Address"].ToString(),
                            City = rdr["City"].ToString(),
                            Zip = long.Parse(rdr["Zip"].ToString()),
                            State = rdr["State"].ToString(),
                            PhoneNumber = long.Parse(rdr["PhoneNumber"].ToString()),
                            Email = rdr["Email"].ToString()
                        };

                        contacts.Add(contact);
                    }
                }
            }

            return contacts;
        }

    }
}
