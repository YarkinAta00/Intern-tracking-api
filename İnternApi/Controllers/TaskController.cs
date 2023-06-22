using System;
using System.Reflection;
using System.Threading.Tasks;
using İnternApi.Data;
using İnternApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace İnternApi.Controllers
{
   
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
     
   private readonly ApplicationDBContext _applicationDBContext;
      
   public TaskController(ApplicationDBContext applicationDBContext)
   {
       this._applicationDBContext = applicationDBContext;
   }

  //Taskları Gösterme İşlemi
  [HttpGet("GetTasks")]
  public IActionResult GetTasks()
   {
            return Ok(_applicationDBContext.tasks.ToList());
   }

   //Task Ekleme İşlemi
   [HttpPost("SendTask")]
   public IActionResult SendTask(Tasks task)
    {
            _applicationDBContext.tasks.Add(task);
            _applicationDBContext.SaveChanges();
            return CreatedAtAction(nameof(GetTasks), new { id = task.TaskId}, task);
        }
    //ID'si Verilen Taskı Gösterme İşlemi
    [HttpGet("GetTaskByID")]
   public IActionResult GetTaskByID(int id)
     {
         var task = _applicationDBContext.tasks.Where(x => x.TaskId == id).FirstOrDefault();
         if (task == null)
        {
            return NotFound();
        }
            return Ok(task);
        }
   //Adı Verilen Taskı Gösterme İşlemi
 [HttpGet("GetTaskByName")]
 public IActionResult GetTaskByName(string taskName)
        {
            var task = _applicationDBContext.tasks.Where(x => x.TaskName == taskName).FirstOrDefault();
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }
        //Adı Verilen Taskı Gösterme İşlemi
      [HttpPut("UpdateTask")]
        public string UpdateTaskByID(int id, string taskName, string sender, string details,DateTime startDate, DateTime endDate)
        {
            var task = _applicationDBContext.tasks.Where(x => x.TaskId == id).FirstOrDefault();
            if (task == null)
            {
                return "task Not Found!";
            }
            task.TaskName = taskName;
            task.TaskSender = sender;
            task.TaskDetails = details;
            task.TaskEndTime = startDate;
            task.TaskEndTime = endDate;
            _applicationDBContext.SaveChanges();
            return "Task Updated";
        }
        //Task Ekleme İşlemi
        [HttpPost("CreateTask")]
        public IActionResult CreateTask(string taskName, string sender, string details,DateTime endDate)
        {
            Tasks task = new Tasks();
            task.TaskName = taskName;
            task.TaskSender = sender;
            task.TaskDetails = details;
            task.UserID = 0;
            task.TaskStartTime = DateTime.Now.Date;
            task.TaskEndTime =endDate.Date;
            task.IsCompleted = false;
            _applicationDBContext.tasks.Add(task);
            _applicationDBContext.SaveChanges();
            return CreatedAtAction(nameof(GetTasks), new { id = task.TaskId }, task);
        }
        //Task Silme İşlemi
        [HttpDelete("DeleteTask")]
        public string DeleteTask(int id)
        {
            var task = _applicationDBContext.tasks.Where(x => x.TaskId == id).FirstOrDefault();
            _applicationDBContext.tasks.Remove(task);
            _applicationDBContext.SaveChanges();
            return "User Deleted";
        }
        [HttpPut("CompleteTask")]
        public string CompleteTask(int id)
        {
            var task = _applicationDBContext.tasks.Where(x => x.TaskId == id).FirstOrDefault();
            if (task == null)
            {
                return "task Not Found!";
            }
            task.IsCompleted = true;
            _applicationDBContext.SaveChanges();
            return "User Deleted";
        }

    }
}

