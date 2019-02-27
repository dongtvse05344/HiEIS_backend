using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Utils;
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
        private readonly IFileService _fileService;

        public TemplateController(ITemplateService templateService, UserManager<MyUser> userManager, IFileService fileService)
        {
            _templateService = templateService;
            _userManager = userManager;
            _fileService = fileService;
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
                _templateService.SaveChanges();
                return StatusCode(201, template.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("file/{id}")]
        public ActionResult Post_( IFormFile Invoice, IFormFile ReleaseAnnouncement, Guid id)
        {
            Template template = null;
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                template = _templateService.GetTemplate(id);
                if (template == null) return NotFound();
                template.FileUrl = _fileService.SaveFile(user.Staff.CompanyId.ToString(), nameof(FileType.Template), Invoice).Result;
                template.ReleaseAnnouncementUrl = _fileService.SaveFile(user.Staff.CompanyId.ToString(), nameof(FileType.Template), ReleaseAnnouncement).Result;
                _templateService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                if(template != null)
                {
                    if(template.FileUrl.Length>0)
                    {
                        _fileService.DeleteFile(template.FileUrl);
                    }
                    if (template.ReleaseAnnouncementUrl.Length > 0)
                    {
                        _fileService.DeleteFile(template.ReleaseAnnouncementUrl);
                    }
                }
                return BadRequest(e.Message);
            }
        }


    }
}