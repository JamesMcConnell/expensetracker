using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ExpenseTracker.Framework.ViewModels
{
    [DataContract]
    public class BudgetItemAsEvent
    {
        [DataMember(Name = "title")]
        public string EventTitle { get; set; }

        [DataMember(Name = "allDay")]
        public bool AllDayEvent { get; set; }

        [DataMember(Name = "start")]
        public DateTime EventStart { get; set; }

        [DataMember(Name = "editable")]
        public bool IsEditable { get; set; }

        [DataMember(Name = "textColor")]
        public string TextColor { get; set; }

        [DataMember(Name = "backgroundColor")]
        public string BackgroundColor { get; set; }

        [DataMember(Name = "borderColor")]
        public string BorderColor { get; set; }
    }
}
