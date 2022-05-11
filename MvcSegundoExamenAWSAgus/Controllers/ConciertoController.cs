using Microsoft.AspNetCore.Mvc;
using MvcSegundoExamenAWSAgus.Models;
using MvcSegundoExamenAWSAgus.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcSegundoExamenAWSAgus.Controllers
{
    public class ConciertoController : Controller
    {
        private ServiceConciertos service;

        public ConciertoController(ServiceConciertos service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Eventos(int? id)
        {
            if (id==null)
            {
                List<Evento> eventos = await this.service.GetEventosAsync();
                return View(eventos);
            }
            else
            {
                List<Evento> eventos = await this.service.GetEventosPorCategAsync(id.Value);
                return View(eventos);
            }            
        }

        public async Task<IActionResult> CambiarCategoria(int id)
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriasAsync();
            Evento evento = await this.service.FindEventoAsync(id);
            string nombrecateg = await this.service.GetNombreCategoria(evento.IdCategoria);
            ViewData["CATEGORIAS"] = categorias;
            ViewData["NOMBRECATEGORIA"] = nombrecateg;
            return View(evento);
        }

        [HttpPost]
        public IActionResult CambiarCategoria(int categoria, int evento)
        {
            this.service.CambiarCategoriaEvento(categoria, evento);
            return RedirectToAction("Eventos");
        }

        public IActionResult EliminarEvento(int id)
        {
            this.service.DeleteEvento(id);
            return RedirectToAction("Eventos");
        }

        public async Task<IActionResult> NuevoEvento()
        {
            List<CategoriaEvento> categorias =await this.service.GetCategoriasAsync();
            ViewData["CATEGORIAS"] = categorias;
            return View();
        }

        [HttpPost]
        public IActionResult NuevoEvento(int idevento, string nombre, string artista, int categoria)
        {
            Evento evento = new Evento()
            {
                IdEvento=idevento,
                Nombre=nombre,
                Artista=artista,
                IdCategoria=categoria
            };
            this.service.NuevoEvento(evento);
            return RedirectToAction("Eventos");
        }

        public async Task<IActionResult> Categorias()
        {
            List<CategoriaEvento> categorias = await this.service.GetCategoriasAsync();
            return View(categorias);
        }

    }
}
