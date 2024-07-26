using System;
using System.ComponentModel.DataAnnotations;

namespace ConstructionProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectName { get; set; }

        [Required]
        [MaxLength(500)]
        public string ProjectLocation { get; set; }

        [Required]
        public string ProjectStage { get; set; }

        [Required]
        public string ProjectCategory { get; set; }

        [Required]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        [MaxLength(2000)]
        public string ProjectDetails { get; set; }

        [Required]
        public int ProjectCreatorId { get; set; }
    }
}
