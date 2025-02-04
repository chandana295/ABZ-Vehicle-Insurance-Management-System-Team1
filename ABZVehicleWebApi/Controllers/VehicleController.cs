﻿using ABZVehicleLibrary.Models;
using ABZVehicleLibrary.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ABZVehicleWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VehicleController : ControllerBase
    {
        IVehicleRepoAsync vehRepo;
        public VehicleController(IVehicleRepoAsync repo)
        {
            vehRepo = repo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Vehicle> vehicles = await vehRepo.GetAllVehiclesAsync();
            return Ok(vehicles);
        }
        [HttpGet("{regNo}")]
        public async Task<ActionResult> GetOne(string regNo)
        {
            try
            {
                Vehicle vehicle = await vehRepo.GetVehicleAsync(regNo);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {

                return NotFound();
            }
        }

        [HttpGet("ByCustomer/{customerId}")]
        public async Task<ActionResult> GetByCustomer(string customerId)
        {
            try
            {
                List<Vehicle> vehicles = await vehRepo.GetVehiclesByCustomerAsync(customerId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }
        }
        [HttpDelete("{regNo}")]
        public async Task<ActionResult> Delete(string regNo)
        {
            try
            {
                await vehRepo.DeleteVehicleAsync(regNo);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{token}")]
        public async Task<ActionResult> Insert(string token,Vehicle vehicle)
        {
            try
            {
                await vehRepo.InsertVehicleAsync(vehicle);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                await client.PostAsJsonAsync("http://localhost:5273/api/Proposal/Vehicle/", new {RegNo=vehicle.RegNo});
               //await client.PostAsJsonAsync("http://abzproposalwebapi-chanad.azurewebsites.net/api/proposal/Vehicle/", new { RegNo = vehicle.RegNo });

                return Created($"api/Vehicle/{vehicle.RegNo}",vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{regNo}")]
        public async Task<ActionResult> Update(string regNo, Vehicle vehicle)
        {
            try
            {
                await vehRepo.UpdateVehicleAsync(regNo, vehicle);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Customer")]
        public async Task<ActionResult> InsertCustomer(Customer customer)
        {
            try
            {
                await vehRepo.InsertCustomerAsync(customer);
                return Created();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
