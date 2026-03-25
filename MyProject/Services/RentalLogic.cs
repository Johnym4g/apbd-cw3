using MyProject.Models;

namespace MyProject.Services;

public class RentalLogic
{
    private readonly List<User> _users = new();
    private readonly List<Equipment> _equipment = new();
    private readonly List<Rental> _rentals = new();
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalLogic(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator;
    }

    public void AddUser(User user) => _users.Add(user);
    
    public void AddEquipment(Equipment item) => _equipment.Add(item);

    public IEnumerable<Equipment> GetAllEquipment() => _equipment;
    
    public IEnumerable<Equipment> GetAvailableEquipment() => _equipment.Where(e => e.IsAvailable);

    public Result RentEquipment(User user, Equipment equipment, TimeSpan duration)
    {
        if (!equipment.IsAvailable)
            return Result.Fail("sprzet niedostepny");

        var activeRentalsCount = _rentals.Count(r => r.Renter.Id == user.Id && r.IsActive);
        if (activeRentalsCount >= user.MaxActiveRentals)
            return Result.Fail($"osiagnieto limit wypozyczen: ({user.MaxActiveRentals}).");

        equipment.MarkAsUnavailable();
        var rental = new Rental(user, equipment, duration);
        _rentals.Add(rental);

        return Result.Success();
    }

    public Result ReturnEquipment(Rental rental, DateTime returnDate)
    {
        if (!rental.IsActive) 
            return Result.Fail("zakonczono wypozyczenie");

        var penalty = _penaltyCalculator.CalculatePenalty(rental.ExpectedReturnDate, returnDate);
        rental.ProcessReturn(returnDate, penalty);
        rental.RentedEquipment.MarkAsAvailable();
        
        return Result.Success();
    }
    
    public IEnumerable<Rental> GetActiveRentalsForUser(User user) => 
        _rentals.Where(r => r.Renter.Id == user.Id && r.IsActive);

    public IEnumerable<Rental> GetOverdueRentals(DateTime currentDate) => 
        _rentals.Where(r => r.IsOverdue(currentDate));

    public string GenerateReport()
    {
        var totalEquipment = _equipment.Count;
        var availableEquipment = GetAvailableEquipment().Count();
        var activeRentals = _rentals.Count(r => r.IsActive);
        
        return $"zarejestrowano: {totalEquipment} | dostepnosc: {availableEquipment} | aktywne wypozyczenia {activeRentals}";
    }
}