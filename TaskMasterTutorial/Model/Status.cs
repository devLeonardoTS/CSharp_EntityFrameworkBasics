using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMasterTutorial.Model
{
    public class Status
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // Attribute that makes sure the decorated property is a Primary Key.
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
