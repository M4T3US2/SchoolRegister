using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.Model.DataModels;

public class Student : User
{
    public int? GroupId { get; set; }
    public virtual Group? Group { get; set; }
    
    public int? ParentId { get; set; }
    public virtual Parent? Parent { get; set; }

    public virtual IList<Grade> Grades { get; set; } = new List<Grade>();

    public double AverageGrade => Grades.Any() ? Grades.Average(g => (int)g.GradeValue) : 0.0;

    public IDictionary<string, double> AverageGradePerSubject => Grades
        .GroupBy(g => g.Subject.Name)
        .ToDictionary(g => g.Key, g => g.Average(x => (int)x.GradeValue));

    public IDictionary<string, List<GradeScale>> GradesPerSubject => Grades
        .GroupBy(g => g.Subject.Name)
        .ToDictionary(g => g.Key, g => g.Select(x => x.GradeValue).ToList());

    public Student() : base() { }
}