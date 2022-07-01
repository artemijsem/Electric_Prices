
using Electric_Prices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Electric_Prices.Models;
using Electric_Prices.Services;

namespace Electric_Prices.Pages;
using Electric_Prices.Models;

public class IndexModel : PageModel
{


    public IList<EpApiTable> EpApiTable { get; set; } = default!;

    private readonly ILogger<IndexModel> _logger;
    private readonly Electric_Prices.Models.Electric_Prices_Context _context;

    private Electric_Prices.Services.RetrieveData getData = new Electric_Prices.Services.RetrieveData();


    public IndexModel(ILogger<IndexModel> logger, Electric_Prices.Models.Electric_Prices_Context context)
    {
        _logger = logger;
        _context = context;

    }


    public void OnGet()
    {

    }




    public void OnPostRefreshData()
    {
        getData.Main();


    }

    // Function to get the price per hour
    public void OnPostSubmitTimePrice()
    {

        string date_output = Request.Form["Hourly_Time_Input"];
        if (date_output != String.Empty)
        {
            DateTime date_output_format = Convert.ToDateTime(date_output);
            string date_output_format_string = date_output_format.ToString("dd/MM/yyyy HH");

            this.EpApiTable = (from data in this._context.EpApiTables
                               select data).ToList();
            foreach (var item in EpApiTable)
            {

                string formated_db_date = item.Date.ToString("dd/MM/yyyy HH");
                if (String.Compare(formated_db_date, date_output_format_string) == 0)
                {
                    ViewData["PricePerHour_Price"] = item.Price;
                    ViewData["PricePerHour_Date"] = date_output;
                }

            }
        }




    }

    public void OnPostSubmitDailyPrice()
    {
        double daily_price = 0;
        double overall_price = 0;
        int days_num = 0;

        string input_date_1 = Request.Form["Daily_Time_Input_01"];
        string input_date_2 = Request.Form["Daily_Time_Input_02"];

        DateTime DT_input_date_1 = Convert.ToDateTime(input_date_1);
        DateTime DT_input_date_2 = Convert.ToDateTime(input_date_2);
        string st_input_date_1 = DT_input_date_1.ToString("yyyy-MM-dd HH:mm:ss");
        string st_input_date_2 = DT_input_date_2.ToString("yyyy-MM-dd HH:mm:ss");
        if (input_date_1 != String.Empty && input_date_1 != String.Empty)
        {
            DateTime raw_input_date_01 = Convert.ToDateTime(st_input_date_1);
            TimeSpan ts_date_1 = new TimeSpan(raw_input_date_01.Hour, 0, 0);
            DT_input_date_1 = raw_input_date_01.Date + ts_date_1;
            DateTime raw_date_daily_output_02 = Convert.ToDateTime(st_input_date_2);
            TimeSpan ts_date_2 = new TimeSpan(raw_date_daily_output_02.Hour, 0, 0);
            DT_input_date_2 = raw_date_daily_output_02.Date + ts_date_2;

            this.EpApiTable = (from data in this._context.EpApiTables
                               select data).ToList();
            foreach (var item in EpApiTable)
            {
                string st_item = item.Date.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime raw_item_date = Convert.ToDateTime(st_item);
                TimeSpan ts_item = new TimeSpan(raw_item_date.Hour, 0, 0);
                DateTime item_date = raw_item_date.Date + ts_item;


                Console.WriteLine("item_date = " + item_date);
                Console.WriteLine("st_item = " + st_item);

                if (DateTime.Compare(item_date, DT_input_date_1) >= 0 && DateTime.Compare(item_date, DT_input_date_2) <= 0)
                {
                    Console.WriteLine("Found appropriate date");
                    overall_price += item.Price;
                    days_num++;
                    daily_price = overall_price / days_num;

                }
            }
            Console.WriteLine("daily_price = " + daily_price);
            Console.WriteLine("overall_price = " + overall_price);
            Console.WriteLine("days_num = " + days_num);
            Console.WriteLine("DT_input_date_1 = " + DT_input_date_1);
            Console.WriteLine("DT_input_date_2= " + DT_input_date_2);

            ViewData["Daily_Price"] = Math.Round(daily_price, 2);

        }
    }



}
