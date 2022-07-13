﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SimulationFramework.SFSL.Nodes;

namespace SimulationFramework.SFSL.Emit;

internal abstract class Emitter : DocumentVisitor
{
    public TextWriter Out { get; }

    // called before text from writer is used to allow emitter to finalize
    public abstract void Flush();

    public Emitter(TextWriter writer)
    {
        this.Out = writer;
    }
}
