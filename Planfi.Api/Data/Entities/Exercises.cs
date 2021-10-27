using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Data.Entities
{
    public class Exercise
    {
        public Exercise()
        {
            ExerciseId = Guid.NewGuid().ToString();
            Files = new List<byte[]>();
        }

        [Key]
        public string ExerciseId { get; set; }
        [MaxLength(160)]
        public string Name { get; set; }
        [MaxLength(3600000)]
        public string Description { get; set; }
        public int Times { get; set; }
        public int Series { get; set; }
        public int Weight { get; set; }
        public int Repeats { get; set; }
        public List<byte[]>? Files { get; set; }
        public string CategoryId { get; set; }
        public string PlanId { get; set; }
    }
}