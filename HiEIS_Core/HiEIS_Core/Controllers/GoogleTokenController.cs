using HiEIS.Model;
using HiEIS.Service;
using HiEIS_Core.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HiEIS_Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleTokenController : ControllerBase
    {
        private readonly IGoogleTokenService _googleTokenService;
        private readonly UserManager<MyUser> _userManager;

        public GoogleTokenController(IGoogleTokenService googleTokenService, UserManager<MyUser> userManager)
        {
            _googleTokenService = googleTokenService;
            _userManager = userManager;
        }

        [HttpGet("Code")]
        public ActionResult GetCode(string code)
        {
            try
            {
                return Ok(code);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Token")]
        public async Task<ActionResult> GetToken([FromBody]string code)
        {
            try
            {
                string url = "https://www.googleapis.com/oauth2/v4/token";

                // Request body được encode từ url
                var list = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("code", code),
                    new KeyValuePair<string, string>("client_id", "396730019122-1bqknv4qb2295opq30g5s0ffn46ojqdt.apps.googleusercontent.com"),
                    new KeyValuePair<string, string>("client_secret", "HhkMh7_F_T_HtJQW8G0ujl1o"),
                    new KeyValuePair<string, string>("redirect_uri", "https://localhost:44326/api/GoogleToken/Code"),
                    new KeyValuePair<string, string>("grant_type", "authorization_code"),
                };

                using (var httpClient = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(list);
                    var response = await httpClient.PostAsync(url, content);
                    try
                    {
                        response.EnsureSuccessStatusCode();
                    }
                    catch (Exception)
                    {
                        return BadRequest(await response.Content.ReadAsStringAsync());
                    }
                    var resContent = await response.Content.ReadAsStringAsync();
                    var googleTokenModel = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenModel>(resContent);

                    var user = await _userManager.GetUserAsync(User);
                    if (user.GoogleToken == null)
                    {
                        var googleToken = googleTokenModel.Adapt<GoogleToken>();
                        googleToken.Id = user.Id;

                        _googleTokenService.CreateGoogleToken(googleToken);
                    }
                    else
                    {
                        var googleToken = _googleTokenService.GetGoogleToken(user.Id);
                        googleToken = googleTokenModel.Adapt(googleToken);

                        _googleTokenService.UpdateGoogleToken(googleToken);
                    }
                    
                    _googleTokenService.SaveChanges();

                    return Ok();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /*
        [HttpPost("RefreshGoogleToken")]
        public ActionResult Refresh(string refresh_token)
        {
            try
            {
                string url = "https://www.googleapis.com/oauth2/v4/token";
                var list = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("refresh_token", refresh_token),
                    new KeyValuePair<string, string>("client_id", "396730019122-1bqknv4qb2295opq30g5s0ffn46ojqdt.apps.googleusercontent.com"),
                    new KeyValuePair<string, string>("client_secret", "HhkMh7_F_T_HtJQW8G0ujl1o"),
                    new KeyValuePair<string, string>("grant_type", "refresh_token"),
                };

                using (var httpClient = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(list);
                    var response = httpClient.PostAsync(url, content).Result;
                    var resContent = response.Content.ReadAsStringAsync().Result;
                    var googleToken = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleToken>(resContent);
                    return Ok(googleToken.Access_token);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        */
    }
}
