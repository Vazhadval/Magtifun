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
        public static ChromeDriver driver;

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsername.Text) &&
                !string.IsNullOrEmpty(txtPassword.Text) &&
                !string.IsNullOrEmpty(txtReceiver.Text) &&
                !string.IsNullOrEmpty(txtMessage.Text))
            {
                progressBar.PerformStep();
                btnSend.Enabled = false;

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
                    using (driver = new ChromeDriver(chromeDriverService, chromeOptions))
                    {

                        LoginToMagtifun(txtUsername.Text, txtPassword.Text, lblSmsLeft, txtMessage);
                        progressBar.PerformStep();
                        SendSMS(txtReceiver.Text, txtMessage.Text);
                        progressBar.Value = 100;

                        MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        progressBar.Value = 0;
                        btnSend.Enabled = true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Please make sure you filled all fields and credentials are correct", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar.Value = 0;
                    btnSend.Enabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please make sure you filled all fields and credentials are correct.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        static void LoginToMagtifun(string username, string password, Label smsLeftLabel, TextBox txtMessage)
        {
            driver.Navigate().GoToUrl("http://www.magtifun.ge/");
            driver.FindElement(By.Id("user")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            smsLeftLabel.Text = "SMS Left: " + (Convert.ToInt32(driver.FindElement(By.CssSelector("span[class='xxlarge dark english']")).Text) - Math.Ceiling(Convert.ToDecimal(txtMessage.Text.Length) / 146)).ToString();
        }
        static void SendSMS(string receiver, string sms)
        {
            driver.FindElement(By.Id("recipient")).SendKeys(receiver);
            driver.FindElement(By.Id("message_body")).SendKeys(sms);
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

        private void TxtMessage_TextChanged(object sender, EventArgs e)
        {
            int smsTextCharCount = txtMessage.Text.Length;
            lblInputSize.Text = $"Input: {smsTextCharCount.ToString()}";

            if (smsTextCharCount > 0 && smsTextCharCount <= 146)
            {
                lblSmsCount.Text = "Will be sent: 1 sms";
            }
            else if (smsTextCharCount > 146 && smsTextCharCount <= 292)
            {
                lblSmsCount.Text = "Will be sent: 2 sms";
            }
            else
            {
                lblSmsCount.Text = "Will be sent: 3 sms";
            }

        }

        private void TxtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))

            {
                e.Handled = true;
            }
        }
    }
}
