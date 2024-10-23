using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LocacaoDesafioBackEnd.Models;
using LocacaoDesafioBackEnd.Data;
using Microsoft.EntityFrameworkCore;

public class AdminMotosController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminMotosController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Método para exibir o formulário de cadastro
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Método para processar o cadastro da moto
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Moto moto)
    {
        if (ModelState.IsValid)
        {
            // Verifica se a placa já existe
            if (await _context.Motos.AnyAsync(m => m.Placa == moto.Placa))
            {
                ModelState.AddModelError("Placa", "A placa já está em uso.");
                return View(moto); // Retorna à view em caso de erro
            }

            _context.Add(moto);
            await _context.SaveChangesAsync();
            // Publicar o evento de moto cadastrada
            //await _motoEventService.PublishMotoCadastradaEvent(moto);
            return RedirectToAction(nameof(Index)); // Redireciona após o cadastro
        }
        return View(moto); // Retorna à view em caso de erro
    }

    // Outros métodos (Index, Edit, Delete) podem ser adicionados aqui
}
