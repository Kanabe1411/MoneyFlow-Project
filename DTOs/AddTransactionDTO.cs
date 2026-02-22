namespace MoneyFlow.DTOs
{
    public class AddTransactionDTO
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string? Note { get; set; }
        public string SelectedType { get; set; }

    }
}
