using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System.Linq;

namespace SchoolRegister.Services.Configuration.AutoMapperProfiles;

public class MainProfile : Profile
{
    public MainProfile()
    {
        // 1. SUBJECT MAPPINGS
        CreateMap<Subject, SubjectVm>()
            .ForMember(dest => dest.TeacherName, x => x.MapFrom(src => 
                src.Teacher == null ? null : $"{src.Teacher.FirstName} {src.Teacher.LastName}"))
            .ForMember(dest => dest.Groups, x => x.MapFrom(src => src.SubjectGroups.Select(y => y.Group)));

        CreateMap<AddOrUpdateSubjectVm, Subject>();
        CreateMap<SubjectVm, AddOrUpdateSubjectVm>();

        // 2. TEACHER MAPPINGS (Naprawia błędy w TeacherServiceUnitTests)
        CreateMap<Teacher, TeacherVm>();

        // 3. GROUP MAPPINGS (Naprawia błędy w GroupServiceUnitTests)
        CreateMap<Group, GroupVm>()
            .ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))
            .ForMember(dest => dest.Subjects, x => x.MapFrom(src => src.SubjectGroups.Select(s => s.Subject)));

        CreateMap<AddOrUpdateGroupVm, Group>();

        // 4. STUDENT MAPPINGS (Dla StudentService i asercji w GroupService)
        CreateMap<Student, StudentVm>()
            .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group == null ? null : src.Group.Name))
            .ForMember(dest => dest.ParentName, x => x.MapFrom(src => 
                src.Parent == null ? null : $"{src.Parent.FirstName} {src.Parent.LastName}"));

        // 5. GRADE MAPPINGS (Naprawia błędy w GradeServiceUnitTests)
        CreateMap<AddGradeToStudentVm, Grade>();
        CreateMap<Grade, GradeVm>()
            .ForMember(dest => dest.StudentFirstName, opt => opt.MapFrom(src => src.Student.FirstName))
            .ForMember(dest => dest.StudentLastName, opt => opt.MapFrom(src => src.Student.LastName))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
    }
}