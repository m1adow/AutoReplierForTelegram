using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutoReplierForTelegram
{
    class Program
    {
        static void Main(string[] args)
        {  
            IWebDriver webDriver = new ChromeDriver();

            string nameSender;
            string answer = "Ваше сообщение не было прочитано на протяжении 30 секунд. Был вызван автоответчик. " +
                "Мой господин \"Мяу\" куда-то пропал. " +
                "Прошу его простить, он сразу ответит, как только придет. Благодарю за понимание!";
         
            OpenTelegram(webDriver);
            Waiter(webDriver, senderAvatar);

            while (true)
            {
                try
                {
                    if (webDriver.FindElement(senderAvatar).Displayed && webDriver.FindElement(unRead).Displayed && webDriver.FindElement(classRp).GetAttribute("class") == "rp")
                    {
                        nameSender = webDriver.FindElement(name).Text;
                        Thread.Sleep(30000);
              
                        if (webDriver.FindElement(name).Text == nameSender && webDriver.FindElement(senderAvatar).Displayed && webDriver.FindElement(unRead).Displayed && webDriver.FindElement(classRp).GetAttribute("class") == "rp")
                        {
                            webDriver.FindElement(senderField).Click();
                            webDriver.FindElement(sendMessageField).SendKeys(answer);
                            webDriver.FindElement(sendButton).Click();
                            webDriver.FindElement(defaultChat).Click();
                        }
                        else { nameSender = ""; }   
                    }
                    else
                    {
                        webDriver.Navigate().Refresh();
                        Thread.Sleep(10000);
                    }
                }
                catch (Exception exc) { Console.WriteLine(exc.Message); }
            }     
        }

        static By senderField = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[3]/div[1]");
        static By unRead = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[3]/div[2]/p[2]/div");
        static By senderAvatar = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[3]/avatar-element");
        static By sendMessageField = By.XPath("//*[@id=\"column-center\"]/div/div/div[4]/div/div[1]/div[7]/div[1]/div[1]");
        static By sendButton = By.XPath("//*[@id=\"column-center\"]/div/div/div[4]/div/div[4]/button/div");
        static By defaultChat = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[1]/div[1]");
        static By classRp = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[4]");
        static By name = By.XPath("//*[@id=\"folders-container\"]/div/div[1]/ul/li[3]/div[2]/p[1]/span[1]/span");

        static void Waiter(IWebDriver webDriver, By by, int delay = 1000)
        {
            bool b = true;

            while (b)
                try
                {
                    if (webDriver.FindElement(by) == null)
                        Task.Delay(delay);
                    else
                        b = false;
                }
                catch
                {

                }
        }

        static void OpenTelegram(IWebDriver webDriver)
        {
            webDriver.Manage().Window.Maximize();
            webDriver.Navigate().GoToUrl("https://web.telegram.org/k/");
        }
    }    
}