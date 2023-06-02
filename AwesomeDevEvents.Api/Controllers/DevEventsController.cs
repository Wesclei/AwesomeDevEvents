﻿using AwesomeDevEvents.Api.Entities;
using AwesomeDevEvents.Api.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.Api.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventDbContext _context;
        public DevEventsController(DevEventDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obter todos os eventos
        /// </summary>
        /// <returns>Coleção de eventos</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvents.Where(w => !w.IsDeleted).ToList();

            return Ok(devEvents);
        }

        /// <summary>
        /// Obter um evento
        /// </summary>
        /// <param name="id">Identificador do evento</param>
        /// <returns>Dados do evento</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Não encontrado</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvents
                .Include(de => de.Speakers)
                .SingleOrDefault(w => w.Id == id);

            if(devEvent == null)
            {
                return NotFound();
            }
            return Ok(devEvent);
        }

        /// <summary>
        /// Cadastrar um evento
        /// </summary>
        /// <remarks>
        /// {"title": "string","description": "string","startDate": "2023-06-02T19:40:40.023Z","endDate": "2023-06-02T19:40:40.023Z"}
        /// </remarks>
        /// <param name="devEvent">Dados do evento</param>
        /// <returns>Objeto criado</returns>
        /// <response code="201">Sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvents.Add(devEvent);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);

        }

        /// <summary>
        /// Atualizar um evento
        /// </summary>
        /// <remarks>
        /// {"title": "string","description": "string","startDate": "2023-06-02T19:40:40.023Z","endDate": "2023-06-02T19:40:40.023Z"}
        /// </remarks>
        /// <param name="id">Identificador do evento</param>
        /// <param name="input">Dados do evento</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(w => w.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvents.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Deletar um evento
        /// </summary>
        /// <param name="id">Identificado de evento</param>
        /// <returns>nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvents.SingleOrDefault(w => w.Id == id);

            if (devEvent == null)
            {
                return NotFound();
            }

            devEvent.Delete();

            _context.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// Cadastrar palestrante
        /// </summary>
        /// <remarks>
        /// {"name": "string","talkTitle": "string","talkDescription": "string","linkedInProfile": "string"}
        /// </remarks>
        /// <param name="id">Identificado do evento</param>
        /// <param name="speaker">Dados do palestrante</param>
        /// <returns>Nada</returns>
        /// <response code="404">Não encontrado</response>
        /// <response code="204">Sucesso</response>
        [HttpPost("{id}/speakers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker speaker)
        {
            speaker.DevEventiId = id;
            var devEvent = _context.DevEvents.Any(w => w.Id == id);

            if (!devEvent)
            {
                return NotFound();
            }

            _context.DevEventSpeakers.Add(speaker);

            _context.SaveChanges();

            return NoContent();
        }
    }
}