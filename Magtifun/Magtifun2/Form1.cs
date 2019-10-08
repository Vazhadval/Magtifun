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

            try
            {
                using (ChromeDriver driver = new ChromeDriver(chromeDriverService, chromeOptions))
                {
                    driver.Navigate().GoToUrl("http://www.magtifun.ge/");

                    LoginToMagtifun(txtUsername.Text, txtPassword.Text, driver, progressBar1, lblSmsLeft);
                    SendSMS(txtReceiver.Text, txtMessage.Text, driver, progressBar1);
                    progressBar1.Value = 100;
                    MessageBox.Show("Message sent successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    btnSend.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                progressBar1.Visible = false;
                btnSend.Enabled = true;

            }


        }



        static void LoginToMagtifun(string username, string password, IWebDriver driver, ProgressBar progress, Label label)
        {
            driver.FindElement(By.Id("user")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
            label.Text = "SMS Left: " + (Convert.ToInt32(driver.FindElement(By.CssSelector("span[class='xxlarge dark english']")).Text) - 1).ToString();

        }
        static void SendSMS(string receiver, string sms, IWebDriver driver, ProgressBar progress)
        {
            Thread.Sleep(100);
            driver.FindElement(By.Id("recipient")).SendKeys(receiver);
            driver.FindElement(By.Id("message_body")).SendKeys(sms);
            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        }

    }
}
