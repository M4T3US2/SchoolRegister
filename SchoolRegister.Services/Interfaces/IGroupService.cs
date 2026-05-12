using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces;

public interface IGroupService {
    GroupVm GetGroup(Expression<Func<Group, bool>> filterPredicate);
    IEnumerable<GroupVm> GetGroups(Expression<Func<Group, bool>> filterPredicate = null);
    GroupVm AddOrUpdateGroup(AddOrUpdateGroupVm addOrUpdateGroupVm);
    StudentVm AttachStudentToGroup(AttachDetachStudentToGroupVm attachStudentToGroupVm);
StudentVm DetachStudentFromGroup(AttachDetachStudentToGroupVm detachStudentToGroupVm);
GroupVm AttachSubjectToGroup(AttachDetachSubjectGroupVm attachSubjectGroupVm);
GroupVm DetachSubjectFromGroup(AttachDetachSubjectGroupVm detachSubjectGroupVm);
SubjectVm AttachTeacherToSubject(AttachDetachSubjectToTeacherVm attachSubjectTeacher);
SubjectVm DetachTeacherFromSubject(AttachDetachSubjectToTeacherVm detachSubjectTeacher);
}