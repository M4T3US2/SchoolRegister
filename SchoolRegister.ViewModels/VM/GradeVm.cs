using System;
using SchoolRegister.Model.DataModels;

namespace SchoolRegister.ViewModels.VM;

public class GradeVm
{
    public int StudentId { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }
    public GradeScale GradeValue { get; set; }
    public DateTime DateOfIssue { get; set; }
}