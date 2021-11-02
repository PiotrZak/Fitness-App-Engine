﻿using System.Collections.Generic;
using System.Linq;
using HotChocolate;
using HotChocolate.Types;
using PlanfiApi.Data.Entities;
using PlanfiApi.Interfaces;
using PlanfiApi.Models.ViewModels;
using PlanfiApi.Services.Exercises;
using WebApi.Data.Entities;
using WebApi.Helpers;
using WebApi.Interfaces;
using WebApi.Models.ViewModels;

namespace PlanfiApi.GraphQl
{

    public class Query
    {
        private readonly ICategoryService _categoryService;
        private readonly IPlanService _planService;
        private readonly IExerciseService _exerciseService;
        private readonly IOrganizationService _organizationService;
        
        public Query(
            ICategoryService categoryService,
            IPlanService planService,
            IExerciseService exerciseService, 
            IOrganizationService organizationService
            )
        {
            _categoryService = categoryService ;
            _planService = planService ;
            _exerciseService = exerciseService;
            _organizationService = organizationService;
        }
        
        [UseFiltering]
        public Exercise GetExercise(
           [Service] DataContext dbContext, string id) =>
               dbContext.exercises.FirstOrDefault(x => x.ExerciseId == id);
        
        [UseFiltering]
        public List<CategoryViewModel> GetCategories([Service] DataContext dbContext) =>
            _categoryService.GetAll().ToList();
        
        [UseFiltering]
        public List<Plan> GetPlans([Service] DataContext dbContext) =>
            _planService.GetAll().ToList();
        
        [UseFiltering]
        public List<ExerciseViewModel> GetSerializedExercises([Service] DataContext dbContext) => 
            _exerciseService.GetSerializedExercises().ToList();
        
        [UseFiltering]
        public List<ExerciseViewModel> GetSerializedExercisesInstances([Service] DataContext dbContext) => 
            _exerciseService.GetSerializedExercisesInstances().ToList();
        
        [UseFiltering]
        public List<UserViewModel> GetUsers([Service] DataContext dbContext) => 
            _organizationService.GetUsers().ToList();
    }
    
    public static class ExtensionMethods
    {
        public static IEnumerable<Exercise> WithoutFiles(this IEnumerable<Exercise> exercises)
        {
            if (exercises == null) return null;
            return exercises.Select(x => x.WithoutFile());
        }

        public static Exercise WithoutFile(this Exercise exercise)
        {
            if (exercise == null) return null;

            exercise.Files = null;
            return exercise;
        }
    }

}
