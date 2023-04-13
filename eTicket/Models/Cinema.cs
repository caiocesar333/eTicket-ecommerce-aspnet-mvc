using System.ComponentModel.DataAnnotations;

namespace eTicket.Models
{
    public class Cinema
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Logo")]
        public string Logo { get; set; }


        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Description")]
        public string Description { get; set; }
    }
}
