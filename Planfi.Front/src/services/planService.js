import { http } from "./http.service";
import { PLANS_URL } from "./utils"

export const planService = {
  getAllPlans,
  getOrganizationPlans,
  addPlan,
  editPlan,
  getPlanById,
  deletePlans,
  assignExercises,
  unAssignExercises,
  userPlans,
  getCreatorPlans,
  };

  function getAllPlans(body) {
    return http.get(PLANS_URL, body);
  }

  function getOrganizationPlans(id) {
    return http.get(`${PLANS_URL}/organizationsplan/${id}`);
  }

  function addPlan(body) {
    return http.post(`${PLANS_URL}/create`, body);
  }

  function editPlan(id, body) {
    return http.put(`${PLANS_URL}/${id}`, body);
  }

  function getCreatorPlans(id){
    return http.get(`${PLANS_URL}/trainerplans/${id}`);
  }

  function assignExercises(body) {
    return http.post(`${PLANS_URL}/assignExercises`, body);
  }

  function unAssignExercises(body) {
    return http.post(`${PLANS_URL}/unassignExercises`, body);
  }

  function getPlanById(id) {
    return http.get(`${PLANS_URL}/${id}`);
  }

  function userPlans(id) {
    return http.get(`${PLANS_URL}/usersplan/${id}`);
  }

  function deletePlans(body) {
    return http.post(`${PLANS_URL}/delete`, body);
  }
