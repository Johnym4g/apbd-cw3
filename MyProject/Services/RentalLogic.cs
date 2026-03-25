using MyProject.Models;

namespace MyProject.Services;

public class RentalLogic
{
    private readonly List<User> _users = new();
    private readonly List<Equipment> _equipment = new();
    private readonly List<Models.Rental> _rentals = new();
    private readonly IPenaltyCalculator _penaltyCalculator;

    public RentalLogic(IPenaltyCalculator penaltyCalculator)
    {
        _penaltyCalculator = penaltyCalculator ?? throw new ArgumentNullException(nameof(penaltyCalculator));
    }

    public void AddUser(User user)
    {
        if (user != null && !_users.Any(u => u.Id == user.Id))
            _users.Add(user);
    }
    
    public void AddEquipment(Equipment item)
    {
        if (item != null && !_equipment.Any(e => e.Id == item.Id))
            _equipment.Add(item);
    }

    public IEnumerable<Equipment> GetAllEquipment() => _equipment;
    
    public IEnumerable<Equipment> GetAvailableEquipment() => _equipment.Where(e => e.IsAvailable);

    public Result RentEquipment(User user, Equipment equipment, TimeSpan duration)
    {
        if (user == null) return Result.Fail("uzytkownik nie mzoe byc null");
        if (equipment == null) return Result.Fail("sprzet nie moze byc null");
        if (duration.TotalMinutes <= 0) return Result.Fail("czas trwania wypozyczenia musi byc dodatni");
        if (!_users.Contains(user)) return Result.Fail("niezarejestrowany uzytkownik");
        if (!_equipment.Contains(equipment)) return Result.Fail("niezajestrowany sprzet");

        if (!equipment.IsAvailable)
            return Result.Fail("wybrany sprzet jest aktualnie niedostepny");
        
        var activeRentalsCount = _rentals.Count(r => r.Renter.Id == user.Id && r.IsActive);
        if (activeRentalsCount >= user.MaxActiveRentals)
            return Result.Fail($"osiagnieto limit wypozyczen: ({user.MaxActiveRentals}).");

        equipment.MarkAsUnavailable();
        var rental = new Models.Rental(user, equipment, duration);
        _rentals.Add(rental);

        return Result.Success();
    }

    public Result ReturnEquipment(Models.Rental rental, DateTime returnDate)
    {
        if (rental == null) return Result.Fail("brak danych o wypozyczeniu");
        if (!_rentals.Contains(rental)) return Result.Fail("nie znaleziono wypozyczenia");
        if (returnDate < rental.RentDate) return Result.Fail("data zwrotu nie moze byc wczesniejsza wypozyczenia");

        if (!rental.IsActive) 
            return Result.Fail("zakonczono wypozyczenie");

        var penalty = _penaltyCalculator.CalculatePenalty(rental.ExpectedReturnDate, returnDate);
        rental.ProcessReturn(returnDate, penalty);
        rental.RentedEquipment.MarkAsAvailable();
        
        return Result.Success();
    }
    
    public IEnumerable<Models.Rental> GetActiveRentalsForUser(User user)
    {
        if (user == null) return Enumerable.Empty<Models.Rental>();
        return _rentals.Where(r => r.Renter.Id == user.Id && r.IsActive);
    }

    public IEnumerable<Models.Rental> GetOverdueRentals(DateTime currentDate) => 
        _rentals.Where(r => r.IsOverdue(currentDate));

    public string GenerateReport()
    {
        var totalEquipment = _equipment.Count;
        var availableEquipment = GetAvailableEquipment().Count();
        var activeRentals = _rentals.Count(r => r.IsActive);
        
        return $"zarejestrowano: {totalEquipment} | dostepnosc: {availableEquipment} | aktywne wypozyczenia {activeRentals}";
    }
}