using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PlanfiApi.Models.SqlProjections;
using WebApi.Models;

namespace PlanfiApi.Models
{
    public class UserDetails
    {
        [Key]
        public string UserId { get; set; }
        public byte[]? Avatar { get; set; }
        public string? Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string OrganizationId { get; set; }   
        public List<ResultPlan> UserPlans;
        public List<UserSqlProjection> ClientTrainers;
    }
}