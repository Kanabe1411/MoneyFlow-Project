namespace MoneyFlow.DTOs;

public class TransactionDisplayDTO
{
    public int TransactionId { get; set; }
    public string CategoryName { get; set; } = "Uncategorized";
    public string Note { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string AmountText { get; set; } = string.Empty;
    public string AmountColor { get; set; } = "#000000";
    public string CategoryIcon { get; set; } = "??";
    public bool IsImageIcon { get; set; }
    public string DateText { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TransactionHistorySummaryDTO
{
    public decimal TotalIn { get; set; }
    public decimal TotalOut { get; set; }
    public decimal Balance => TotalIn - TotalOut;
    public List<TransactionDisplayDTO> Transactions { get; set; } = [];
}
