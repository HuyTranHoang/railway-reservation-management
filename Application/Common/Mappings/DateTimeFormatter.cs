namespace Application.Common.Mappings;

public class DateTimeFormatter : IValueConverter<DateTime, string>
{
    public string Convert(DateTime sourceMember, ResolutionContext context)
    {
        return sourceMember.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
