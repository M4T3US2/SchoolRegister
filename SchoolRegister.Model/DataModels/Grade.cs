using System;
using System.ComponentModel.DataAnnotations; // Dodaj to dla [Key]
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolRegister.Model.DataModels;

public class Grade
{
    [Key] // Klucz główny jest niezbędny
    public int Id { get; set; }

    public DateTime DateOfIssue { get; set; } = DateTime.Now;
    public GradeScale GradeValue { get; set; }
    
    public int SubjectId { get; set; }
    [ForeignKey("SubjectId")]
    public virtual Subject Subject { get; set; } = null!;

    public int StudentId { get; set; }
    [ForeignKey("StudentId")]
    public virtual Student Student { get; set; } = null!;

    public Grade() { }
}