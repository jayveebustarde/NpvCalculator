namespace NpvCalculator.Models
{
    public class NpvRequestModel
    {
        public decimal LowerBoundRate { get; set; }
        public decimal UpperBoundRate { get; set; }
        public decimal Increment { get; set; }
        public decimal[] CashFlows { get; set; }
        public decimal InitialAmount { get; set; }
    }
}