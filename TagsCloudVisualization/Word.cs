using System.Drawing;
using System.Windows.Forms;

namespace TagsCloudVisualization
{
    class Word
    {
        public string Value { get; }
        public int Count{ get; }
        public Font Font { get; }
        public Size Size{ get; }
        public Rectangle LayoutRectangle { get; set; }
        public Word(string value, int count, int fontSize,  string fontFamily = "Calibri")
        {
            Value = value;
            Count = count;
            Font = new Font(new FontFamily(fontFamily), fontSize);
            Size = TextRenderer.MeasureText(Value, Font);
        }
    }
}
