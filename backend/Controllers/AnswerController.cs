    using backend.Data;
    using backend.DTOs;
    using backend.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace backend.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class AnswerController : ControllerBase
        {
            private readonly ApplicationDbContext _context;

            public AnswerController(ApplicationDbContext context)
            {
                _context = context;
            }

            [HttpGet]
            [Authorize(Roles = "Examiner, Examinee")] 
            public async Task<ActionResult<IEnumerable<AnswerDto>>> GetAnswers()
            {
                return await _context.Answers
                    .Select(answer => new AnswerDto
                    {
                        Id = answer.Id,
                        QuestionId = answer.QuestionId,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect
                    })
                    .ToListAsync();
            }

            [HttpGet("{id}")]
            [Authorize(Roles = "Examiner, Examinee")] 
            public async Task<ActionResult<AnswerDto>> GetAnswer(int id)
            {
                var answer = await _context.Answers.FindAsync(id);

                if (answer == null)
                {
                    return NotFound();
                }

                return new AnswerDto
                {
                    Id = answer.Id,
                    QuestionId = answer.QuestionId,
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect
                };
            }

            [HttpPost]
            [Authorize(Roles = "Examiner")] 
            public async Task<ActionResult<AnswerDto>> CreateAnswer(CreateAnswerDto createAnswerDto)
            {
                var answer = new Answer
                {
                    QuestionId = createAnswerDto.QuestionId,
                    Text = createAnswerDto.Text,
                    IsCorrect = createAnswerDto.IsCorrect
                };

                _context.Answers.Add(answer);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetAnswer), new { id = answer.Id }, new AnswerDto
                {
                    Id = answer.Id,
                    QuestionId = answer.QuestionId,
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect
                });
            }

            [HttpPut("{id}")]
            [Authorize(Roles = "Examiner")] 
            public async Task<IActionResult> UpdateAnswer(int id, UpdateAnswerDto updateAnswerDto)
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                {
                    return NotFound();
                }

                answer.Text = updateAnswerDto.Text;
                answer.IsCorrect = updateAnswerDto.IsCorrect;

                _context.Entry(answer).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }

            [HttpDelete("{id}")]
            [Authorize(Roles = "Examiner")] 
            public async Task<IActionResult> DeleteAnswer(int id)
            {
                var answer = await _context.Answers.FindAsync(id);
                if (answer == null)
                {
                    return NotFound();
                }

                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }
