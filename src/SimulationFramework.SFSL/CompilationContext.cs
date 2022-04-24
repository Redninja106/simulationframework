using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

internal class CompilationContext
{
    private List<CompilationNotification> notifications = new();

    public bool HasErrors()
    {
        return notifications.Count > 0;
    }

    public TargetLanguage TargetLanguage { get; private set; }

    public CompilationResult CreateResult()
    {
        return new CompilationResult()
        {
            Notifications = this.notifications.ToArray(),
            Language = this.TargetLanguage,
        };
    }

    public void Notify(CompilationNotification notification)
    {
        notifications.Add(notification);
    }
}
