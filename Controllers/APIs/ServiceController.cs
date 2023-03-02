using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManagementApplication.Controllers.APIs
{
    [ApiController]
    [Route("api/bandr")]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceRepository serviceRepository;
        public ServiceController(IServiceRepository _serviceRepository)
        {
            serviceRepository = _serviceRepository;
        }

        // GET: api/values
        [HttpGet]
        public List<Service> Get()
        {
            return serviceRepository.GetAllServices();
        }

        // GET api/values/5
        [HttpGet("{serviceId}")]
        public Service Get(int serviceId)
        {
            
            Service? service =  serviceRepository.RetrieveService(serviceId);
            if(service == null)
            {
                service = new();
            }
            return service;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Service service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            serviceRepository.AddService(service);
            return Ok();
        }

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}

