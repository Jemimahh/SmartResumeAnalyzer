# Smart Resume Analyzer
An AI-powered resume analysis tool that runs locally. Upload your resume, get intelligent feedback, and improve your chances of landing the job.

## Features
- Resume text extraction and preprocessing
- AI resume analysis using a local LLM (via Ollama)
- Feedback breakdown:
      - Score (0–100)
      - Strengths & Weaknesses
      - Missing Keywords
      - Recommendations
- Secure: No data is uploaded or stored — all processing is local
- Client- and server-side file validation (.pdf, .docx, .txt)

## Stack
- Razor Pages (.NET 8)
- ASP.NET Core Web App (.NET 8)
- Ollama
- Models: phi4-mini
- Parsing: System.Text.Json, Regex, HttpClient
- Validation: Custom DataAnnotations + JS
