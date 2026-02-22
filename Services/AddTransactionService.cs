using MoneyFlow.DTOs;
using MoneyFlow.Models;
using MoneyFlow.data;
using Microsoft.EntityFrameworkCore;

namespace MoneyFlow.Services
{
    public class AddTransactionService
    {
        private readonly AppDbContext _db;

        public AddTransactionService(AppDbContext db) => _db = db;

        // Bê từ LoadCategoriesAsync sang
        public async Task<List<TransactionCategory>> GetCategoriesByTypeAsync(string type)
        {
            return await _db.TransactionCategories
                .Where(c => c.IsDeleted != true && c.Type == type)
                .OrderBy(c => c.Name).ToListAsync();
        }

        // Bê từ DoSaveAsync sang
        public async Task SaveTransactionAsync(int userId, AddTransactionDTO dto)
        {
            var transaction = new Transaction
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Date = DateOnly.FromDateTime(dto.Date),
                Note = string.IsNullOrWhiteSpace(dto.Note) ? "None" : dto.Note.Trim(),
                Status = "completed",
                CreatedAt = DateTime.Now
            };
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }

        // Bê từ CheckBudgetAsync sang
        public async Task<string?> GetBudgetWarningAsync(int userId, int categoryId, DateTime date)
        {
            int m = date.Month, y = date.Year;
            var budget = await _db.Budgets.FirstOrDefaultAsync(b =>
                b.UserId == userId && b.CategoryId == categoryId && b.Month == m && b.Year == y);

            if (budget == null) return null;

            var spent = await _db.Transactions
                .Where(t => t.UserId == userId && t.CategoryId == categoryId && t.Date.Month == m && t.Date.Year == y)
                .SumAsync(t => (decimal?)t.Amount) ?? 0;

            var pct = budget.Amount > 0 ? (spent / budget.Amount) * 100 : 0;
            if (pct >= 100) return $"Vượt ngân sách! {spent:N0} / {budget.Amount:N0} ₫ ({pct:F0}%)";
            if (pct >= 80) return $"Gần giới hạn. {spent:N0} / {budget.Amount:N0} ₫ ({pct:F0}%)";
            return null;
        }
    }
}
