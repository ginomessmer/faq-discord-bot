namespace FaqDiscordBot.Providers.Azure.Data
{
    public class MetadataDto
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public MetadataDto(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public MetadataDto()
        {
        }
    }
}