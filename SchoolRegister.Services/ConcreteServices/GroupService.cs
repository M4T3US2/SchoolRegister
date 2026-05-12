using AutoMapper;
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

namespace SchoolRegister.Services.ConcreteServices
{
    public class GroupService : BaseService, IGroupService
    {
        public GroupService(ApplicationDbContext dbContext, IMapper mapper, ILogger logger)
            : base(dbContext, mapper, logger) { }

        public GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate)
        {
            var group = DbContext.Groups.FirstOrDefault(filterPredicate);
            return Mapper.Map<GroupVm>(group);
        }

        public IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterPredicate = null)
        {
            var groups = DbContext.Groups.AsQueryable();
            if (filterPredicate != null)
                groups = groups.Where(filterPredicate);
            return Mapper.Map<IEnumerable<GroupVm>>(groups);
        }

        public GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm)
        {
            var groupEntity = Mapper.Map<Group>(addOrUpdateGroupVm);
            if (addOrUpdateGroupVm.Id == 0)
                DbContext.Groups.Add(groupEntity);
            else
                DbContext.Groups.Update(groupEntity);
            DbContext.SaveChanges();
            return Mapper.Map<GroupVm>(groupEntity);
        }

        public StudentVm AttachStudentToGroup(AttachDetachStudentToGroupVm vm)
        {
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId);
            if (student != null)
            {
                student.GroupId = vm.GroupId;
                DbContext.SaveChanges();
            }
            return Mapper.Map<StudentVm>(student);
        }

        public StudentVm DetachStudentFromGroup(AttachDetachStudentToGroupVm vm)
        {
            var student = DbContext.Users.OfType<Student>().FirstOrDefault(s => s.Id == vm.StudentId);
            if (student != null)
            {
                student.GroupId = null;
                DbContext.SaveChanges();
            }
            return Mapper.Map<StudentVm>(student);
        }

        public GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm vm)
        {
            var subjectGroup = new SubjectGroup { GroupId = vm.GroupId, SubjectId = vm.SubjectId };
            DbContext.SubjectGroups.Add(subjectGroup);
            DbContext.SaveChanges();
            return GetGroup(g => g.Id == vm.GroupId);
        }

        public GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm vm)
        {
            var subjectGroup = DbContext.SubjectGroups.FirstOrDefault(sg => sg.GroupId == vm.GroupId && sg.SubjectId == vm.SubjectId);
            if (subjectGroup != null)
            {
                DbContext.SubjectGroups.Remove(subjectGroup);
                DbContext.SaveChanges();
            }
            return GetGroup(g => g.Id == vm.GroupId);
        }

        public SubjectVm AttachTeacherToSubject(AttachDetachSubjectToTeacherVm vm)
        {
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId);
            if (subject != null)
            {
                subject.TeacherId = vm.TeacherId;
                DbContext.SaveChanges();
            }
            return Mapper.Map<SubjectVm>(subject);
        }

        public SubjectVm DetachTeacherFromSubject(AttachDetachSubjectToTeacherVm vm)
        {
            var subject = DbContext.Subjects.FirstOrDefault(s => s.Id == vm.SubjectId);
            if (subject != null)
            {
                subject.TeacherId = null;
                DbContext.SaveChanges();
            }
            return Mapper.Map<SubjectVm>(subject);
        }
    }
}