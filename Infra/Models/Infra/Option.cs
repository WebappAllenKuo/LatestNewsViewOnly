namespace Infra.Models.Infra
{
    public class Option
    {
        public Option(object value, string text)
        {
            Value = value;
            Text  = text;
        }

        public string Text { get; set; }

        public object Value { get; set; }
    }
}
