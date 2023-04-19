namespace eTicket.Data.ViewModels
{
    using eTicket.Data.Base;
    using eTicket.Data.Enum;
    using eTicket.Models;
    using global::eTicket.Data.Enum;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

        public class NewMovieVM
        {
             public int Id { get; set; }

            [Display(Name = "Movie Name")]
            [Required(ErrorMessage = "Name is Required")]
            public string Name { get; set; }

            [Display(Name = "Movie Description")]
            [Required(ErrorMessage = "Name is Required")]
            public string Description { get; set; }

            [Display(Name = "Price")]
            [Required(ErrorMessage = "Price is Required")]
            public double Price { get; set; }

            [Display(Name = "Movie Poster URL")]
            [Required(ErrorMessage = "Image URL is Required")]
            public string ImageURL { get; set; }

            [Display(Name = "Movie Start Date")]
            [Required(ErrorMessage = "Start Date is Required")]
            public DateTime StartDate { get; set; }

            [Display(Name = "Movie End Date")]
            [Required(ErrorMessage = "End Date is Required")]
            public DateTime EndDate { get; set; }

            [Display(Name = "Select Category")]
            [Required(ErrorMessage = "Category is Required")]
            public MovieCategory MovieCategory { get; set; }

            //Relationships

            [Display(Name = "Select Actor(s)")]
            [Required(ErrorMessage = "Movie Actor(s) is Required")]
            public List<int> ActorIds { get; set; }

            [Display(Name = "Select Cinema")]
            [Required(ErrorMessage = "Movie Cinema is Required")]
            public int CinemaId { get; set; }

            [Display(Name = "Select Producer")]
            [Required(ErrorMessage = "Movie Producer is Required")]
            public int ProducerId { get; set; }


        }
    }

