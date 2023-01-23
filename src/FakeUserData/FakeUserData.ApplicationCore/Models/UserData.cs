namespace FakeUserData.ApplicationCore.Models;

public class UserData
{
    public string Id { get; set; }
    
    public int Gender { get; set; }
    
    public string FirstName { get; set; }
    
    public string MiddleName { get; set; }

    public string LastName { get; set; }
    
    public string FullName => MiddleName?.Length != 0 ? $"{FirstName} {MiddleName} {LastName}" : $"{FirstName} {LastName}";
    
    public Address Address { get; set; }

    public string AddressString { get; set; }
    
    public string PhoneNumber { get; set; }
}