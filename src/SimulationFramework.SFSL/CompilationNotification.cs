using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public class CompilationNotification
{
    public CompilationNotification(int code, string description, NotificationSeverity severity)
    {
        this.Code = code;
        this.Description = description;
        this.Severity = severity;
    }

    public int Code { get; private set; }
    public string Description { get; private set; }
    public NotificationSeverity Severity { get; private set; }
}
