using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartResumeAnalyzer.Services;
using SmartResumeAnalyzer.Validators;
using System.ComponentModel.DataAnnotations;

namespace SmartResumeAnalyzer.Pages.Upload
{
    public class UploadResumeModel(ResumeParserService parser) : PageModel
    {
        [BindProperty]
        [AllowedFileExtensions([".pdf", ".docx", ".txt"])]
        [MaxFileSize(5)]
        public required IFormFile ResumeFile { get; set; }

        public IActionResult OnGet()
        {

            return Page();
        }

        public IActionResult OnPost() 
        {
            if (!ModelState.IsValid)
            {
                //ErrorMessage = ModelState.Values.SelectMany(v => v.Errors)
                //                                .Select(e => e.ErrorMessage)
                //                                .FirstOrDefault();
                return Page();
            }
            using var stream = ResumeFile.OpenReadStream();
            try
            {
                var extractedText = parser.ExtractText(stream, ResumeFile.FileName);

                // Save text to TempData or Session, redirect to analysis page
                TempData["ResumeText"] = extractedText;
                return RedirectToPage("../Analyze/AnalysisResult");
            }
            
            catch
            {
                throw;
            }
        }
    }
}
