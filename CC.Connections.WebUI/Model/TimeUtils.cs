using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CC.Connections.WebUI.Model
{
    public class TimeUtils
    {
        public static List<SelectListItem> TimeList = new List<SelectListItem>() {
                new SelectListItem {
                    Text = "12:00 AM", Value = "0"},
                new SelectListItem {
                    Text = "12:30 AM", Value = "1"},
                new SelectListItem {
                    Text = "1:00 AM", Value = "2"},
                new SelectListItem {
                    Text = "1:30 AM", Value = "3"},
                new SelectListItem {
                    Text = "2:00 AM", Value = "4"},
                new SelectListItem {
                    Text = "2:30 AM", Value = "5"},
                new SelectListItem {
                    Text = "3:00 AM", Value = "6"},
                new SelectListItem {
                    Text = "3:30 AM", Value = "7"},
                new SelectListItem {
                    Text = "4:00 AM", Value = "8"},
                new SelectListItem {
                    Text = "4:30 AM", Value = "9"},
                new SelectListItem {
                    Text = "5:00 AM", Value = "10"},
                new SelectListItem {
                    Text = "5:30 AM", Value = "11"},
                new SelectListItem {
                    Text = "6:00 AM", Value = "12"},
                new SelectListItem {
                    Text = "6:30 AM", Value = "13"},
                new SelectListItem {
                    Text = "7:00 AM", Value = "14"},
                new SelectListItem {
                    Text = "7:30 AM", Value = "15"},
                new SelectListItem {
                    Text = "8:00 AM", Value = "16"},
                new SelectListItem {
                    Text = "8:30 AM", Value = "17"},
                new SelectListItem {
                    Text = "9:00 AM", Value = "18"},
                new SelectListItem {
                    Text = "9:30 AM", Value = "19" },
                new SelectListItem {
                    Text = "10:00 AM", Value ="20" },
                new SelectListItem {
                    Text = "10:30 AM", Value ="21" },
                new SelectListItem {
                    Text = "11:00 AM", Value ="22" },
                new SelectListItem {
                    Text = "11:30 AM", Value ="23" },
                new SelectListItem {
                    Text = "12:00 PM", Value ="24" },
                new SelectListItem {
                    Text = "12:30 PM", Value ="25" },
                 new SelectListItem {
                    Text = "1:00 PM", Value = "26"},
                new SelectListItem {
                    Text = "1:30 PM", Value = "27"},
                new SelectListItem {
                    Text = "2:00 PM", Value = "28"},
                new SelectListItem {
                    Text = "2:30 PM", Value = "29"},
                new SelectListItem {
                    Text = "3:00 PM", Value = "30"},
                new SelectListItem {
                    Text = "3:30 PM", Value = "31" },
                new SelectListItem {
                    Text = "4:00 PM", Value = "32"},
                new SelectListItem {
                    Text = "4:30 PM", Value = "33"},
                new SelectListItem {
                    Text = "5:00 PM", Value = "34" },
                new SelectListItem {
                    Text = "5:30 PM", Value = "35"},
                new SelectListItem {
                    Text = "6:00 PM", Value = "36"},
                new SelectListItem {
                    Text = "6:30 PM", Value = "37"},
                new SelectListItem {
                   Text = "7:00 PM", Value =  "38"},
                new SelectListItem {
                    Text = "7:30 PM", Value = "39"},
                new SelectListItem {
                    Text = "8:00 PM", Value = "40"},
                new SelectListItem {
                    Text = "8:30 PM", Value = "41"},
                new SelectListItem {
                    Text = "9:00 PM", Value = "42"},
                new SelectListItem {
                    Text = "9:30 PM", Value = "43"},
                new SelectListItem {
                    Text = "10:00 PM", Value ="44"},
                new SelectListItem {
                    Text = "10:30 PM", Value ="45"},
                new SelectListItem {
                    Text = "11:00 PM", Value ="46"},
                new SelectListItem {
                    Text = "11:30 PM", Value ="47"},

            };

        internal static int ToInt(DateTime startTime)
        {
            return (int)(startTime.Hour*2+(startTime.Minute / 30.0));
        }

        public static DateTime ToTime(int value)
        {
            return DateTime.Now.Date.AddHours(((double)value) / 2);
        }
    }
}