namespace Sparky;

public interface ICustomer
{
    int Disccount { get; set; }
    int OrderTotal { get; set; }
    string GreetMessage { get; set; }
    bool IsPlatinum { get; set; }
    string GreetAndCombineNames(string firstName, string LastName);
    CustomerType GetCustomerDetails();
}

public class Customer : ICustomer
{
    public int Disccount { get; set; }
    public int OrderTotal { get; set; }
    public string GreetMessage { get; set; }
    public bool IsPlatinum { get; set; }

    public Customer()
    {
        Disccount = 15;
        IsPlatinum = false;
    }
    public string GreetAndCombineNames(string firstName, string LastName)
    {
        if (string.IsNullOrEmpty(firstName)) 
            throw new ArgumentException("Empty First Name");

        GreetMessage = $"Hello, {firstName} {LastName}";
        Disccount = 20;
        return GreetMessage;
    }

    public CustomerType GetCustomerDetails()
    {
        if (OrderTotal < 100)
        {
            return new BasicCustomer();
        }
        return new PlatinumCustomer();
    }
}
public class CustomerType { }
public class BasicCustomer : CustomerType { }
public class PlatinumCustomer: CustomerType { }