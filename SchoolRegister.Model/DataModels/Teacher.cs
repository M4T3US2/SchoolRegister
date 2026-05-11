using System.Collections.Generic;

namespace SchoolRegister.Model.DataModels;

public class Teacher : User
{
    public string Title { get; set; } = null!;
    public virtual IList<Subject> Subjects { get; set; } = new List<Subject>();

    public Teacher() : base() { }
}