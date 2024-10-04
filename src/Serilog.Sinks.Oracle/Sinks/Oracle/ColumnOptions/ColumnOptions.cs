using System.Collections.ObjectModel;

namespace Serilog.Sinks.Oracle;
public partial class ColumnOptions
{
    private ICollection<StandardColumn> _store;

    public ColumnOptions()
    {
        Id = new IdColumnOptions();
        Level = new LevelColumnOptions();
        TraceId = new TraceIdColumnOptions();
        SpanId = new SpanIdColumnOptions();
        Properties = new PropertiesColumnOptions();
        Message = new MessageColumnOptions();
        MessageTemplate = new MessageTemplateColumnOptions();
        TimeStamp = new TimeStampColumnOptions();
        Exception = new ExceptionColumnOptions();
        LogEvent = new LogEventColumnOptions();

        Store = new Collection<StandardColumn>
            {
                StandardColumn.Id,
                StandardColumn.Message,
                StandardColumn.MessageTemplate,
                StandardColumn.Level,
                StandardColumn.TimeStamp,
                StandardColumn.Exception,
                StandardColumn.Properties
            };

        PrimaryKey = Id; // for backwards-compatibility, ignored if Id removed from Store
    }

    public ICollection<StandardColumn> Store
    {
        get { return _store; }
        set
        {
            if (value == null)
            {
                _store = new Collection<StandardColumn>();
                foreach (StandardColumn column in Enum.GetValues(typeof(StandardColumn)))
                {
                    _store.Add(column);
                }
            }
            else
            {
                _store = value;
            }
        }
    }

    public OracleColumn PrimaryKey { get; set; }
    public bool ClusteredColumnstoreIndex { get; set; }
    public bool DisableTriggers { get; set; }
    public ICollection<OracleColumn> AdditionalColumns { get; set; }
    public IdColumnOptions Id { get; private set; }
    public LevelColumnOptions Level { get; private set; }
    public TraceIdColumnOptions TraceId { get; private set; }
    public SpanIdColumnOptions SpanId { get; private set; }
    public PropertiesColumnOptions Properties { get; private set; }
    public ExceptionColumnOptions Exception { get; set; }
    public MessageTemplateColumnOptions MessageTemplate { get; set; }
    public MessageColumnOptions Message { get; set; }
    public TimeStampColumnOptions TimeStamp { get; private set; }
    public LogEventColumnOptions LogEvent { get; private set; }

    internal OracleColumn GetStandardColumnOptions(StandardColumn standardColumn)
    {
        switch (standardColumn)
        {
            case StandardColumn.Id: return Id;
            case StandardColumn.Level: return Level;
            case StandardColumn.TraceId: return TraceId;
            case StandardColumn.SpanId: return SpanId;
            case StandardColumn.TimeStamp: return TimeStamp;
            case StandardColumn.LogEvent: return LogEvent;
            case StandardColumn.Message: return Message;
            case StandardColumn.MessageTemplate: return MessageTemplate;
            case StandardColumn.Exception: return Exception;
            case StandardColumn.Properties: return Properties;
            default: return null;
        }
    }
}
