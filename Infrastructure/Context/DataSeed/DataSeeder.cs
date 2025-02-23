namespace Infrastructure.Context.DataSeed;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            await SeedDepartmentsAsync(context);
            await SeedInstructorsAsync(context);
            await AssignDepartmentManagersAsync(context);
            await SeedStudentsAsync(context);
            await SeedSubjectsAsync(context);
            await SeedDepartmentSubjectsAsync(context);
            await SeedInstructorSubjectsAsync(context);
            await SeedStudentSubjectsAsync(context);

            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedDepartmentsAsync(ApplicationDBContext context)
    {
        if (!context.Departments.Any())
        {
            var departments = new[]
            {
                new Department { DNameAr = "قسم علوم الحاسب", DNameEn = "Computer Science" },
                new Department { DNameAr = "قسم الهندسة الكهربائية", DNameEn = "Electrical Engineering" }
            };

            await context.Departments.AddRangeAsync(departments);
        }
    }

    private static async Task SeedInstructorsAsync(ApplicationDBContext context)
    {
        if (!context.Instructors.Any())
        {
            var instructors = new[]
            {
                new Instructor { ENameAr = "د. محمد علي", ENameEn = "Dr. Mohamed Ali", Address = "Cairo, Egypt", Position = "Professor", Salary = 15000, DID = 1 },
                new Instructor { ENameAr = "د. أحمد حسن", ENameEn = "Dr. Ahmed Hassan", Address = "Alexandria, Egypt", Position = "Associate Professor", Salary = 12000, DID = 1 },
                new Instructor { ENameAr = "د. خالد سامي", ENameEn = "Dr. Khaled Sami", Address = "Giza, Egypt", Position = "Professor", Salary = 14000, DID = 2 }
            };

            await context.Instructors.AddRangeAsync(instructors);
        }
    }

    private static async Task AssignDepartmentManagersAsync(ApplicationDBContext context)
    {
        var compSciDept = await context.Departments.FirstOrDefaultAsync(d => d.DNameEn == "Computer Science");
        var elecEngDept = await context.Departments.FirstOrDefaultAsync(d => d.DNameEn == "Electrical Engineering");

        var instructor1 = await context.Instructors.FirstOrDefaultAsync(i => i.ENameEn == "Dr. Mohamed Ali");
        var instructor2 = await context.Instructors.FirstOrDefaultAsync(i => i.ENameEn == "Dr. Khaled Sami");

        if (compSciDept != null && instructor1 != null)
            compSciDept.InsManager = instructor1.InsId;

        if (elecEngDept != null && instructor2 != null)
            elecEngDept.InsManager = instructor2.InsId;
    }

    private static async Task SeedStudentsAsync(ApplicationDBContext context)
    {
        if (!context.Students.Any())
        {
            var students = new[]
            {
                new Student { NameAr = "علي محمود", NameEn = "Ali Mahmoud", Address = "Cairo, Egypt", Phone = "01012345678", DID = 1 },
                new Student { NameAr = "سارة أحمد", NameEn = "Sara Ahmed", Address = "Alexandria, Egypt", Phone = "01123456789", DID = 2 }
            };

            await context.Students.AddRangeAsync(students);
        }
    }

    private static async Task SeedSubjectsAsync(ApplicationDBContext context)
    {
        if (!context.Subjects.Any())
        {
            var subjects = new[]
            {
                new Subject { SubjectNameAr = "برمجة متقدمة", SubjectNameEn = "Advanced Programming", Period = 60 },
                new Subject { SubjectNameAr = "دوائر كهربائية", SubjectNameEn = "Electrical Circuits", Period = 45 }
            };

            await context.Subjects.AddRangeAsync(subjects);
        }
    }

    private static async Task SeedDepartmentSubjectsAsync(ApplicationDBContext context)
    {
        var compSciDept = await context.Departments.FirstOrDefaultAsync(d => d.DNameEn == "Computer Science");
        var elecEngDept = await context.Departments.FirstOrDefaultAsync(d => d.DNameEn == "Electrical Engineering");

        var advProgramming = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Advanced Programming");
        var electricalCircuits = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Electrical Circuits");

        if (compSciDept != null && elecEngDept != null && advProgramming != null && electricalCircuits != null)
        {
            await context.DepartmetSubjects.AddRangeAsync(new[]
            {
                new DepartmetSubject { DID = compSciDept.DID, SubID = advProgramming.SubID },
                new DepartmetSubject { DID = elecEngDept.DID, SubID = electricalCircuits.SubID }
            });
        }
    }

    private static async Task SeedInstructorSubjectsAsync(ApplicationDBContext context)
    {
        var instructor1 = await context.Instructors.FirstOrDefaultAsync(i => i.ENameEn == "Dr. Mohamed Ali");
        var instructor2 = await context.Instructors.FirstOrDefaultAsync(i => i.ENameEn == "Dr. Khaled Sami");

        var advProgramming = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Advanced Programming");
        var electricalCircuits = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Electrical Circuits");

        if (instructor1 != null && instructor2 != null && advProgramming != null && electricalCircuits != null)
        {
            await context.InstructorSubjects.AddRangeAsync(new[]
            {
                new Ins_Subject { InsId = instructor1.InsId, SubId = advProgramming.SubID },
                new Ins_Subject { InsId = instructor2.InsId, SubId = electricalCircuits.SubID }
            });
        }
    }

    private static async Task SeedStudentSubjectsAsync(ApplicationDBContext context)
    {
        var student1 = await context.Students.FirstOrDefaultAsync(s => s.NameEn == "Ali Mahmoud");
        var student2 = await context.Students.FirstOrDefaultAsync(s => s.NameEn == "Sara Ahmed");

        var advProgramming = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Advanced Programming");
        var electricalCircuits = await context.Subjects.FirstOrDefaultAsync(s => s.SubjectNameEn == "Electrical Circuits");

        if (student1 != null && student2 != null && advProgramming != null && electricalCircuits != null)
        {
            await context.StudentSubjects.AddRangeAsync(new[]
            {
                new StudentSubject { StudID = student1.StudID, SubID = advProgramming.SubID, grade = 85.5m },
                new StudentSubject { StudID = student2.StudID, SubID = electricalCircuits.SubID, grade = 90.0m }
            });
        }
    }
}
