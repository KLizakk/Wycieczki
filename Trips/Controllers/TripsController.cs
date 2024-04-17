using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trips;
using Trips.Models;
using TripsS.Repositories.Interfaces;
using TripsS.Services.Interfaces;
using TripsS.ViewModel;

namespace Trips.Controllers
{
    public class TripsController : Controller
    {
        private readonly ITripService _context;

        public TripsController(ITripService context)
        {
            this._context = context;
        }

        // GET: Trips
        public async Task<IActionResult> Index()
        {
            var Trips = await _context.GetAllAsync();
            var TripsList = Trips.Select(trip => new TripViewModel
            {
                IdTrip = trip.IdTrip,
                From = trip.From,
                To = trip.To,
                StartTrip = trip.StartTrip,
                EndTrip = trip.EndTrip,
                Price = trip.Price
            }).ToList();


            return View( TripsList);
        }

        //// GET: Trips/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var trip = await _context.Trips
        //        .FirstOrDefaultAsync(m => m.IdTrip == id);
        //    if (trip == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(trip);
        //}

        // GET: Trips/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrip,From,To,StartTrip,EndTrip,Price")] TripViewModel tripViewModel)
        {
           if(ModelState.IsValid)
            {
                var trip = new Trip
                {
                   EndTrip = tripViewModel.EndTrip,
                   From = tripViewModel.From,
                   IdTrip = tripViewModel.IdTrip,
                   Price = tripViewModel.Price,
                   StartTrip = tripViewModel.StartTrip,
                   To = tripViewModel.To


                };
                await _context.InsertAsync(trip);
                await _context.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tripViewModel);
        }

        // GET: Trips/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.GetByIdAsync(id);
            if (trip == null)
            {
                return NotFound();
            }

            //Mapowanie Trip na TripViewModel
            var tripViewModel = new TripViewModel
            {
                IdTrip = trip.IdTrip,
                From = trip.From,
                To = trip.To,
                StartTrip = trip.StartTrip,
                EndTrip = trip.EndTrip,
                Price = trip.Price
            };
            return View(tripViewModel);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrip,From,To,StartTrip,EndTrip,Price")] TripViewModel tripViewModel)
        {
            if (id != tripViewModel.IdTrip)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var trip = new Trip()
                {
                    From = tripViewModel.From,
                    To = tripViewModel.To,
                    StartTrip = tripViewModel.StartTrip,
                    EndTrip = tripViewModel.EndTrip,
                    Price = tripViewModel.Price,
                    IdTrip = tripViewModel.IdTrip

                };
                try
                {
                    _context.Update(trip);
                    await _context.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TripExists(trip.IdTrip))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tripViewModel);
        }

        // GET: Trips/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trip = await _context.GetByIdAsync(id);
                
            if (trip == null)
            {
                return NotFound();
            }

            return View(trip);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trip = await _context.GetByIdAsync(id);
            if (trip != null)
            {
                _context.Delete(trip);
            }

            await _context.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TripExists(int id)
        {
            return _context.Exist(new Trip { IdTrip = id});
        }
    }
}
