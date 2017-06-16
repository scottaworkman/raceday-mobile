using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RaceDay
{
    public enum SnackbarDuration
    {
        Long = 0,
        Short = -1,
    };

    public class SnackbarOptions
    {
        public string Text { get; set; }
        public SnackbarDuration Duration { get; set; }
        public VisualElement FloatingButton { get; set; }
    };

    public enum SnackbarAction
    {
        Create = 0,
        Timeout = 1,
        Clicked = 2,
        Dismissed = 3,
        ApplicationHidden = 4,
        Failed = 16,
        NotApplicable = 32
    }

    public interface ISnackbarResult
    {
        SnackbarAction Action { get; set; }
    }

    public interface ISnackbar
    {
        Task<ISnackbarResult> Show(SnackbarOptions options);
        Task<ISnackbarResult> Notify();
        int GetHeight();
    }
}
