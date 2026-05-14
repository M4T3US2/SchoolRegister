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
        gradeEntity.DateOfIssue = DateTime.Now;
        DbContext.Grades.Add(gradeEntity);
        DbContext.SaveChanges();

        var savedGrade = DbContext.Grades
            .Where(g => g.Id == gradeEntity.Id)
            .FirstOrDefault();

        return Mapper.Map<GradeVm>(savedGrade);
    }

    public IEnumerable<GradeVm> GetGradesReportForStudent(GetGradesReportVm getGradesReportVm)
    {
        var userRequesting = DbContext.Users.FirstOrDefault(u => u.Id == getGradesReportVm.GetterUserId);
        var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == getGradesReportVm.StudentId);

       
        if (userRequesting == null || student == null)
            return Enumerable.Empty<GradeVm>();

        bool hasAccess = (userRequesting.Id == student.Id)
            || (userRequesting is Parent p && student.ParentId == p.Id)
            || (userRequesting is Teacher)
            || DbContext.UserRoles
                .Any(ur => ur.UserId == userRequesting.Id
                    && DbContext.Roles.Any(r => r.Id == ur.RoleId && r.Name == "Admin"));

        if (!hasAccess)
            return Enumerable.Empty<GradeVm>();

        var grades = DbContext.Grades.Where(g => g.StudentId == student.Id);
        return Mapper.Map<IEnumerable<GradeVm>>(grades);
    }
}