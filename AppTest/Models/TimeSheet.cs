using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AppTest.Models
{
    public class TimeSheet
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("employee")]
        public string IdEmployment { get; set; }
        public Employee? employee { get; set; }

        [Required]
        public DateTime Start { get; set; }
        public DateTime? BreakStart { get; set; }
        public DateTime? BreakEnd { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}
