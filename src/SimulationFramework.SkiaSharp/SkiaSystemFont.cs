using SimulationFramework.Drawing;
using SkiaSharp;

namespace SimulationFramework.SkiaSharp;

class SkiaSystemFont : SkiaFont
{
    private readonly SKTypeface regular, italic, bold, boldItalic;

    public override bool SupportsBold => bold is not null;
    public override bool SupportsItalic => italic is not null;

    public override string Name => regular.FamilyName;

    public SkiaSystemFont(string name)
    {
        this.regular = SKTypeface.FromFamilyName(name, SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);
        this.italic = SKTypeface.FromFamilyName(name, SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);

        if (!this.italic.IsItalic)
        {
            this.italic.Dispose();
            this.italic = null;
        }

        this.bold = SKTypeface.FromFamilyName(name, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

        if (!this.bold.IsBold)
        {
            this.bold.Dispose();
            this.bold = null;
        }

        if (this.bold is not null && this.italic is not null)
        {
            this.boldItalic = SKTypeface.FromFamilyName(name, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Italic);
        }
    }

    public override SKTypeface GetTypeface(FontStyle style)
    {
        if (style.HasFlag(FontStyle.Bold))
        {
            if (style.HasFlag(FontStyle.Italic))
            {
                return boldItalic;
            }
            else
            {
                return bold;
            }
        }
        else if (style.HasFlag(FontStyle.Italic))
        {
            return italic;
        }
        else
        {
            return regular;
        }
    }

    public override void Dispose()
    {
        regular.Dispose();
        bold?.Dispose();
        italic?.Dispose();
        boldItalic?.Dispose();
        base.Dispose();
    }
}
