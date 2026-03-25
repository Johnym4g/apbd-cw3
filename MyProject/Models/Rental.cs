    namespace MyProject.Models;

    public class Rental
    {
        public Guid Id { get; private set; }
        public User Renter { get; private set; }
        public Equipment RentedEquipment { get; private set; }
        public DateTime RentDate { get; private set; }
        public DateTime ExpectedReturnDate { get; private set; }
        public DateTime? ActualReturnDate { get; private set; }
        public decimal PenaltyAmount { get; private set; }

        public Rental(User renter, Equipment equipment, TimeSpan rentDuration)
        {
            Id = Guid.NewGuid();
            Renter = renter;
            RentedEquipment = equipment;
            RentDate = DateTime.Now;
            ExpectedReturnDate = RentDate.Add(rentDuration);
            PenaltyAmount = 0;
        }

        public void ProcessReturn(DateTime returnDate, decimal calculatedPenalty)
        {
            ActualReturnDate = returnDate;
            PenaltyAmount = calculatedPenalty;
        }

        public bool IsOverdue(DateTime currentDate)
        {
            return !ActualReturnDate.HasValue && currentDate > ExpectedReturnDate;
        }
        
        public bool IsActive => !ActualReturnDate.HasValue;
    }