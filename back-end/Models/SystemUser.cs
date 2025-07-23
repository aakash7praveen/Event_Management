using System.ComponentModel.DataAnnotations;

namespace EventManagementAPI.Models
{
    public class SystemUser
    {
        public int User_Id { get; set; }
        public string First_Name { get; set; }
        public string? Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }
        public string? Phone_Number { get; set; }
        public string? Profile_Picture { get; set; }
        public bool Role { get; set; } = false; 
        public DateTime Created_Ts { get; set; } = DateTime.Now;
        public string Enc_Password { get; set; }
    }
}
