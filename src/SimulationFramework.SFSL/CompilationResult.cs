using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFSLPrototype;

public class CompilationResult
{
    public bool Succeeded => Notifications.Length == 0;

    public CompilationNotification[] Notifications;

    public string[] Results;

    public TargetLanguage Language;
}
