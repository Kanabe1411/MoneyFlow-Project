using Microsoft.EntityFrameworkCore;
using MoneyFlow.data;
using MoneyFlow.DTOs;
using MoneyFlow.Models;

namespace MoneyFlow.Services;

public class TransactionHistoryService
{
    private readonly AppDbContext _context;

    public TransactionHistoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DateTime> GetUserStartDateAsync(int userId)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        return user?.CreatedAt ?? new DateTime(2000, 1, 1);
    }

    public async Task<TransactionHistorySummaryDTO> GetHistoryAsync(int userId, DateTime month)
    {
        var startDate = new DateOnly(month.Year, month.Month, 1);
        var endDate = startDate.AddMonths(1);

        var data = await _context.Transactions
            .Include(t => t.Category)
            .Where(t => t.UserId == userId
                        && t.Date >= startDate
                        && t.Date < endDate)
            .OrderByDescending(t => t.Date)
            .ToListAsync();

        var summary = new TransactionHistorySummaryDTO
        {
            TotalIn = data.Where(t => t.Category?.Type?.Equals("income", StringComparison.OrdinalIgnoreCase) == true).Sum(t => t.Amount),
            TotalOut = data.Where(t => t.Category?.Type?.Equals("expense", StringComparison.OrdinalIgnoreCase) == true).Sum(t => t.Amount),
            Transactions = data.Select(t => MapToDisplayDTO(t)).ToList()
        };
        return summary;
    }

    private TransactionDisplayDTO MapToDisplayDTO(Transaction t)
    {
        bool isIncome = t.Category?.Type?.Equals("income", StringComparison.OrdinalIgnoreCase) == true;
        return new TransactionDisplayDTO
        {
            TransactionId = t.TransactionId,
            CategoryName = t.Category?.Name ?? "Uncategorized",
            Note = string.IsNullOrEmpty(t.Note) ? "No description" : t.Note,
            Amount = t.Amount,
            AmountText = (isIncome ? "+" : "-") + t.Amount.ToString("N0") + "₫",
            AmountColor = isIncome ? "#2E9B6A" : "#E05252",
            CategoryIcon = t.Category?.Icon ?? "??",
            IsImageIcon = t.Category?.Icon is string icon && (icon.Contains('.') || icon.Contains('/')),
            Date = t.Date,
            // Nếu CreatedAt có giá trị thì lấy HH:mm, nếu null thì để chuỗi rỗng hoặc mặc định
            DateText = t.CreatedAt?.ToString("HH:mm") ?? ""
        };
    }
}