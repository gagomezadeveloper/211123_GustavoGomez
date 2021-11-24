using _211123_GustavoGomez.Dto;
using _211123_GustavoGomez.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace _211123_GustavoGomez.Controllers
{
    public class HomeController : Controller
    {
        public DataViewModel Data = new DataViewModel();

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var request = new ApiRequestDto();

            try
            {
                using (var client = new HttpClient())
                {
                    var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("https://fleetsapapiqa.azurewebsites.net/api/GetIMEIDataServicesByIMEIAndCompany", content);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Data.ApiResponseDtos.Add(System.Text.Json.JsonSerializer.Deserialize<ApiResponseDto>(resultContent));
                }
                ViewData["Date"] = $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}";
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return Error();
            }

            return View(Data);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
