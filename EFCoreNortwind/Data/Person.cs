using System;
using System.Collections.Generic;

#nullable disable

namespace EFCoreNortwind.Data
{
    public partial class Person
    {
        public Person()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid RowVersionId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
