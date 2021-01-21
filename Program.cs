using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            bool isbottom;
            
            using (IWebDriver driver = new ChromeDriver())
            {

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                driver.Navigate().GoToUrl("https://m.gizabet510.com/tr/bet/sports/football");

                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                Thread.Sleep(3000);


                HtmlAgilityPack.HtmlDocument dokuman = new HtmlAgilityPack.HtmlDocument();
                dokuman.LoadHtml(driver.PageSource);

                var nationalCount = dokuman.DocumentNode.SelectNodes("//*[@id='allCnt']//div[contains(@class,'list-btn-cont')]/a");

                for (int nc = 0; nc < nationalCount.Count; nc++)
                {


                    var nationalPath = nationalCount[nc].XPath;
                    Thread.Sleep(1000);
                    var macVarmi2 = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']//div[contains(@class,'message-box')]//div[contains(@class,'ng-star-inserted')]") == null ? "devam" : "oops";
                    if (macVarmi2 == "oops")
                    {
                        driver.Navigate().Back();
                        continue;
                    }

                    wait.Until(x => x.FindElement(By.XPath(nationalPath))).Click();

                    Thread.Sleep(1000);

                    isbottom = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']/app-bottom-menu/div[1]") != null ? true : false;
                    if (isbottom)
                    {
                        var bottom = driver.FindElement(By.XPath("//*[@id='allCnt']/app-bottom-menu/div[1]"));
                        js.ExecuteScript("arguments[0].style.display = 'none';", bottom);
                    }


                    dokuman.LoadHtml(driver.PageSource);
                    var datas = dokuman.DocumentNode.SelectNodes("//div[@class='modul-content']//div[contains(@class,'match-content')]");
                    if (datas == null || datas.Count == 0)
                    {
                        continue;
                    }
                    for (int count = 0; count < datas.Count; count++)
                    {
                        var insideData = datas[count].XPath + "//span[contains(@class,'other-btn')]";
                        Thread.Sleep(2000);

                        isbottom = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']/app-bottom-menu/div[1]") != null ? true : false;
                        if (isbottom)
                        {
                            var bottom = driver.FindElement(By.XPath("//*[@id='allCnt']/app-bottom-menu/div[1]"));
                            js.ExecuteScript("arguments[0].style.display = 'none';", bottom);
                        }
                      
                        wait.Until(x => x.FindElement(By.XPath(insideData))).Click();
                        

                        Thread.Sleep(1000);


                        dokuman.LoadHtml(driver.PageSource);

                        Thread.Sleep(2000);
                        try
                        {
                            var hometown = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]//div[1]/div[2]").InnerText;

                            var macTarihi = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]//div[contains(@class,'date')]").InnerText;
                            var homeaway = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]/div[3]").InnerText;
                            Console.WriteLine("*****************************MAÇ******************************");
                            Console.WriteLine("Ev Saihibi :" + hometown + "Mac Tarihi :" + macTarihi + "Deplasman :" + homeaway);
                        }
                        catch (Exception)
                        {
                            driver.Navigate().Back();
                            driver.Navigate().Forward();
                            count--;
                            continue;

                        }
    

                        var headersCount = dokuman.DocumentNode.SelectNodes("//fixture-detail//div[contains(@id,'type1')]/div");

                        for (int hc = 0; hc < headersCount.Count; hc++)
                        {

                            var headerTextPath = headersCount[hc].XPath + "//span[contains(@class,'header-text')]";

                            var headerText = dokuman.DocumentNode.SelectSingleNode(headerTextPath).InnerText;
                            Console.WriteLine("Başlık :" + headerText);

                            //Her bir headerın iç kısmı için path buluyoruz
                            var path = headersCount[hc].XPath + "//div[contains(@class,'modul-accordion')]";

                            var bahisCount = dokuman.DocumentNode.SelectNodes(path);
                            for (int bc = 0; bc < bahisCount.Count; bc++)
                            {
                                var bahisHeaderPath = bahisCount[bc].XPath + "//span[contains(@class,'header-text')]";

                                var bahisHeaderText = dokuman.DocumentNode.SelectSingleNode(bahisHeaderPath).InnerText;

                                Console.WriteLine("Bashis Başlıkları" + bahisHeaderText);
                            }

                        }
                        driver.Navigate().Back();
                        Thread.Sleep(1000);
                    }
                    dokuman.LoadHtml(driver.PageSource);
                    var leagueCount = dokuman.DocumentNode.SelectNodes("//*[@id='allCnt']//a[contains(@class,'list-btn')]");

                    if (leagueCount == null || leagueCount.Count == 0)
                    {
                        driver.Navigate().Back();
                        continue;
                    }
                    for (int lc = 0; lc < leagueCount.Count; lc++)
                    {
                        var leaguePath = leagueCount[lc].XPath;

                        isbottom = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']/app-bottom-menu/div[1]") != null ? true : false;

                        if (isbottom)
                        {
                            var bottom = driver.FindElement(By.XPath("//*[@id='allCnt']/app-bottom-menu/div[1]"));
                            js.ExecuteScript("arguments[0].style.display = 'none';", bottom);
                        }

                        var macVarmi = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']//div[contains(@class,'message-box')]//div[contains(@class,'ng-star-inserted')]") == null ? "devam" : "oops";
                        if (macVarmi=="oops")
                        {
                            driver.Navigate().Back();
                            continue;
                        }
                        try
                        {

                          wait.Until(x => x.FindElement(By.XPath(leaguePath))).Click();
                        }
                        catch (Exception)
                        {

                            driver.Navigate().Refresh();
                        }

                        Thread.Sleep(2000);


                        dokuman.LoadHtml(driver.PageSource);

                        //Normal alınan ligin dışında alınan şampiyonaların verileri örn: premir ligin bir altında çıkan ligler
                      var datasAlt = dokuman.DocumentNode.SelectNodes("//div[@class='modul-content']//div[contains(@class,'match-content')]");
                        if (datasAlt == null || datasAlt.Count == 0)
                        {
                            continue;
                        }
                        for (int count = 0; count < datasAlt.Count; count++)
                        {
                            try
                            {
                                var insideData = datasAlt[count].XPath + "//span[contains(@class,'other-btn')]";
                                Thread.Sleep(2000);

                                isbottom = dokuman.DocumentNode.SelectSingleNode("//*[@id='allCnt']/app-bottom-menu/div[1]") != null ? true : false;
                                if (isbottom)
                                {
                                    var bottom = driver.FindElement(By.XPath("//*[@id='allCnt']/app-bottom-menu/div[1]"));
                                    js.ExecuteScript("arguments[0].style.display = 'none';", bottom);
                                }


                                wait.Until(x => x.FindElement(By.XPath(insideData))).Click();


                                Thread.Sleep(2000);


                                dokuman.LoadHtml(driver.PageSource);

                                try
                                {
                                    var hometown = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]//div[1]/div[2]").InnerText;

                                    var macTarihi = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]//div[contains(@class,'date')]").InnerText;
                                    var homeaway = dokuman.DocumentNode.SelectSingleNode("//div[contains(@class,'detail-top-info')]/div[3]").InnerText;
                                    Console.WriteLine("*****************************MAÇ******************************");
                                    Console.WriteLine("Ev Saihibi :" + hometown + "Mac Tarihi :" + macTarihi + "Deplasman :" + homeaway);
                                }
                                catch (Exception)
                                {
                                    driver.Navigate().Back();
                                    driver.Navigate().Forward();
                                    count--;
                                    continue;

                                }
                              
                                var headersCount = dokuman.DocumentNode.SelectNodes("//fixture-detail//div[contains(@id,'type1')]/div");

                                for (int hc = 0; hc < headersCount.Count; hc++)
                                {

                                    var headerTextPath = headersCount[hc].XPath + "//span[contains(@class,'header-text')]";

                                    var headerText = dokuman.DocumentNode.SelectSingleNode(headerTextPath).InnerText;
                                    Console.WriteLine("Başlık :" + headerText);

                                    //Her bir headerın iç kısmı için path buluyoruz
                                    var path = headersCount[hc].XPath + "//div[contains(@class,'modul-accordion')]";

                                    var bahisCount = dokuman.DocumentNode.SelectNodes(path);
                                    for (int bc = 0; bc < bahisCount.Count; bc++)
                                    {
                                        var bahisHeaderPath = bahisCount[bc].XPath + "//span[contains(@class,'header-text')]";

                                        var bahisHeaderText = dokuman.DocumentNode.SelectSingleNode(bahisHeaderPath).InnerText;

                                        Console.WriteLine("Bashis Başlıkları" + bahisHeaderText);
                                    }

                                }
                                driver.Navigate().Back();
                                Thread.Sleep(1500);
                            }
                            catch (Exception)
                            {
                                continue;
                            }
                            
                        }

                        driver.Navigate().Back();
                        Thread.Sleep(1500);
                    }

                }
                Console.ReadLine();
            }


        }

    }
}