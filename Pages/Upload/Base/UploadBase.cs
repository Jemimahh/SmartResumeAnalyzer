using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartResumeAnalyzer.Services;

namespace SmartResumeAnalyzer.Pages.Upload.Base
{
    public class UploadBase(ResumeParserService parser) : PageModel
    {
        private readonly ResumeParserService _parser = parser;

        [BindProperty]
        public IFormFile ResumeFile { get; set; }

        public string ExtractedText { get; set; }
    }


}
 