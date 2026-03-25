namespace MyProject.Services;

public class PenaltyCalculator : IPenaltyCalculator
{
    private readonly decimal _dailyPenaltyRate = 10.0m;

    public decimal CalculatePenalty(DateTime expectedReturnDate, DateTime actualReturnDate)
    {
        if (actualReturnDate <= expectedReturnDate) 
            return 0m;

        var lateDays = (actualReturnDate - expectedReturnDate).Days;
        return lateDays > 0 ? lateDays * _dailyPenaltyRate : 0m;
    }
}