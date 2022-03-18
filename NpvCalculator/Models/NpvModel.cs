namespace NpvCalculator.Models
{
    public class NpvModel
    {
        public decimal Npv { get; set; }
        public decimal DiscountRate { get; set; }
        public bool IsLoss { get; set; }
    }
}