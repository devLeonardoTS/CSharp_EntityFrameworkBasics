using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMasterTutorial.Model
{
    public class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Attribute that makes sure the decorated property is a Primary Key.
        public int Id { get; set; } // We can ommit the above attribute in this case, since we are using the "Id" name, and it is understood by Entity Framework as the Primary Key of this entity.
        public string Name { get; set; }
    }
}
