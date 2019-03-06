using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Paging;
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

        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<ActionResult> Get(int index = 1, int pageSize = 5)
        {
            var user = await _userManager.GetUserAsync(User);
            var templates = _templateService.GetTemplates(
                _ => _.CompanyId.Equals(user.Staff.CompanyId));
            var result = templates.ToPageList<TemplateVM, Template>(index, pageSize);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetAll")]
        public async Task<ActionResult> Get()
        {
            var user = await _userManager.GetUserAsync(User);
            var templates = _templateService.GetTemplates(
                _ => _.CompanyId.Equals(user.Staff.CompanyId) && 
                     _.IsActive == true).OrderBy(_=>_.ReleaseDate).ToList();
            List<TemplateVM> result = new List<TemplateVM>();
            foreach (var item in templates)
            {
                result.Add(item.Adapt<TemplateVM>());
            }
            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> Post(TemplateCM model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
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

        [HttpPost("{id}/files")]
        public ActionResult Post_(IFormFile Invoice, IFormFile ReleaseAnnouncement, Guid id)
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
                if (template != null)
                {
                    if (template.FileUrl != null && template.FileUrl.Length > 0)
                    {
                        _fileService.DeleteFile(template.FileUrl);
                    }
                    if (template.ReleaseAnnouncementUrl != null && template.ReleaseAnnouncementUrl.Length > 0)
                    {
                        _fileService.DeleteFile(template.ReleaseAnnouncementUrl);
                    }
                }
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult GetTempalte(Guid id)
        {
            try
            {
                var template = _templateService.GetTemplate(id);
                if (template == null) return NotFound();

                return Ok(template.Adapt<TemplateVM>());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/invoice")]
        public ActionResult GetTemplateFiles(Guid id)
        {
            try
            {
                var template = _templateService.GetTemplate(id);
                if (template == null) return NotFound();

                var file = _fileService.GetFile(template.FileUrl).Result;
                //return File(file.Stream,file.ContentType, file.FileName);
                return File(file.Stream, file.ContentType);
                //return new FileStreamResult(file.Stream, file.ContentType);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public ActionResult UpdateTemplate(TemplateUM model)
        {
            try
            {
                var template = _templateService.GetTemplate(model.Id);
                if (template == null) return NotFound();

                template = model.Adapt(template);
                _templateService.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/files")]
        public ActionResult UpdateTemplateFiles(IFormFile Invoice, IFormFile ReleaseAnnouncement, Guid id)
        {
            Template template = null;
            string newFileUrl = null;
            string newReleaseAnnouncementUrl = null;

            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                template = _templateService.GetTemplate(id);
                if (template == null) return NotFound();

                if (Invoice != null)
                {
                    newFileUrl = _fileService.SaveFile(user.Staff.CompanyId.ToString(), nameof(FileType.Template), Invoice).Result;
                }
                if (ReleaseAnnouncement != null)
                {
                    newReleaseAnnouncementUrl = _fileService.SaveFile(user.Staff.CompanyId.ToString(), nameof(FileType.Template), ReleaseAnnouncement).Result;
                }

                template.FileUrl = newFileUrl;
                template.ReleaseAnnouncementUrl = newReleaseAnnouncementUrl;
                _templateService.SaveChanges();
                return Ok();
            }
            catch (Exception e)
            {
                if (template != null)
                {
                    if (newFileUrl != null)
                    {
                        _fileService.DeleteFile(newFileUrl);
                    }
                    if (newReleaseAnnouncementUrl != null)
                    {
                        _fileService.DeleteFile(newReleaseAnnouncementUrl);
                    }
                }
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public ActionResult DeleteTemplate(Guid id)
        {
            try
            {
                var template = _templateService.GetTemplate(id);
                if (template == null) return NotFound();

                template.IsActive = false;
                _templateService.SaveChanges();

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public ActionResult GetTemplates(int index = 1, int pageSize = 5)
        {
            try
            {
                var user = _userManager.GetUserAsync(User).Result;
                var templates = _templateService
                    .GetTemplates(_ => _.IsActive == true && _.CompanyId.Equals(user.Staff.CompanyId));
                if (templates == null) return NotFound();

                var result = templates.ToPageList<TemplateVM, Template>(index, pageSize);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}