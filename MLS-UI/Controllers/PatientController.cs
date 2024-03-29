﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MLS_UI.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MLS_UI.Controllers
{
    public class PatientController : Controller
    {
        private readonly ILogger<PatientController> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;

        public PatientController(ILogger<PatientController> logger, IHttpClientFactory httpClient, IHttpContextAccessor contextAccessor)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:6001");
            _contextAccessor = contextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> Index()    //string token
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var request = await _httpClient.GetAsync("/gateway/patientsmanager");

            if (request.IsSuccessStatusCode)
            {
                var patients = await request.Content.ReadFromJsonAsync<List<PatientDto>>();

                _logger.LogInformation($"List of patients retrieved successfully at {DateTime.UtcNow.ToLongTimeString()}");

                return View(patients);
            }
            else
            {
                _logger.LogError("Error retrieving patients");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(PatientDto patient)
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var content = new StringContent(JsonSerializer.Serialize(patient), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync("/gateway/patientsmanager", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Patient added successfully");

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong");

                return View("Error");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var request = await _httpClient.GetAsync($"/gateway/patientsmanager/{id}");

            if (request.IsSuccessStatusCode)
            {
                var patient = await request.Content.ReadFromJsonAsync<PatientDto>();

                _logger.LogInformation($"Patient with ID {id} retrieved successfully at {DateTime.UtcNow.ToLongTimeString()}");

                return View(patient);
            }
            else
            {
                _logger.LogError($"Error retrieving patient with ID {id}");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(int id, PatientDto updatedPatient)
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var content = new StringContent(JsonSerializer.Serialize(updatedPatient), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"/gateway/patientsmanager/{id}", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Patient with ID {id} updated successfully");

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong while updating patient with ID {id}");

                return View("Error");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var response = await _httpClient.GetAsync($"/gateway/patientsmanager/{id}");

            if (response.IsSuccessStatusCode)
            {
                var patient = await response.Content.ReadFromJsonAsync<PatientDto>();
                return View(patient);
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Unable to retrieve patient details");
                return View("Error");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ReallyDelete(int id)
        {
            // recupere le jwt
            var token = _contextAccessor.HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is missing");
            }

            // Ajout token dans le header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



            var response = await _httpClient.DeleteAsync($"/gateway/patientsmanager/{id}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Patient with ID {id} deleted successfully");
                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong while deleting patient with ID {id}");
                return View("Error");
            }
        }
    }
}
