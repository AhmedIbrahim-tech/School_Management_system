﻿namespace Data.Entities.Models;

public partial class Department : GeneralLocalizeEntity
{
    public Department()
    {
        Students = new HashSet<Student>();
        DepartmentSubjects = new HashSet<DepartmetSubject>();
        Instructors = new HashSet<Instructor>();
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DID { get; set; }

    public string? DNameAr { get; set; }

    [StringLength(200)]
    public string? DNameEn { get; set; }

    public int? InsManager { get; set; }

    [InverseProperty("Department")]
    public virtual ICollection<Student> Students { get; set; }
    [InverseProperty("Department")]
    public virtual ICollection<DepartmetSubject> DepartmentSubjects { get; set; }
    [InverseProperty("department")]
    public virtual ICollection<Instructor> Instructors { get; set; }

    [ForeignKey("InsManager")]
    [InverseProperty("departmentManager")]
    public virtual Instructor? Instructor { get; set; }
}