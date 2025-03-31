using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;

class ClinicAppointmentSystem
{
    static string connectionString = "server=127.0.0.1;port=3306;database=clinicdatabase;user=root;password=;";

    static void Main()
    {
        

        while (true)
        {
            Console.WriteLine("\nClinic Appointment System");
            Console.WriteLine("1. Register Patient");
            Console.WriteLine("2. Book an Appointment");
            Console.WriteLine("3. View Available Doctors");
            Console.WriteLine("4. View Appointments");
            Console.WriteLine("5. Delete an Appointment");
            Console.WriteLine("6. Update Patient");
            Console.WriteLine("7. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    RegisterPatient();
                    break;
                case "2":
                    BookAppointment();
                    break;
                case "3":
                    ViewDoctors();
                    break;
                case "4":
                    ViewAppointments();
                    break;
                case "5":
                    DeleteAppointment();
                    break;
                case "6":
                    UpdatePatient();
                    break;
                case "7":
                    Console.WriteLine("Exiting the system. Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice! Please try again.");
                    break;
            }
        }
    }

    static void RegisterPatient()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write(@"
 
 ██▀███ ▓█████  ▄████ ██▓ ██████▄▄▄█████▓█████ ██▀███      ██▓███  ▄▄▄    ▄▄▄█████▓██▓█████ ███▄    █▄▄▄█████▓
▓██ ▒ ██▓█   ▀ ██▒ ▀█▓██▒██    ▒▓  ██▒ ▓▓█   ▀▓██ ▒ ██▒   ▓██░  ██▒████▄  ▓  ██▒ ▓▓██▓█   ▀ ██ ▀█   █▓  ██▒ ▓▒
▓██ ░▄█ ▒███  ▒██░▄▄▄▒██░ ▓██▄  ▒ ▓██░ ▒▒███  ▓██ ░▄█ ▒   ▓██░ ██▓▒██  ▀█▄▒ ▓██░ ▒▒██▒███  ▓██  ▀█ ██▒ ▓██░ ▒░
▒██▀▀█▄ ▒▓█  ▄░▓█  ██░██░ ▒   ██░ ▓██▓ ░▒▓█  ▄▒██▀▀█▄     ▒██▄█▓▒ ░██▄▄▄▄█░ ▓██▓ ░░██▒▓█  ▄▓██▒  ▐▌██░ ▓██▓ ░ 
░██▓ ▒██░▒████░▒▓███▀░██▒██████▒▒ ▒██▒ ░░▒████░██▓ ▒██▒   ▒██▒ ░  ░▓█   ▓██▒▒██▒ ░░██░▒████▒██░   ▓██░ ▒██▒ ░ 
░ ▒▓ ░▒▓░░ ▒░ ░░▒   ▒░▓ ▒ ▒▓▒ ▒ ░ ▒ ░░  ░░ ▒░ ░ ▒▓ ░▒▓░   ▒▓▒░ ░  ░▒▒   ▓▒█░▒ ░░  ░▓ ░░ ▒░ ░ ▒░   ▒ ▒  ▒ ░░   
  ░▒ ░ ▒░░ ░  ░ ░   ░ ▒ ░ ░▒  ░ ░   ░    ░ ░  ░ ░▒ ░ ▒░   ░▒ ░      ▒   ▒▒ ░  ░    ▒ ░░ ░  ░ ░░   ░ ▒░   ░    
  ░░   ░   ░  ░ ░   ░ ▒ ░  ░  ░   ░        ░    ░░   ░    ░░        ░   ▒   ░      ▒ ░  ░     ░   ░ ░  ░      
   ░       ░  ░     ░ ░       ░            ░  ░  ░                      ░  ░       ░    ░  ░        ░         
                                                                                                              


");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter your Name: ");
        string name = Console.ReadLine();
        Console.Write("Enter your Contact Number: ");
        string contact = Console.ReadLine();
        Console.Write("Enter your Email: ");
        string email = Console.ReadLine();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO patients (name, contact, email) VALUES (@name, @contact, @email)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@contact", contact);
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Patient registration successful!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void BookAppointment()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write(@"

▄▄▄▄   ▒█████  ▒█████  ██ ▄█▀    ▄▄▄      ███▄    █     ▄▄▄      ██▓███  ██▓███  ▒█████  ██▓███▄    █▄▄▄█████▓███▄ ▄███▓█████ ███▄    █▄▄▄█████▓
▓█████▄▒██▒  ██▒██▒  ██▒██▄█▒    ▒████▄    ██ ▀█   █    ▒████▄   ▓██░  ██▓██░  ██▒██▒  ██▓██▒██ ▀█   █▓  ██▒ ▓▓██▒▀█▀ ██▓█   ▀ ██ ▀█   █▓  ██▒ ▓▒
▒██▒ ▄█▒██░  ██▒██░  ██▓███▄░    ▒██  ▀█▄ ▓██  ▀█ ██▒   ▒██  ▀█▄ ▓██░ ██▓▓██░ ██▓▒██░  ██▒██▓██  ▀█ ██▒ ▓██░ ▒▓██    ▓██▒███  ▓██  ▀█ ██▒ ▓██░ ▒░
▒██░█▀ ▒██   ██▒██   ██▓██ █▄    ░██▄▄▄▄██▓██▒  ▐▌██▒   ░██▄▄▄▄██▒██▄█▓▒ ▒██▄█▓▒ ▒██   ██░██▓██▒  ▐▌██░ ▓██▓ ░▒██    ▒██▒▓█  ▄▓██▒  ▐▌██░ ▓██▓ ░ 
░▓█  ▀█░ ████▓▒░ ████▓▒▒██▒ █▄    ▓█   ▓██▒██░   ▓██░    ▓█   ▓██▒██▒ ░  ▒██▒ ░  ░ ████▓▒░██▒██░   ▓██░ ▒██▒ ░▒██▒   ░██░▒████▒██░   ▓██░ ▒██▒ ░ 
░▒▓███▀░ ▒░▒░▒░░ ▒░▒░▒░▒ ▒▒ ▓▒    ▒▒   ▓▒█░ ▒░   ▒ ▒     ▒▒   ▓▒█▒▓▒░ ░  ▒▓▒░ ░  ░ ▒░▒░▒░░▓ ░ ▒░   ▒ ▒  ▒ ░░  ░ ▒░   ░  ░░ ▒░ ░ ▒░   ▒ ▒  ▒ ░░   
▒░▒   ░  ░ ▒ ▒░  ░ ▒ ▒░░ ░▒ ▒░     ▒   ▒▒ ░ ░░   ░ ▒░     ▒   ▒▒ ░▒ ░    ░▒ ░      ░ ▒ ▒░ ▒ ░ ░░   ░ ▒░   ░   ░  ░      ░░ ░  ░ ░░   ░ ▒░   ░    
 ░    ░░ ░ ░ ▒ ░ ░ ░ ▒ ░ ░░ ░      ░   ▒     ░   ░ ░      ░   ▒  ░░      ░░      ░ ░ ░ ▒  ▒ ░  ░   ░ ░  ░     ░      ░     ░     ░   ░ ░  ░      
 ░         ░ ░     ░ ░ ░  ░            ░  ░        ░          ░  ░                   ░ ░  ░          ░               ░     ░  ░        ░         
      ░                                                                                                                                          
");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("Enter your Name: ");
        string patientName = Console.ReadLine();
        Console.Write("Enter your Email: ");
        string patientEmail = Console.ReadLine();
        Console.Write("Enter Doctor's Name: ");
        string doctorName = Console.ReadLine();
        Console.Write("Enter Appointment Date (YYYY-MM-DD): ");
        string date = Console.ReadLine();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO appointments (patient_name, patient_email, doctor_name, appointment_date) VALUES (@patient, @email, @doctor, @date)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@patient", patientName);
                    command.Parameters.AddWithValue("@email", patientEmail);
                    command.Parameters.AddWithValue("@doctor", doctorName);
                    command.Parameters.AddWithValue("@date", date);
                    command.ExecuteNonQuery();
                }
                Console.WriteLine("Appointment booked successfully!");

