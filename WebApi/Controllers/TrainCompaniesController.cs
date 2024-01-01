using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Exceptions;

namespace WebApi.Controllers
{
    public class TrainCompaniesController : BaseApiController
    {

        private readonly ITrainCompanyService _trainCompanySerivce;
        public TrainCompaniesController(ITrainCompanyService trainCompanySerivce)
        {
            _trainCompanySerivce = trainCompanySerivce;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainCompanyDto>>> GetTrainCompanies()
        {
            var trainCompaniesDto = await _trainCompanySerivce.GetAllCompanyDtoAsync();
            return Ok(trainCompaniesDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrainCompanyDto>> GetTrainCompany(int id)
        {
            var trainCompanies = await _trainCompanySerivce.GetCompanyDtoByIdAsync(id);

            if (trainCompanies == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            return Ok(trainCompanies);
        }

        [HttpPost]
        public async Task<IActionResult> PostTrainCompany([FromBody] TrainCompany trainCompany)
        {
            await _trainCompanySerivce.AddCompanyAsync(trainCompany);
            return CreatedAtAction("GetTrainCompany", new { id = trainCompany.Id }, trainCompany);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainCompany(int id, [FromBody] TrainCompany trainCompany)
        {

            if (id != trainCompany.Id)
            {
                return BadRequest(new ErrorResponse(400));
            }

            try
            {
                await _trainCompanySerivce.UpdateCompanyAsync(trainCompany);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!trainCompaniesExists(id))
                {
                    return NotFound(new ErrorResponse(404));
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainCompany(int id)
        {
            var trainCompany = await _trainCompanySerivce.GetCompanyByIdAsync(id);
            if (trainCompany == null)
            {
                return NotFound(new ErrorResponse(404));
            }

            await _trainCompanySerivce.SoftDeleteCompanyAsync(trainCompany);

            return NoContent();
        }

        private bool trainCompaniesExists(int id)
        {
            return _trainCompanySerivce.GetCompanyByIdAsync(id) != null;
        }
    }
}