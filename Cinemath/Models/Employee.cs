using Cinemath.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cinemath.Models
{
    public class Employee
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Address
        {
            get; set;
        }
        public string Phone
        {
            get; set;
        }
        public Roles Role
        {
            get; set;
        }
        public Employee()
        { 
            Id = 0;
            Name = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;

        }
        public Employee(int id, string name, string address, string phone, Roles role)
        {
            Id = id;
            Name = name;
            Address = address;
            Phone = phone;
            Role = role;
        }
    }
}
