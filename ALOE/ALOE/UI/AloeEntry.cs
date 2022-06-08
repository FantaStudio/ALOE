using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
namespace ALOE
{
    public class AloeEntry : Entry
    {
        public static readonly BindableProperty TintColorProperty = BindableProperty.Create(nameof(TintColor), typeof(Color), typeof(AloeEntry), Color.White);
        public static readonly BindableProperty HighlightColorProperty = BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(AloeEntry), Color.White);

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public Color HighlightColor
        {
            get { return (Color)GetValue(HighlightColorProperty); }
            set { SetValue(HighlightColorProperty, value); }
        }
    }
}
