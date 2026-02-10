using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace Pos_system
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Set window properties
            this.Text = "POS System";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Initialize WebView2
            InitializeWebView();
        }

        async void InitializeWebView()
        {
            try
            {
                // Ensure WebView2 runtime is initialized
                await webView21.EnsureCoreWebView2Async(null);

                // Enable DevTools for debugging (optional - remove in production)
                webView21.CoreWebView2.Settings.AreDevToolsEnabled = true;

                // Enable context menu
                webView21.CoreWebView2.Settings.AreDefaultContextMenusEnabled = true;

                // Locate the HTML file
                string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "public", "index.html");

                // Check if file exists
                if (File.Exists(htmlPath))
                {
                    // Navigate to the local HTML file
                    webView21.Source = new Uri(htmlPath);

                    // Optional: Handle navigation completed
                    webView21.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
                }
                else
                {
                    // Show detailed error message
                    string errorMessage = $"HTML file not found!\n\n" +
                                        $"Expected location:\n{htmlPath}\n\n" +
                                        $"Instructions:\n" +
                                        $"1. Create a 'public' folder in your project\n" +
                                        $"2. Add index.html to the 'public' folder\n" +
                                        $"3. Right-click index.html → Properties\n" +
                                        $"4. Set 'Copy to Output Directory' to 'Copy if newer'\n" +
                                        $"5. Rebuild the project";

                    MessageBox.Show(errorMessage, "File Not Found",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (WebView2RuntimeNotFoundException)
            {
                MessageBox.Show(
                    "Microsoft Edge WebView2 Runtime is not installed!\n\n" +
                    "Please download and install it from:\n" +
                    "https://go.microsoft.com/fwlink/p/?LinkId=2124703\n\n" +
                    "After installation, restart the application.",
                    "WebView2 Runtime Required",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error initializing POS system:\n\n{ex.Message}\n\n" +
                    $"Stack Trace:\n{ex.StackTrace}",
                    "Initialization Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                // Successfully loaded
                this.Text = "POS System - Ready";
            }
            else
            {
                MessageBox.Show(
                    $"Failed to load the page.\n\nError Status: {e.WebErrorStatus}",
                    "Navigation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        // Optional: Handle window closing to save state or confirm exit
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Uncomment to confirm before closing
            var result = MessageBox.Show(
                "Are you sure you want to close the POS system?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            
             if (result == DialogResult.No)
             {
                 e.Cancel = true;
             }
        }
    }
}