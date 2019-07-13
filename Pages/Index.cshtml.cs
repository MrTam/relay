using Microsoft.AspNetCore.Mvc.RazorPages;
using Relay.Models;

namespace Relay.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LineupContext _context;
        
        public IndexModel(LineupContext context)
        {
            _context = context;
        }
        
        public void OnGet()
        {

        }
    }
}
