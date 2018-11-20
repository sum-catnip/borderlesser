using System;
using System.ComponentModel;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Collections;
using System.Windows.Controls;
using System.IO;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Borderlesser {
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : MetroWindow {
      private System.Windows.Forms.NotifyIcon m_icon;
      private Main m_borderlesser;

      public IEnumerable StyleBinding {
         get { return Enum.GetNames(typeof(WindowStyles)); }
      }

      public IEnumerable StyleExBinding {
         get { return Enum.GetNames(typeof(WindowStylesEx)); }
      }

      public IEnumerable HotKeyModifiersBinding {
         get { return Enum.GetValues(typeof(HotKeyModifiers)); }
      }

      public int HotKeyModifierBinding {
         get { return (int)Config.Instance.HotKeyModifier; }
         set { m_borderlesser.SetHotkey(Config.Instance.Hotkey, (HotKeyModifiers)value); }
      }

      public bool AutoStartBinding {
         get { return Config.Instance.Autostart; }
         set { m_borderlesser.SetAutostart(value); }
      }

      public string FromXBinding {
         get { return Config.Instance.FromX.ToString(); }
         set {
            if(string.IsNullOrWhiteSpace(value)) Config.Instance.FromX = 0;
            else if (int.TryParse(value, out int res)) Config.Instance.FromX = res;
            else this.ShowMessageAsync("Error", "Failed to parse interger input in TextBox 'FromX'");
         }
      }

      public string FromYBinding {
         get { return Config.Instance.FromY.ToString(); }
         set {
            if (string.IsNullOrWhiteSpace(value)) Config.Instance.FromY = 0;
            else if (int.TryParse(value, out int res)) Config.Instance.FromY = res;
            else this.ShowMessageAsync("Error", "Failed to parse interger input in TextBox 'FromY'");
         }
      }

      public string ToXBinding {
         get { return Config.Instance.ScreenWidth.ToString(); }
         set {
            if (string.IsNullOrWhiteSpace(value)) Config.Instance.ScreenWidth = 0;
            else if (int.TryParse(value, out int res)) Config.Instance.ScreenWidth = res;
            else this.ShowMessageAsync("Error", "Failed to parse interger input in TextBox 'ToX'");
         }
      }

      public string ToYBinding {
         get { return Config.Instance.ScreenHeigth.ToString(); }
         set {
            if (string.IsNullOrWhiteSpace(value)) Config.Instance.ScreenHeigth = 0;
            else if (int.TryParse(value, out int res)) Config.Instance.ScreenHeigth = res;
            else this.ShowMessageAsync("Error", "Failed to parse interger input in TextBox 'ToY'");
         }
      }

      private static readonly Regex NonNumeric = new Regex("[^0-9.-]+", RegexOptions.Compiled);

      public MainWindow() {
         InitializeComponent();
         
         m_icon = new System.Windows.Forms.NotifyIcon {
            Visible = true,
            Icon = new System.Drawing.Icon(Application.GetResourceStream(new Uri("/frame.ico", UriKind.Relative)).Stream),
            ContextMenu = new System.Windows.Forms.ContextMenu(
               new System.Windows.Forms.MenuItem[] {
                  new System.Windows.Forms.MenuItem("Open", (sender, e) => { NotifyShow();        }),
                  new System.Windows.Forms.MenuItem("Quit", (sender, e) => { Environment.Exit(0); })})};

         m_icon.DoubleClick += (sender, e) => { NotifyShow(); };

         m_borderlesser = new Main(this);
         NotifyHide();

         // dis is a smoll project lets just be our own model view
         DataContext = this;
      }

      protected override void OnClosing(CancelEventArgs e) {
         e.Cancel = true;

         try { Config.Instance.WriteConfig(); }
         catch(IOException ex) {
            this.ShowMessageAsync("Error",
               "Failed to write config file. " +
               "Please make sure i have write permissions in this folder to write the config file. " +
               "I you don't know what im talking about, just try moving me to a new folder. " +
               "Details: \n" + ex.Message).ContinueWith((res) => {
                  Environment.Exit(-1);
               });

            return;
         }

         NotifyHide();
      }
      private void NotifyHide() {
         Hide();
         m_icon.Visible = true;
      }
      private void NotifyShow() {
         Show();
         m_icon.Visible = false;
      }

      private void MetroWindow_Loaded(object sender, RoutedEventArgs e) {
         UpdateWindowStyleGrid();
         UpdateWindowStyleExGrid();

         //NotifyHide();
      }

      private void UpdateWindowStyleGrid() {
         IEnumerable<string> selectedStyles = Enum.GetNames(typeof(WindowStyles))
            .Cast<string>()
            .Where(style => {
               return Config.Instance.Styles.HasFlag((Enum)Enum.Parse(typeof(WindowStyles), style));
            });

         foreach (string selected in selectedStyles) {
            StyleGrid.SelectedItems.Add(selected);
         }
      }

      private void UpdateWindowStyleExGrid() {
         IEnumerable<string> selectedStylesEx = Enum.GetNames(typeof(WindowStylesEx))
            .Cast<string>()
            .Where(style => {
               return Config.Instance.StylesEx.HasFlag((Enum)Enum.Parse(typeof(WindowStylesEx), style));
            });

         foreach(string selected in selectedStylesEx) {
            StyleGridEx.SelectedItems.Add(selected);
         }
      }

      private void HotkeyBox_keydown(object sender, KeyEventArgs e) {
         if(e.Key != Key.Escape) {
            TextBox tb = (TextBox)e.Source;
            e.Handled = true;
            tb.Text = e.Key.ToString();
            m_borderlesser.SetHotkey((System.Windows.Forms.Keys)KeyInterop.VirtualKeyFromKey(e.Key), Config.Instance.HotKeyModifier);
         }
      }

      private void HotkeyBox_initialized(object sender, EventArgs e) {
         TextBox tb = (TextBox)sender;
         tb.Text = KeyInterop.KeyFromVirtualKey((int)Config.Instance.Hotkey).ToString();
      }

      private void StyleGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
         DataGrid grid = (DataGrid)sender;
         HitTestResult hitTestResult = VisualTreeHelper.HitTest(grid, e.GetPosition(grid));
         DataGridRow row = hitTestResult.VisualHit.TryFindParent<DataGridRow>();
         if (row != null) {
            e.Handled = true;
            if (row.IsSelected) Config.Instance.Styles ^= (WindowStyles)Enum.Parse(typeof(WindowStyles), (string)row.Item);
            else                Config.Instance.Styles |= (WindowStyles)Enum.Parse(typeof(WindowStyles), (string)row.Item);

            row.IsSelected = !row.IsSelected;
            UpdateWindowStyleGrid();
         }
      }

      private void StyleGridEx_PreviewMouseDown(object sender, MouseButtonEventArgs e) {
         DataGrid grid = (DataGrid)sender;
         HitTestResult hitTestResult = VisualTreeHelper.HitTest(grid, e.GetPosition(grid));
         DataGridRow row = hitTestResult.VisualHit.TryFindParent<DataGridRow>();
         if (row != null) {
            e.Handled = true;
            if (row.IsSelected) Config.Instance.StylesEx ^= (WindowStylesEx)Enum.Parse(typeof(WindowStylesEx), (string)row.Item);
            else                Config.Instance.StylesEx |= (WindowStylesEx)Enum.Parse(typeof(WindowStylesEx), (string)row.Item);

            row.IsSelected = !row.IsSelected;
            UpdateWindowStyleExGrid();
         }
      }

      private void CheckNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e) {
         e.Handled = NonNumeric.IsMatch(e.Text);
      }

      private void CheckNumeric_Pasting(object sender, DataObjectPastingEventArgs e) {
         if (e.DataObject.GetDataPresent(typeof(string))) {
            string text = (string)e.DataObject.GetData(typeof(string));
            if (NonNumeric.IsMatch(text)) e.CancelCommand();
         }
         else e.CancelCommand();
      }
   }
}
