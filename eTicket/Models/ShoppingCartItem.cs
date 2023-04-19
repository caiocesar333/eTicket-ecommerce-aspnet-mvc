using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eTicket.Models
{
    public class ShoppingCartItem
    {
        [Key]

        public int Id { get; set; }

        public Movie Movie{ get; set; }

        public int MovieId { get; set; }

        public int Amount { get; set; }

        public string ShoppingCartId { get; set; }
    }
}
