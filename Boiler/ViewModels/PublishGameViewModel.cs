using Boiler.Models;

namespace Boiler.ViewModels
{
    public class PublishGameViewModel
    {

        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public string[]? AchievementsNames { get; set; }

        public int[]? Categories { get; set; }
    }
}
