using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

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
            progressBar.Maximum = 100;
            progressBar.Step = 10;


            if (!string.IsNullOrEmpty(txtUsername.Text) && !string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtReceiver.Text) && !string.IsNullOrEmpty(txtMessage.Text))
            {
                progressBar.PerformStep();
                btnSend.Enabled = false;
                progressBar.Visible = true;

                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments(new List<string>() {
                 "--silent-launch",
                 "--no-startup-window",
                 "no-sandbox",
                 "headless",});

                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;    // This is to hidden the console.
                progressBar.PerformStep();
                try
                {
                    using (ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions))
                    {
                        driver.Navigate().GoToUrl("http://www.magtifun.ge/");
                        progressBar.PerformStep();

                        LoginToMagtifun(txtUsername.Text, txtPassword.Text, driver, progressBar, lblSmsLeft);
                        SendSMS(txtReceiver.Text, txtMessage.Text, driver, progressBar);

                        progressBar.Value = 100;
                        MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        progressBar.Visible = false;
                        progressBar.Value = 0;
                        btnSend.Enabled = true;
                    }
                }
                catch (Exception)

                {
                    MessageBox.Show("Please make sure you filled all fields and credentials are correct", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar.Visible = false;
                    progressBar.Value = 0;
                    btnSend.Enabled = true;

                }

            }
            else
            {
                MessageBox.Show("Please make sure you filled all fields and credentials are correct.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }


        }



        static void LoginToMagtifun(string username, string password, IWebDriver driver, ProgressBar progressBar, Label smsLeftLabel)
        {
            driver.FindElement(By.Id("user")).SendKeys(username);
            progressBar.PerformStep();
            driver.FindElement(By.Id("password")).SendKeys(password);
            progressBar.PerformStep();
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            progressBar.PerformStep();
            smsLeftLabel.Text = "SMS Left: " + (Convert.ToInt32(driver.FindElement(By.CssSelector("span[class='xxlarge dark english']")).Text) - 1).ToString();
            progressBar.PerformStep();
        }
        static void SendSMS(string receiver, string sms, IWebDriver driver, ProgressBar progressBar)
        {
            driver.FindElement(By.Id("recipient")).SendKeys(receiver);
            progressBar.PerformStep();
            driver.FindElement(By.Id("message_body")).SendKeys(sms);
            progressBar.PerformStep();
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            progressBar.PerformStep();
        }

    }
}
