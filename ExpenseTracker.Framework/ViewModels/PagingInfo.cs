using System.Runtime.Serialization;

namespace ExpenseTracker.Framework.ViewModels
{
    [DataContract]
    public class PagingInfo
    {
        [DataMember(Name = "hasPrevious")]
        public bool HasPrevious { get; set; }

        [DataMember(Name = "hasFuture")]
        public bool HasFuture { get; set; }
    }
}
