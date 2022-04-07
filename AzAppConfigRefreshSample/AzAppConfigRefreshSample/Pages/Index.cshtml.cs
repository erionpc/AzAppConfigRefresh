using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace AzAppConfigRefreshSample.Pages
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly IOptionsSnapshot<Config> _configOptions;

        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IOptionsSnapshot<Config> configOptions, IConfiguration configuration)
        {
            _logger = logger;
            _configOptions = configOptions;
            _configuration = configuration; 
        }

        public string? StrongTypedSetting1 => _configOptions.Value.Setting1;

        public string? Setting1 => _configuration["Config:Setting1"];


        public void OnGet()
        {

        }
    }
}