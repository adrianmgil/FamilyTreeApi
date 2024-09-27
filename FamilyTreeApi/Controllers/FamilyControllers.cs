using FamilyTreeApi.Model;
using FamilyTreeApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTreeApi.Controllers
{
    [ApiController]
    [Route("Family")]
    public class FamilyControllers : ControllerBase
    {
        private readonly IFamilyServices familyServices;

        public FamilyControllers(IFamilyServices familyServices)
        {
            this.familyServices = familyServices;
        }

        [HttpGet]
        [Route("")]
        public async Task<IEnumerable<Person>> GetAll()
        {
            return await familyServices.GetAll();
        }

        [HttpGet]
        [Route("person/{personId}")]
        public async Task<Person> GetPerson(int personId)
        {
            return await familyServices.GetPerson(personId);
        }

        [HttpPut]
        [Route("person/{personId}")]
        public async Task<IActionResult> UpdatePerson(int personId, [FromBody] Person person)
        {
            await familyServices.Update(personId, person);
            return Ok();
        }

        [HttpPost]
        [Route("person/{person}")]
        public async Task<IActionResult> CreatePerson(Person person)
        {
            await familyServices.CreatePerson(person);
            return Ok(Response);
        }

        [HttpPost]
        [Route("child/{parentId}")]
        public async Task<IActionResult> CreateChild(int parentId, [FromBody]Person person)
        {
            await familyServices.CreateChild(parentId, person);
            return Ok(Response);
        }

        [HttpPost]
        [Route("parent/{childId}")]
        public async Task<IActionResult> CreateParent(int childId, [FromBody] Person person)
        {
            await familyServices.CreateParent(childId, person);
            return Ok(Response);
        }

        [HttpPost]
        [Route("spouse/{personId}")]
        public async Task<IActionResult> CreateSpouse(int personId, [FromBody] Person person)
        {
            await familyServices.CreateSpouse(personId, person);
            return Ok(Response);
        }

        [HttpDelete]
        [Route("person/{personId}")]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            await familyServices.DeletePerson(personId);
            return Ok(Response);
        }
    }
}
