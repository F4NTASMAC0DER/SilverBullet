using System;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace RuriLib.LS
{
    /// Implements AvalonEdit ICompletionData interface to provide the entries in the
    /// completion drop down.
    public class LoliScriptCompletionData : ICompletionData
    {
        public LoliScriptCompletionData(string text, string description)
        {
            Text = text;
            Description = description;
        }

        ///<inheritdoc />
        public System.Windows.Media.ImageSource Image
        {
            get { return null; }
        }

        ///<inheritdoc />
        public string Text { get; private set; }

        ///<summary>Use this property if you want to show a fancy UIElement in the list.</summary>
        public object Content
        {
            get { return !string.IsNullOrWhiteSpace(Description.ToString()) ? Text + " (" + Description + ")" : Text; }
        }

        ///<inheritdoc />
        public object Description { get; private set; }

        ///<inheritdoc />
        public double Priority
        {
            get { return 0.0; }
        }

        ///<inheritdoc />
        public void Complete(TextArea textArea, ISegment completionSegment,
            EventArgs e)
        {
            textArea.Document.Replace(completionSegment, this.Text);
        }


        /// <summary>
        /// block parameters (REQUEST,...)
        /// </summary>
        public class BlockParameters
        {
            /// <summary>
            /// Create block Parameters
            /// </summary>
            /// <returns></returns>
            public static BlockParameters Create()
            {
                return new BlockParameters();
            }
        }
    }
}
