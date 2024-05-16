using AddressBookSystem;

internal class Program
{
    private static void Main(string[] args)
    {
        AddressBookMain addressBookMain = new AddressBookMain();

        // Adding First Address Book
        addressBookMain.AddAddressBook();

        // Menudriven code for Address book
        int userChoice;
        do
        {
            Console.WriteLine("\n\nEnter the Choice");
            Console.WriteLine("\n1.Add Address Book\n2.Enter Address Book\n3.Search Person by City or State\n4.Search Person for each City\n5.Search Person for each State\n6.Get count by City or State\n7.Get Sorted Contacts by Name\n8.Get Sorted Contacts by city and state\n0.Exit Application\n");
            _ = int.TryParse(Console.ReadLine(), out userChoice);

            // Choice for Creating Address Book or Entering particular Address Book
            switch(userChoice)
            {
                case 1:
                    // Adding new Address Book 
                    addressBookMain.AddAddressBook();
                    break;
                case 2:
                    //Entering Address book
                    addressBookMain.EnterAddressBook();
                    break;
                case 3:
                    // Search Contact/Person by city or state
                    addressBookMain.SearchAddressBooksByCityOrState();
                    break;
                case 4:
                    // Search Contact/Person for each city
                    addressBookMain.SearchAddressBookForEachCity();
                    break;
                case 5:
                    // Search Contact/Person for each state
                    addressBookMain.SearchAddressBookForEachState();
                    break;
                case 6:
                    // Count of contacts of particular city or state
                    addressBookMain.CountOfContactsByCityOrState();
                    break;
                case 7:
                    // Sorted Contacts By Name 
                    addressBookMain.SortedContactsByName();
                    break;
                case 8:
                    // Sorted Contacts By City or State
                    addressBookMain.SortedContactsByCityAndState();
                    break;
                case 0:
                    Console.WriteLine("Exiting Application, Thank you for Visiting"); 
                    break;
                default :
                    Console.WriteLine("Wrong choice");
                    break;
            }
        } while (userChoice != 0);

        addressBookMain.PrintAddressBooks();
    }   
}