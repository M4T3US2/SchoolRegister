using AutoMapper;
using SchoolRegister.Model.DataModels;
using SchoolRegister.ViewModels.VM;
using System;
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
        
        // Mapowanie dla przypisywania przedmiotów do grup (KLUCZOWE DLA TWOICH PRZYCISKÓW)
        CreateMap<AttachDetachSubjectGroupVm, SubjectGroup>();

        // 2. USER / REGISTRATION MAPPINGS
        CreateMap<RegisterNewUserVm, User>()
            .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
            .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now)); 

        CreateMap<RegisterNewUserVm, Parent>()
            .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
            .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now)); 

        CreateMap<RegisterNewUserVm, Student>()
            .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
            .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now)); 

        CreateMap<RegisterNewUserVm, Teacher>()
            .ForMember(dest => dest.UserName, y => y.MapFrom(src => src.Email))
            .ForMember(dest => dest.RegistrationDate, y => y.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Title, y => y.MapFrom(src => src.TeacherTitles)); 

        // 3. TEACHER MAPPINGS
        CreateMap<Teacher, TeacherVm>();

        // 4. GROUP MAPPINGS
        CreateMap<Group, GroupVm>()
            .ForMember(dest => dest.Students, x => x.MapFrom(src => src.Students))
            .ForMember(dest => dest.Subjects, x => x.MapFrom(src => src.SubjectGroups.Select(s => s.Subject)));

        CreateMap<AddOrUpdateGroupVm, Group>();
        CreateMap<GroupVm, AddOrUpdateGroupVm>(); 
        CreateMap<Group, AddOrUpdateGroupVm>();   

        // 5. STUDENT MAPPINGS
        CreateMap<Student, StudentVm>()
            .ForMember(dest => dest.GroupName, x => x.MapFrom(src => src.Group == null ? null : src.Group.Name))
            .ForMember(dest => dest.ParentName, x => x.MapFrom(src => 
                src.Parent == null ? null : $"{src.Parent.FirstName} {src.Parent.LastName}"));
        
        CreateMap<AttachDetachStudentToGroupVm, Student>();
        CreateMap<StudentVm, StudentVm>();

        // 6. GRADE MAPPINGS
        CreateMap<AddGradeToStudentVm, Grade>();
        CreateMap<Grade, GradeVm>()
            .ForMember(dest => dest.StudentFirstName, opt => opt.MapFrom(src => src.Student.FirstName))
            .ForMember(dest => dest.StudentLastName, opt => opt.MapFrom(src => src.Student.LastName))
            .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject.Name));
    }
}