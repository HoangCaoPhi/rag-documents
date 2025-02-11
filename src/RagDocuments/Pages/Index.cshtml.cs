using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RagDocuments.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Question { get; set; } = string.Empty;

        public string? Response { get; set; }

        public void OnPost()
        {
            // Fake AI Response (có thể thay bằng API OpenAI)
            Response = GenerateResponse(Question);
        }

        private string GenerateResponse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Xin hãy đặt một câu hỏi hợp lệ.";

            return $"Bạn hỏi: '{input}' - Tôi chưa thông minh lắm nhưng tôi sẽ cố gắng trả lời!";
        }
    }
}
