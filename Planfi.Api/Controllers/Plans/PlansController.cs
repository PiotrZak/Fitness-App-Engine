using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlanfiApi.Data.Entities;
using PlanfiApi.Data.ViewModels;
using PlanfiApi.Interfaces;
using WebApi.Helpers;
using WebApi.Models;

namespace PlanfiApi.Controllers.Plans
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlansController : ControllerBase
    {
        private IPlanService _planService;
        private IMapper _mapper;

        public PlansController(
            IPlanService planService,
            IMapper mapper)
        {
            _planService = planService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreatePlan model)
        {
            var plan =  _mapper.Map<Plan>(model);

            try
            {
                await _planService.Create(plan);
                return Ok(new
                {
                    plan.PlanId,
                    plan.Title,
                    plan.CreatorId,
                    plan.CreatorName,
                    plan.OrganizationId,
                });
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var plan = await _planService.GetById(id);

            if (plan == null)
                return NotFound();

            return Ok(plan);
        }

        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id,[FromForm]string title)
        {
            try
            {
                await _planService.Update(id, title);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("assignExercises")]
        public async Task<IActionResult> AssignToPlan([FromBody]AssignExerciseToPlan model)
        {
            try
            {
                await _planService.AssignExercisesToPlan(model.PlanId, model.ExerciseId, model.Series);
                return Ok();
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("unassignExercises")]
        public IActionResult UnassignToPlan([FromBody] UnAssignExerciseToPlan model)
        {
            _planService.UnassignExercisesToPlan(model.PlanId, model.ExerciseId);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("organizationsplan/{id}")]
        public IActionResult GetOrganizationPlans(string id)
        {
            var plans = _planService.GetOrganizationPlans(id);

            if (plans == null)
                return NotFound();

            // Convert it to the DTO
            var transformedPlans = _mapper.Map<List<Plan>, List<ResultPlan>>(plans.ToList());

            return Ok(transformedPlans);
        }

        [AllowAnonymous]
        [HttpGet("usersplan")]
        public async Task<IActionResult> GetUserPlans(string userId)
        {
            var plans = await _planService.GetUserPlans(userId);

            if (plans == null)
                return NotFound();
            
            return Ok(plans);
        }
        
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var plans = _planService.GetAll();
            return Ok(plans);
        }

        [AllowAnonymous]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] string[] id)
        {
            await _planService.Delete(id);
            return Ok();
        }

    }
}
