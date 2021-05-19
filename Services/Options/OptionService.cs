using Infra.Models.Infra;

namespace Services.Options
{
    public class OptionService
    {
        public Option[] GetBoolean()
        {
            return new[]
                   {
                       new Option(false, "否"),
                       new Option(true, "是"),
                   };
        }
    }
}
