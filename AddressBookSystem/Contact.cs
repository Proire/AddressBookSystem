using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookSystem
{
    internal class Contact(string firstName,string lastName, string address, string city, long zip, string state, long phoneNumber, string email)
    {
        public int Id { get; set; } 
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set; } = lastName;

        public string Address { get; set; } = address;

        public string City { get; set; } = city;

        public long Zip { get; set; } = zip;

        public string State { get; set; } = state;

        public long PhoneNumber { get; set; } = phoneNumber;

        public string Email { get; set; } = email;

        public Contact() : this("", "", "", "", 0, "", 0, "") { }
        public override bool Equals(Object? obj)
        {
            if(obj is Contact && obj != null)
            {
                Contact contact = (Contact)obj;
                if (contact.FirstName == FirstName && contact.LastName == LastName)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id}. {FirstName} {LastName} {Address} {City} {Zip} {State} {PhoneNumber} {Email}";
        }
    }
}
