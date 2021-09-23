using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMasterTutorial.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DueDate { get; set; }

        public int StatusId { get; set; }   // Status followed by Id is a naming convention that will help E.F. to interpret this property as the received Foreign Key of the Status Entity.
        public Status Status { get; set; } // This is what is called a Navigation Property, it will help us chain objects, using standard object oriented notation. Do some research for more info on this topic.
    }
}
