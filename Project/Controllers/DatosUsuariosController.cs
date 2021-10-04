using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project.Models;

namespace Project.Controllers
{
    
    [Route("api/usuarios")]
    [ApiController]
    public class DatosUsuariosController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly DataBaseContext _context;

        public DatosUsuariosController(DataBaseContext context, ILogger<DatosUsuariosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/DatosUsuarios
        /// <summary>
        /// Obtiene todos los usuarios en base de datos.
        /// </summary>
        /// 
        /// <returns>Lista de todos los usuarios</returns>
        /// <response code="200">Retorna los datos de base de datos</response>
        /// <response code="500">Error interno</response>  
        [Produces("application/json")]
        [HttpGet]
        [Route("ObtenerUsuarios")]
        public async Task<ActionResult<IEnumerable<DatosUsuarios>>> GetUsuariosItems()
        {
            _logger.LogInformation("Consumida api de Obtener Usuarios");
            return await _context.UsuariosItems.ToListAsync();
        }

        // PUT: api/DatosUsuarios/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("EditarUsuario/{id}")]
        public async Task<IActionResult> PutDatosUsuarios(int id, DatosUsuarios datosUsuarios)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Consumida api de Editar Usuario donde ha ocurrido el error de que el modelo no coincide");
                return BadRequest();
            }
            if (id != datosUsuarios.Id)
            {
                _logger.LogInformation("Consumida api de Editar Usuario donde ha ocurrido el error de que el id no coincide");
                return BadRequest();
            }

            _context.Entry(datosUsuarios).State = EntityState.Modified;

            try
            {
                _logger.LogInformation("Consumida api de Editar Usuario Guardando Usuario");
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DatosUsuariosExists(id))
                {
                    _logger.LogInformation("Consumida api de Editar Usuario El usuario no existe");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation("Consumida api de Editar Usuario Error guardando");
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DatosUsuarios
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [Produces("application/json")]
        [HttpPost]
        [Route("AgregarUsuario")]
        public async Task<ActionResult<DatosUsuarios>> PostDatosUsuarios(DatosUsuarios datosUsuarios)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Consumida api de Agregar Usuario donde ha ocurrido el error de que el modelo no coincide");
                return BadRequest();
            }
            try
            {
                _context.UsuariosItems.Add(datosUsuarios);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Consumida api de Agregar Usuario Guardado exitoso");
                return CreatedAtAction("GetDatosUsuarios", new { id = datosUsuarios.Id }, datosUsuarios);
            }
            catch (Exception err)
            {
                _logger.LogInformation("Consumida api de Agregar Usuario Ha ocurrido un error"+ err.Message);
                return BadRequest();
            }
            
        }

        // DELETE: api/DatosUsuarios/5
        [Produces("application/json")]
        [HttpGet]
        [Route("EliminarUsuario/{id}")]
        public async Task<ActionResult<DatosUsuarios>> DeleteDatosUsuarios(int id)
        {

            _logger.LogInformation("Consumida api de Eliminar Usuario Eliminacion ");
            var datosUsuarios = await _context.UsuariosItems.FindAsync(id);
            if (datosUsuarios == null)
            {
                _logger.LogInformation("Consumida api de Eliminar Usuario No se encontro usuario: "+id);
                return NotFound();
            }

            _context.UsuariosItems.Remove(datosUsuarios);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Consumida api de Eliminar Usuario Eliminacion exitosa del id: " + id);
            return datosUsuarios;
        }

        private bool DatosUsuariosExists(long id)
        {
            return _context.UsuariosItems.Any(e => e.Id == id);
        }
    }
}
