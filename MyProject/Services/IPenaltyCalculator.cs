namespace MyProject.Services;

public interface IPenaltyCalculator
{
    decimal CalculatePenalty(DateTime expectedReturnDate, DateTime actualReturnDate);
}