﻿namespace Data.Entities.Models;

public class Ins_Subject
{
    [Key]
    public int InsId { get; set; }

    [Key]
    public int SubId { get; set; }

    [ForeignKey(nameof(InsId))]
    [InverseProperty("Ins_Subjects")]
    public Instructor? instructor { get; set; }
    
    [ForeignKey(nameof(SubId))]
    [InverseProperty("Ins_Subjects")]
    public Subject? Subject { get; set; }

}