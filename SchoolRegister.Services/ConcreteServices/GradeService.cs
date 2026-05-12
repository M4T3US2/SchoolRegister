using AutoMapper;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SchoolRegister.Services.ConcreteServices;

public class GradeService : BaseService, IGradeService
{
    public GradeService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger) 
        : base(dbContext, mapper, logger) { }

    public GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm)
    {
        var gradeEntity = Mapper.Map<Grade>(addGradeToStudentVm);
        gradeEntity.DateOfIssue = DateTime.Now; // Dodajemy datę wystawienia
        DbContext.Grades.Add(gradeEntity);
        DbContext.SaveChanges();
        return Mapper.Map<GradeVm>(gradeEntity);
    }

    public IEnumerable<GradeVm> GetGradesReportForStudent(GetGradesReportVm getGradesReportVm)
    {
        var userRequesting = DbContext.Users.FirstOrDefault(u => u.Id == getGradesReportVm.GetterUserId);
        var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == getGradesReportVm.StudentId);

        if (userRequesting == null || student == null) return null;

        // Prosta logika uprawnień na potrzeby testów:
        // Jeśli pytający to ten sam uczeń, jego rodzic lub jakikolwiek nauczyciel - dajemy raport
        bool hasAccess = (userRequesting.Id == student.Id) || 
                         (userRequesting is Parent p && student.ParentId == p.Id) || 
                         (userRequesting is Teacher);

        if (!hasAccess) return null;

        var grades = DbContext.Grades.Where(g => g.StudentId == student.Id);
        return Mapper.Map<IEnumerable<GradeVm>>(grades);
    }
}