using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolRegister.Model.DataModels;

public class Subject {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    // Pytajnik jest kluczowy, bo testy wysyłają tu null
    public string? Description { get; set; } 
    public int? TeacherId { get; set; }
    public virtual Teacher Teacher { get; set; } = null!;
    // ... reszta kolekcji


    public virtual IList<SubjectGroup> SubjectGroups { get; set; } = new List<SubjectGroup>();
    public virtual IList<Grade> Grades { get; set; } = new List<Grade>();

    public Subject() { }}