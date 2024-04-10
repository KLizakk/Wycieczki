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
            return View(await _clientServices.GetAllAsync());
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

            return View(client);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create([Bind("IdClient,FirstName,LastName,Email,PhoneNumber")] Client client)
        {
            if (ModelState.IsValid)
            {
                await _clientServices.InsertAsync(client);
                await _clientServices.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
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
            return View(client);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, [Bind("IdClient,FirstName,LastName,Email,PhoneNumber")] Client client)
        {
            if (id != client.IdClient)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
            return View(client);
        }

        public async Task<IActionResult> Delete(int? id)
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

            return View(client);
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
