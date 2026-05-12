using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Model.DataModels;

public class Group
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public virtual IList<Student> Students { get; set; } = new List<Student>();
    public virtual IList<SubjectGroup> SubjectGroups { get; set; } = new List<SubjectGroup>();

    public Group() { }
}