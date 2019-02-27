using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.ViewModels;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        private readonly UserManager<MyUser> _userManager;

        public TemplateController(ITemplateService templateService, UserManager<MyUser> userManager)
        {
            _templateService = templateService;
            _userManager = userManager;
        }

        [Authorize(Roles ="Manager")]
        [HttpPost]
        public async Task <ActionResult> Post(TemplateCM model)
        {
            try
            {
                var user =  await _userManager.GetUserAsync(User);
                var template = model.Adapt<Template>();
                template.CompanyId = user.Staff.CompanyId;
                _templateService.CreateTemplate(template);

                return StatusCode(201, template.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("file")]
        public ActionResult Post_( IFormFile Invoice, IFormFile ReleaseAnnouncement, string templateId)
        {
            try
            {

                return StatusCode(201);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}