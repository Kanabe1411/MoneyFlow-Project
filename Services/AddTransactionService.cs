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

        public async Task<List<TransactionCategory>> GetCategoriesByTypeAsync(string type)
        {
            return await _db.TransactionCategories
                .Where(c => c.IsDeleted != true && c.Type == type)
                .OrderBy(c => c.Name).ToListAsync();
        }

        public async Task SaveTransactionAsync(int userId, AddTransactionDTO dto)
        {
            var transaction = new Transaction
            {
                UserId = userId,
                CategoryId = dto.CategoryId,
                Amount = dto.Amount,
                Date = DateOnly.FromDateTime(dto.Date),
                Note = string.IsNullOrWhiteSpace(dto.Note) ? "None" : dto.Note.Trim(),
                Status = "completed",// mặc định là completed
                CreatedAt = DateTime.Now
            };
            _db.Transactions.Add(transaction);
            await _db.SaveChangesAsync();
        }
    }
}
