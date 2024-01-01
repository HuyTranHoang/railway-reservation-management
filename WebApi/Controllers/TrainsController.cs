using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class TrainsController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly ITrainRepository _repository;
        public TrainsController(IUnitOfWork unitOfWork, ITrainRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Train>>> GetAll()
        {
            return await _repository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Train>> GetById(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult> Create (Train train)
        {
            _repository.Add(train);
            await _unitOfWork.SaveChangesAsync();
            return Ok();
        }
    }
}
