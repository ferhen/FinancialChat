using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialChat.Models;
using FinancialChat.Services;
using FinancialChat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatroomController : ControllerBase
    {
        private readonly ChatroomService _service;

        public ChatroomController(ChatroomService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChatroomViewModel>>> List() => Ok(await _service.List());

        [HttpGet("{id}")]
        public async Task<ActionResult<ChatroomViewModel>> Find(int id) => Ok(await _service.Find(id));

        [HttpPost]
        public async Task<ActionResult<ChatroomViewModel>> Create([FromBody]string name) => Ok(await _service.Create(name));

        [HttpPut]
        public async Task<ActionResult<ChatroomViewModel>> Update([FromBody]ChatroomViewModel chatroom) => Ok(await _service.Update(chatroom));

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id) => Ok(await _service.Delete(id));
    }
}
