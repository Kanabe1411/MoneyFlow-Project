using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoneyFlow.Models;

public partial class Transaction
{
    [Key]
    [Column("transaction_id")]
    public int TransactionId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("date")]
    public DateOnly Date { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Transactions")]
    public virtual TransactionCategory? Category { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Transactions")]
    public virtual User? User { get; set; }
}
