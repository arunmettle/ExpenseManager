using ExpenseManager.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Models
{
    public class SpendLimit
    {
        [Key]
        public int ItemId { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime EndDate { get; set; } = DateTime.Now;

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(10, 2)")]
        [Required]
        public decimal SpendingCap { get; set; } 

        [DataType(DataType.Text)]
        [Required]
        public bool IsWeekly { get; set; }
        public virtual ApplicationrUser ApplicationrUser { get; set; }

        public string UserName { get; set; }
    }
}