                SendEmailNotification(patientEmail, doctorName, date);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void ViewDoctors()
    {
        Console.WriteLine("\nAvailable Doctors:");
        Console.WriteLine("- Dr. Kunat (General Practitioner) - Mon-Fri 8AM-4PM");
        Console.WriteLine("- Dr. Mo (Surgeon) - Tue-Sat 9PM-5AM");
        Console.WriteLine("- Dr. Kurt (Pediatrician) - Wed-Sun 12PM-6PM\n");
    }

    static void ViewAppointments()
    {

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(@"
           /$$                                                                 /$$           /$$                                     /$$            
          |__/                                                                |__/          | $$                                    | $$            
 /$$    /$$/$$ /$$$$$$ /$$  /$$  /$$        /$$$$$$  /$$$$$$  /$$$$$$  /$$$$$$ /$$/$$$$$$$ /$$$$$$  /$$$$$$/$$$$  /$$$$$$ /$$$$$$$ /$$$$$$  /$$$$$$$
|  $$  /$$| $$/$$__  $| $$ | $$ | $$       |____  $$/$$__  $$/$$__  $$/$$__  $| $| $$__  $|_  $$_/ | $$_  $$_  $$/$$__  $| $$__  $|_  $$_/ /$$_____/
 \  $$/$$/| $| $$$$$$$| $$ | $$ | $$        /$$$$$$| $$  \ $| $$  \ $| $$  \ $| $| $$  \ $$ | $$   | $$ \ $$ \ $| $$$$$$$| $$  \ $$ | $$  |  $$$$$$ 
  \  $$$/ | $| $$_____| $$ | $$ | $$       /$$__  $| $$  | $| $$  | $| $$  | $| $| $$  | $$ | $$ /$| $$ | $$ | $| $$_____| $$  | $$ | $$ /$\____  $$
   \  $/  | $|  $$$$$$|  $$$$$/$$$$/      |  $$$$$$| $$$$$$$| $$$$$$$|  $$$$$$| $| $$  | $$ |  $$$$| $$ | $$ | $|  $$$$$$| $$  | $$ |  $$$$/$$$$$$$/
    \_/   |__/\_______/\_____/\___/        \_______| $$____/| $$____/ \______/|__|__/  |__/  \___/ |__/ |__/ |__/\_______|__/  |__/  \___/|_______/ 
                                                   | $$     | $$                                                                                    
                                                   | $$     | $$                                                                                    
                                                   |__/     |__/                                                                                    ");
        Console.ForegroundColor = ConsoleColor.White;
        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM appointments";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("\nScheduled Appointments:");
                    while (reader.Read())
                    {
                        Console.WriteLine($"- {reader["appointment_date"]}: {reader["patient_name"]} with Dr. {reader["doctor_name"]}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }

    static void DeleteAppointment()
    {
        Console.Write("Enter Patient Name to delete: ");
        string patientName = Console.ReadLine();

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM appointments WHERE patient_name = @patientName";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@patientName", patientName);
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine(rowsAffected > 0 ? "Appointment deleted successfully!" : "Appointment not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
        }
    }
    static void UpdatePatient()
    {
        Console.Write("========================================");
        Console.Write(@"
                          $$\            $$\                                          $$\     $$\                      $$\     
                          $$ |           $$ |                                         $$ |    \__|                     $$ |    
$$\   $$\  $$$$$$\   $$$$$$$ | $$$$$$\ $$$$$$\    $$$$$$\         $$$$$$\   $$$$$$\ $$$$$$\   $$\  $$$$$$\  $$$$$$$\ $$$$$$\   
$$ |  $$ |$$  __$$\ $$  __$$ | \____$$\\_$$  _|  $$  __$$\       $$  __$$\  \____$$\\_$$  _|  $$ |$$  __$$\ $$  __$$\\_$$  _|  
$$ |  $$ |$$ /  $$ |$$ /  $$ | $$$$$$$ | $$ |    $$$$$$$$ |      $$ /  $$ | $$$$$$$ | $$ |    $$ |$$$$$$$$ |$$ |  $$ | $$ |    
$$ |  $$ |$$ |  $$ |$$ |  $$ |$$  __$$ | $$ |$$\ $$   ____|      $$ |  $$ |$$  __$$ | $$ |$$\ $$ |$$   ____|$$ |  $$ | $$ |$$\ 
\$$$$$$  |$$$$$$$  |\$$$$$$$ |\$$$$$$$ | \$$$$  |\$$$$$$$\       $$$$$$$  |\$$$$$$$ | \$$$$  |$$ |\$$$$$$$\ $$ |  $$ | \$$$$  |
 \______/ $$  ____/  \_______| \_______|  \____/  \_______|      $$  ____/  \_______|  \____/ \__| \_______|\__|  \__|  \____/ 
          $$ |                                                   $$ |                                                          
          $$ |                                                   $$ |                                                          
          \__|                                                   \__|                                                          
");

        Console.Write("\nEnter your Name to update info: ");
        string name = Console.ReadLine();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            
            string checkQuery = "SELECT COUNT(*) FROM patients WHERE name = @name";
            using (var checkCmd = new MySqlCommand(checkQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@name", name);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    Console.WriteLine("Patient not found. Please register first.");
                    return;
                }
            }

            Console.WriteLine("What do you want to update?");
            Console.WriteLine("1. Contact Number");
            Console.WriteLine("2. Email");
            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            string field = "", newValue = "";

            switch (option)
            {
                case "1":
                    field = "contact";
                    Console.Write("Enter new Contact Number: ");
                    newValue = Console.ReadLine();
                    break;
                case "2":
                    field = "email";
                    Console.Write("Enter new Email: ");
                    newValue = Console.ReadLine();
                    break;
                case "3":
                    field = "password";
                    Console.Write("Enter new Password: ");
                    newValue = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    return;
            }

            string updateQuery = $"UPDATE patients SET {field} = @value WHERE name = @name";
            using (var updateCmd = new MySqlCommand(updateQuery, connection))
            {
                updateCmd.Parameters.AddWithValue("@value", newValue);
                updateCmd.Parameters.AddWithValue("@name", name);
                int rows = updateCmd.ExecuteNonQuery();

                if (rows > 0)
                    Console.WriteLine("Patient information updated successfully!");
                else
                    Console.WriteLine("Update failed.");
            }
        }
        Console.Write("\n========================================");
    }

    static void SendEmailNotification(string toEmail, string doctorName, string date)
    {
        try
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("abelardogermedia@gmail.com", "Clinic Appointment System");
            mail.To.Add(toEmail);
            mail.Subject = "Appointment Confirmation";
            mail.Body = $"Dear Patient,\n\nYour appointment with Dr. {doctorName} on {date} has been confirmed.\n\nThank you for choosing our clinic.";

            smtpServer.Port = 587;
            smtpServer.Credentials = new NetworkCredential("abelardogermedia@gmail.com", "tefb xvyt kmhw vsnr\r\n");
            smtpServer.EnableSsl = true;
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpServer.UseDefaultCredentials = false;

            smtpServer.Send(mail);
            Console.WriteLine("Email notification sent successfully.");
        }
        catch (SmtpException smtpEx)
        {
            Console.WriteLine("SMTP Error: " + smtpEx.Message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to send email: " + ex.Message);
        }
    }
}
