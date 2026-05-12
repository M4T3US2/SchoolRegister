using System.Collections.Generic;
using SchoolRegister.ViewModels.VM;

namespace SchoolRegister.Services.Interfaces;

public interface IGradeService
{
    GradeVm AddGradeToStudent(AddGradeToStudentVm addGradeToStudentVm);
    IEnumerable<GradeVm> GetGradesReportForStudent(GetGradesReportVm getGradesReportVm);
}