public class Current
{
    static DateTime? now;

    public static DateTime Time
    {
        get
        {
            return now ?? DateTime.Now;
        }
        set
        {
            now = value;
        }
    }

    public static DateTimeOffset Offset
    {
        get { return new DateTimeOffset(Time); }
    }

    static Guid? guid;

    public static Guid Guid
    {
        get
        {
            return guid ?? System.Guid.NewGuid();
        }
        set
        {
            guid = value;
        }
    }
}