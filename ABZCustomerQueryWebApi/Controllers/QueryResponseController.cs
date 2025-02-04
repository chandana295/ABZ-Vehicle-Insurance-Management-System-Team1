﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ABZCustomerQueryLibrary.Repos;
using ABZCustomerQueryLibrary.Models;
using Microsoft.AspNetCore.Authorization;

namespace ABZCustomerQueryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QueryResponseController : ControllerBase
    {
        IQueryResponseRepoAsync qrRepo;
        public QueryResponseController(IQueryResponseRepoAsync repo)
        {
            qrRepo = repo;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<QueryResponse> qr = await qrRepo.GetAllQuerysAsync();
            return Ok(qr);
        }
        [HttpGet("{queryId}/{srNo}")]
        public async Task<ActionResult> GetOne(string queryId, string srNo)
        {
            try
            {
                QueryResponse qr = await qrRepo.GetQueryResponseAsync(queryId, srNo);
                return Ok(qr);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("{token}")]
        public async Task<ActionResult> Insert(string token, QueryResponse queryresponse)
        {
            try
            {
                await qrRepo.InsertQueryResponseAsync(queryresponse);
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                return Created($"api/QueryResponse/{queryresponse.QueryID}", queryresponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{queryId}/{srNo}")]
        public async Task<ActionResult> Update(string queryId, string srNo, QueryResponse queryresponse)
        {
            try
            {
                await qrRepo.UpdateQueryResponseAsync(queryId, srNo,queryresponse);
                return Ok(queryresponse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{queryId}/{srNo}")]
        public async Task<ActionResult> Delete(string queryId, string srNo)
        {
            try
            {
                await qrRepo.DeleteQueryResponseAsync(queryId, srNo);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ByCustomerQuery/{queryId}")]
        
        public async Task<ActionResult> GetByCustomerQuery(string queryId)
        {
            try
            {
                List<QueryResponse> queryResponses = await qrRepo.GetQueryResponseByCustomerQueryAsync(queryId);
                return Ok(queryResponses);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost("Agent")]

        public async Task<ActionResult> InsertAgent(Agent agent)
        {
            await qrRepo.InsertAgentAsync(agent);
            return Ok(agent);
        }
        [HttpGet("ByAgent/{agentID}")]
        public async Task<ActionResult> GetByAgent(string agentID)
        {
            try
            {
                List<QueryResponse> queryResponses = await qrRepo.GetQueryResponseByAgentAsync(agentID);
                return Ok(queryResponses);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

    }
}
