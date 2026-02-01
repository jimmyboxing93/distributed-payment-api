using Microsoft.AspNetCore.Mvc;
using MVCProject1.Models;
using MVCProject1.UserData;
using System;

namespace MVCProject1.Controllers
{
    // Creates API endpoint
    [ApiController]
    public class ApiController : ControllerBase
    {
        private IUserInfo _userInfo;

        public ApiController(IUserInfo model)
        {
            _userInfo = model;
        }
        // Reads data using GET method
        [HttpGet]
        [Route("api/[controller]")]
        public ActionResult GetCreditCards()
        {
            
            return Ok(_userInfo.GetCreditCards());
        }

        [HttpGet]
        [Route("api/[controller]/{id}")]
        public ActionResult GetCreditCard(Guid id)
        {
            var user = _userInfo.GetCreditCard(id);

            if (user != null)
            {
                return Ok(user);
            }

            return NotFound($"User with Id: {id} was not found");
        }

        // Writes data using post method
        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult Payment(UserInfo model)
        {
            _userInfo.AddCreditCard(model);
            return Created(HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path + "/" + model.UserID,
                model);

           
        }
        // Edits data using put method
        [HttpPut]
        [Route("api/[controller]/{id}")]
        public IActionResult EditCreditCard(Guid id, UserInfo model)
        {
            var existing_user = _userInfo.GetCreditCard(id);

            if (existing_user != null)
            {
                model.UserID = existing_user.UserID;
                _userInfo.EditCreditCard(model);
               
            }
            return Ok(model);
        }
        // Deletes data using delete method
        
        [HttpDelete]
        [Route("api/[controller]/{id}")]
        public IActionResult DeleteCreditCard(Guid id)
        {
            var user = _userInfo.GetCreditCard(id);

            if (user != null)
            {
                _userInfo.DeleteCreditCard(user);
                return Ok();
            }
            return NotFound($"User with Id: {id} was not found");

        }

        


    }
}
