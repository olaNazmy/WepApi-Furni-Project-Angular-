using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealApiData.Models;
using RealApiData.Repository;
using System.Linq;


namespace RealApiData.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //get  repos from generic repo
        public GenericRepository<User> userRepository;
        public UsersController(GenericRepository<User> _userRepo )
        {
            userRepository = _userRepo;
        }

        //1- GET ALL USERS
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return userRepository.getall().ToList();
        }
        //2- GET BY ID
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(int id)
        {
            var user = userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;

        }
        //3- CHECK IF EXIST
        private bool UserExists(int id)
        {
            return userRepository.getall().Any(e => e.id == id);
        }

        //4- EDIT USER
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            try
            {
                userRepository.EditEntity(user);
                userRepository.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        //5- ADD USER TO TABLE
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {

            userRepository.addEntity(user);
            userRepository.Save();
            return CreatedAtAction("GetUser", new { id = user.id }, user);
        }
        //6- DELETE USER
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = userRepository.GetById(id);
            if (user == null)
            {
                return NotFound();
            }

            userRepository.deletEntity(id);
            userRepository.Save();

            return NoContent();
        }

    }
}
