using backend.Data;
using backend.DTOs;
using backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourProjectName.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public QuestionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Examiner, Examinee")] 
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            return await _context.Questions
                .Include(q => q.Answers)
                .Select(question => new QuestionDto
                {
                    Id = question.Id,
                    Text = question.Text,
                    CreatedBy = question.CreatedBy,
                    Answers = question.Answers.Select(answer => new AnswerDto
                    {
                        Id = answer.Id,
                        QuestionId = answer.QuestionId,
                        Text = answer.Text,
                        IsCorrect = answer.IsCorrect
                    }).ToList()
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Examiner, Examinee")] 
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id)
        {
            var question = await _context.Questions
                .Include(q => q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }

            return new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                CreatedBy = question.CreatedBy,
                Answers = question.Answers.Select(answer => new AnswerDto
                {
                    Id = answer.Id,
                    QuestionId = answer.QuestionId,
                    Text = answer.Text,
                    IsCorrect = answer.IsCorrect
                }).ToList()
            };
        }

        [HttpPost]
        [Authorize(Roles = "Examiner")] 
        public async Task<ActionResult<QuestionDto>> CreateQuestion(CreateQuestionDto createQuestionDto)
        {
            var question = new Question
            {
                Text = createQuestionDto.Text,
                CreatedBy = createQuestionDto.CreatedBy,
                Answers = createQuestionDto.Answers.Select(a => new Answer
                {
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, new QuestionDto
            {
                Id = question.Id,
                Text = question.Text,
                CreatedBy = question.CreatedBy,
                Answers = question.Answers.Select(a => new AnswerDto
                {
                    Id = a.Id,
                    QuestionId = a.QuestionId,
                    Text = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Examiner")]
        public async Task<IActionResult> UpdateQuestion(int id, UpdateQuestionDto updateQuestionDto)
        {
            var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            question.Text = updateQuestionDto.Text;

            // Remove existing answers
            _context.Answers.RemoveRange(question.Answers);

            // Add updated answers
            question.Answers = updateQuestionDto.Answers.Select(a => new Answer
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect,
                QuestionId = question.Id
            }).ToList();

            _context.Entry(question).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Examiner")] // Only allow users with the Examiner role to access this endpoint
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var question = await _context.Questions.Include(q => q.Answers).FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            _context.Answers.RemoveRange(question.Answers);
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
