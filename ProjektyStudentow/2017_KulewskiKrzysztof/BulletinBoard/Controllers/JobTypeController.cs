﻿using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BulletinBoard.Data;
using BulletinBoard.Helpers;
using BulletinBoard.Models;
using BulletinBoard.Models.JobTypeViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BulletinBoard.Controllers
{
    [Authorize(Roles = RoleHelper.Administrator)]
    public class JobTypeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public JobTypeController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            var jobTypes = await _context.JobTypes
                .Select(m => _mapper.Map<JobTypeViewModel>(m)).ToListAsync();
            return View(jobTypes);
        }

        // GET: JobType/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DetailsJobTypeViewModel>(jobType);
            return View(viewModel);
        }

        // GET: JobType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobType = new JobType {Name = model.Name};
            _context.Add(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<EditJobTypeViewModel>(jobType);
            return View(viewModel);
        }

        // POST: JobType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var jobType = await _context.JobTypes
                    .SingleOrDefaultAsync(m => m.JobTypeId == model.JobTypeId);
                jobType.Name = model.Name;
                _context.Update(jobType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobTypeExists(model.JobTypeId))
                {
                    return View("NotFound");
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobType/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return View("NotFound");
            }

            var viewModel = _mapper.Map<DeleteJobTypeViewModel>(jobType);
            return View(viewModel);
        }

        // POST: JobType/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(DeleteJobTypeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobType = await _context.JobTypes
                .SingleOrDefaultAsync(m => m.JobTypeId == model.JobTypeId);
            _context.JobTypes.Remove(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobTypeExists(string id)
        {
            return await _context.JobTypes.AnyAsync(e => e.JobTypeId == id);
        }
    }
}
