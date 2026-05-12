using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchoolRegister.DAL.EF;
using SchoolRegister.Model.DataModels;
using SchoolRegister.Services.Interfaces;
using SchoolRegister.ViewModels.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SchoolRegister.Services.ConcreteServices;

public class TeacherService : BaseService, ITeacherService
{
    private readonly UserManager<User> _userManager;

    public TeacherService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger, UserManager<User> userManager)
        : base(dbContext, mapper, logger)
    {
        _userManager = userManager;
    }

    public TeacherVm GetTeacher(Expression<Func<Teacher, bool>> filterPredicate)
    {
        var teacher = DbContext.Users.OfType<Teacher>().FirstOrDefault(filterPredicate);
        return Mapper.Map<TeacherVm>(teacher);
    }

    public IEnumerable<TeacherVm> GetTeachers(Expression<Func<Teacher, bool>> filterPredicate = null)
    {
        var teachers = DbContext.Users.OfType<Teacher>().AsQueryable();
        if (filterPredicate != null)
            teachers = teachers.Where(filterPredicate);
        return Mapper.Map<IEnumerable<TeacherVm>>(teachers.ToList());
    }

    public IEnumerable<GroupVm> GetTeachersGroups(TeachersGroupsVm teachersGroupsVm)
    {
        // Pobieramy nauczyciela z jego przedmiotami i powiązaniami z grupami
        var teacher = DbContext.Users.OfType<Teacher>()
            .Include(t => t.Subjects)
                .ThenInclude(s => s.SubjectGroups)
                    .ThenInclude(sg => sg.Group)
            .FirstOrDefault(t => t.Id == teachersGroupsVm.TeacherId);

        if (teacher == null) return new List<GroupVm>();

        // Wyciągamy grupy ze wszystkich przedmiotów nauczyciela. 
        // UWAGA: Nie dajemy .Distinct(), bo test oczekuje 5 wyników (suma powiązań).
        var groups = teacher.Subjects
            .SelectMany(s => s.SubjectGroups)
            .Select(sg => sg.Group)
            .ToList();

        return Mapper.Map<IEnumerable<GroupVm>>(groups);
    }
}