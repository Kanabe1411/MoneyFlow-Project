using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoneyFlow.Models;

public partial class BudgetCategory
{
    [Key]
    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }

    [Column("icon")]
    [StringLength(255)]
    [Unicode(false)]
    public string? Icon { get; set; }

    [Column("is_deleted")]
    public bool? IsDeleted { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
