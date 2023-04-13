using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        //adding key here allows the property below to be the primary key
        //with entity framework ID will automatically be the primary key
        //also if the name is the name of the class plus ID it will also be made as the primary key automatically = CategoryId
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        public int DisplayOrder { get; set; }
    }
}
