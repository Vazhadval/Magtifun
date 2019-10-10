using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Magtifun2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void btnSend_Click(object sender, EventArgs e)
        {
            progressBar1.Maximum = 100;
            progressBar1.Step = 10;


            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtReceiver.Text) && !string.IsNullOrEmpty(txtMessage.Text))
            {
                progressBar1.PerformStep();
                btnSend.Enabled = false;
                progressBar1.Visible = true;

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(new List<string>() {
                 "--silent-launch",
                 "--no-startup-window",
                 "no-sandbox",
                 "headless",});

                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;    // This is to hidden the console.
                progressBar1.PerformStep();
                try
                {
                    using (ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions))
                    {
                        driver.Navigate().GoToUrl("http://www.magtifun.ge/");
                        progressBar1.PerformStep();

                        LoginToMagtifun(txtUsername.Text, txtPassword.Text, driver, progressBar1, lblSmsLeft);
                        SendSMS(txtReceiver.Text, txtMessage.Text, driver, progressBar1);

                        progressBar1.Value = 100;
                        MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar1.Visible = false;
                        progressBar1.Value = 0;
                        btnSend.Enabled = true;
                    }
                }
                catch (Exception)

                {
                    MessageBox.Show("Please make sure you filled all fields and credentials are correct", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    btnSend.Enabled = true;

                }

            }
            else
            {
                MessageBox.Show("Please make sure you filled all fields and credentials are correct.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }



        static void LoginToMagtifun(string username, string password, IWebDriver driver, ProgressBar progress, Label label)
        {
            driver.FindElement(By.Id("user")).SendKeys(username);
            progress.PerformStep();
            driver.FindElement(By.Id("password")).SendKeys(password);
            progress.PerformStep();
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            progress.PerformStep();
            label.Text = "SMS Left: " + (Convert.ToInt32(driver.FindElement(By.CssSelector("span[class='xxlarge dark english']")).Text) - 1).ToString();
            progress.PerformStep();
        }
        static void SendSMS(string receiver, string sms, IWebDriver driver, ProgressBar progress)
        {
            driver.FindElement(By.Id("recipient")).SendKeys(receiver);
            progress.PerformStep();
            driver.FindElement(By.Id("message_body")).SendKeys(sms);
            progress.PerformStep();
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            progress.PerformStep();
        }

    }
}
