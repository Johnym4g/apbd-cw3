namespace MyProject.Models;

public abstract class User
{
    public Guid Id { get; protected set; }
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public abstract int MaxActiveRentals { get; }

    protected User(string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
    }
}

public class Student : User
{
    public override int MaxActiveRentals => 2;

    public Student(string firstName, string lastName) : base(firstName, lastName) { }
}

public class Employee : User
{
    public override int MaxActiveRentals => 5;

    public Employee(string firstName, string lastName) : base(firstName, lastName) { }
}