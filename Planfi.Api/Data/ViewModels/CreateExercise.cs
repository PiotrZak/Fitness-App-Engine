﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PlanfiApi.Data.ViewModels
{
    public class 
        CreateExercise
    {

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<IFormFile>? Files { get; set; }
        public string CategoryId { get; set; }
    }
}
