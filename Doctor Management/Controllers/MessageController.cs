using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doctor_Management.IRepository;
using Doctor_Management.Models;
using Doctor_Management.Models_View;

namespace Doctor_Management.Controllers
{
    public class MessageController : Controller
    {
        private readonly IRepositoryData<Messages> message;

        public MessageController(IRepositoryData<Messages> message)
        {
            this.message = message;
        }


        [HttpPost]
        public async Task<IActionResult> SendToDoctor(string text_message, string from)
        {
            var savemessage = new Messages
            {
                Message = text_message,
                From = from,
                date = DateTime.Now,
                ISRead = false
            };
            await message.AddAsync(savemessage);
            return Ok();
        }

        public JsonResult GetMessgaeDoctor()
        {
            var mess = message.Get(x => x.date.Date == DateTime.Now.Date && !x.ISRead && x.From == "Out").ToList();
            return Json(mess);
        }

        public JsonResult GetMessgaeDoctorRead()
        {
            var mess = message.Get(x => x.date.Date == DateTime.Now.Date && x.ISRead && x.From == "Out").ToList();
            return Json(mess);
        }

        public IActionResult DeleteAllRead()
        {
            var mess = message.Get(x => x.ISRead).ToList();
            foreach (var item in mess)
            {
                message.Delete(item);
            }
            return Ok();
        }
        public IActionResult EditeMessageDoctor(int? id)
        {
            var mss = message.Find(id);
            mss.ISRead = true;
            message.Update(mss);
            return Ok();
        }
       
    }
}
