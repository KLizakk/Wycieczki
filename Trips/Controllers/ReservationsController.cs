using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trips;
using Trips.Models;
using TripsS.Repositories.Interfaces;
using TripsS.Services.Interfaces;
using TripsS.ViewModel;

namespace TripsS.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly IReservationService _context;
        private readonly IValidator<ReservationViewModel> _reservationValidator;

        public ReservationsController(IReservationService context, IValidator<ReservationViewModel> reservationValidator)
        {
            _context = context;
            _reservationValidator = reservationValidator;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var Reservations = await _context.GetAllAsync();
            var reservationViewModel = Reservations.Select(reservation => new ReservationViewModel
            {
                IdReservation = reservation.IdReservation,
                IdClient = reservation.IdClient,
                IdTrip = reservation.IdTrip,
                AmountOfPeople = reservation.AmountOfPeople,
                ReservationDate = reservation.ReservationDate
                
            }).ToList();
            return View(reservationViewModel);
        }

       

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdReservation,IdClient,IdTrip,AmountOfPeople,ReservationDate,Status")] ReservationViewModel reservationViewModel)
        {
            var result = _reservationValidator.Validate(reservationViewModel);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
            if (result.IsValid)
            {
                var reservation = new Reservation
                {
                    IdClient = reservationViewModel.IdClient,
                    IdTrip = reservationViewModel.IdTrip,
                    AmountOfPeople = reservationViewModel.AmountOfPeople,
                    ReservationDate = reservationViewModel.ReservationDate

                };
                reservation.IdReservation = Guid.NewGuid();
                await _context.InsertAsync(reservation);
                await _context.SaveAsync();
                return RedirectToAction(nameof(Index));
            }

            
            return View(reservationViewModel);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.GetByIdAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }
            var reservationViewModel = new ReservationViewModel
            {
                IdReservation = reservation.IdReservation,
                IdClient = reservation.IdClient,
                IdTrip = reservation.IdTrip,
                AmountOfPeople = reservation.AmountOfPeople,
                ReservationDate = reservation.ReservationDate
            };
            return View(reservationViewModel);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdReservation,IdClient,IdTrip,AmountOfPeople,ReservationDate,Status")] ReservationViewModel reservationViewModel)
        {
            if (id != reservationViewModel.IdReservation)
            {
                return NotFound();
            }
            var result = _reservationValidator.Validate(reservationViewModel);
            if (!result.IsValid)
            {
                foreach (var failure in result.Errors)
                {
                    ModelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
                }
            }
            if (result.IsValid)
            {
                var reservation = new Reservation()
                {
                    IdClient = reservationViewModel.IdClient,
                    IdTrip = reservationViewModel.IdTrip,
                    AmountOfPeople = reservationViewModel.AmountOfPeople,
                    ReservationDate = reservationViewModel.ReservationDate,
                    IdReservation = reservationViewModel.IdReservation

                };
                try
                {
                    _context.Update(reservation);
                    await _context.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.IdReservation))
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
            return View(reservationViewModel);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }
            var reservationViewModel = new ReservationViewModel
            {
                IdClient = reservation.IdClient,
                IdTrip = reservation.IdTrip,
                AmountOfPeople = reservation.AmountOfPeople,
                ReservationDate = reservation.ReservationDate

            };
            return View(reservationViewModel);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            
            var reservation = await _context.GetByIdAsync(id);
            if (reservation != null)
            {
                _context.Delete(reservation);
            }

            await _context.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(Guid id)
        {
            return _context.Exist(new Reservation { IdReservation = id });  
        }
    }
}
