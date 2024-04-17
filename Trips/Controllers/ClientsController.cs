using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Trips;
using Trips.Models;
using TripsS.Repositories;
using TripsS.Repositories.Interfaces;   
using TripsS.Services.Interfaces;
using TripsS.ViewModel;

namespace Trips.Controllers
{
    public class ClientsController : Controller
    {


        private readonly IClientService _clientServices;  
        public ClientsController(IClientService clientRepository)
        {
            this._clientServices = clientRepository;
        }

        public async Task<IActionResult> Index()
        {
            var clients = await _clientServices.GetAllAsync();
            var clientsList = clients.Select(client => new ClientViewModel
            {
                IdClient = client.IdClient,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone
            }).ToList();
            return View(clientsList);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientServices.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            var clientViewModel = new ClientViewModel
            {
                IdClient = client.IdClient,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone
            };

            return View(clientViewModel);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create([Bind("IdClient,FirstName,LastName,Email,Phone")] ClientViewModel clientViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = new Client
                {
                   FirstName = clientViewModel.FirstName,
                   LastName = clientViewModel.LastName,
                   Email = clientViewModel.Email,
                   Phone = clientViewModel.Phone,
                   IdClient = clientViewModel.IdClient
                   

                };
                await _clientServices.InsertAsync(client);
                await _clientServices.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(clientViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientServices.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            var clientViewModel = new ClientViewModel
            {
                IdClient = client.IdClient,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone
            };
            return View(clientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
            public async Task <IActionResult> Edit(int id, [Bind("IdClient,FirstName,LastName,Email,PhoneNumber")] ClientViewModel clientViewModel)
            {
                if (id != clientViewModel.IdClient)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    var client = new Client
                    {
                        IdClient = clientViewModel.IdClient,
                        FirstName = clientViewModel.FirstName,
                        LastName = clientViewModel.LastName,
                        Email = clientViewModel.Email,
                        Phone = clientViewModel.Phone
                    };
                    try
                    {
                        _clientServices.Update(client);
                        await _clientServices.SaveAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ClientExists(client.IdClient))
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
                return View(clientViewModel);
            }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _clientServices.GetByIdAsync(id);
            var clientViewModel = new ClientViewModel
            {
                IdClient = client.IdClient,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Phone = client.Phone
            };

            if (client == null)
            {
                return NotFound();
            }

            return View(clientViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = await _clientServices.GetByIdAsync(id);
            if (client != null)
            {
                _clientServices.Delete(client);
            }

            await _clientServices.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        
        private bool ClientExists(int id)
        {
            return _clientServices.Exist(new Client { IdClient = id });
        }

    }
}
