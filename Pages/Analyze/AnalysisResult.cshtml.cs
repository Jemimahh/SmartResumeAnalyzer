using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SmartResumeAnalyzer.Pages.Analyze
{
    public class AnalysisResultModel(IConfiguration config) : PageModel
    {
        private readonly IConfiguration _config = config;

        [BindProperty(SupportsGet = true)]
        public string ResumeText { get; set; } = string.Empty;

        public ResumeFeedback Feedback { get; set; }
        public string ErrorMessage { get; set; }
        public IActionResult OnGet()
        {
            ResumeText = (string)TempData["ResumeText"];


            if (string.IsNullOrWhiteSpace(ResumeText))
            {
                ErrorMessage = "No resume text was provided.";
                return Page();
            }

            try
            {
                Feedback = CallLocalLlm(ResumeText);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Something went wrong: {ex.Message}. Make sure Ollama is running and try again.";
            }

            return Page();
        }
        private ResumeFeedback CallLocalLlm(string resumeText)
        {

            using var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(3)
            };

            var prompt = @$"You are a professional resume reviewer. Analyze the following resume text and return the response in the following JSON format:
    {{
  ""Score"": int,
  ""Strengths"": string,
  ""Weaknesses"": string,
  ""MissingKeywords"": [string],
  ""Recommendations"": string
}}

Do not use markdown, backticks, or formatting. Only valid JSON.

Here is the resume: " + resumeText;

        var payload = new
        {
            model = "phi4-mini",
            prompt = prompt,
            stream = false // Important for performance and parsing
        };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = client.PostAsync("http://localhost:11434/api/generate", content).Result;
            var responseText = response.Content.ReadAsStringAsync().Result;

            var parsed = JsonDocument.Parse(responseText);
            var reply = parsed.RootElement.GetProperty("response").GetString();
            var cleaned = reply
       .Replace("“", "\"")
       .Replace("”", "\"")
       .Replace("‘", "'")
       .Replace("’", "'")
       .Replace("```json", "")     // Remove markdown formatting
    .Replace("```", "")         // Remove ending backticks
       .Trim();
            // Optional: Strip any trailing commas
            cleaned = Regex.Replace(cleaned, @",\s*([}\]])", "$1");
            var results = JsonSerializer.Deserialize<ResumeFeedback>(cleaned);
            return results;
           
        }
    }
}
public class ResumeFeedback
{
    public int Score { get; set; }
    public string Strengths { get; set; }
    public string Weaknesses { get; set; }
    public List<string> MissingKeywords { get; set; }
    public string Recommendations { get; set; }
}
