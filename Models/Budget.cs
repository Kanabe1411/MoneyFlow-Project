using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoneyFlow.Models;

public partial class Budget
{
    [Key]
    [Column("budget_id")]
    public int BudgetId { get; set; }

    [Column("user_id")]
    public int? UserId { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("amount", TypeName = "decimal(18, 2)")]
    public decimal Amount { get; set; }

    [Column("allocated", TypeName = "decimal(18, 2)")]
    public decimal Allocated { get; set; }

    [Column("month")]
    public int? Month { get; set; }

    [Column("year")]
    public int? Year { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("auto_renew")]
    public bool? AutoRenew { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Budgets")]
    public virtual BudgetCategory? Category { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Budgets")]
    public virtual User? User { get; set; }
}
