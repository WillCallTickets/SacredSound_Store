using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utils
{
    public class EmailAddress
    {
        public EmailAddress(string emailAddress)
        {
            _address = emailAddress;
        }

        private string _address = null;
        public string Address
        {
            get 
            {
                return _address;
            }
            set 
            {
                //allow nulls to pass thru
                if (value != null && (!Utils.Validation.IsValidEmail(value)))
                    throw new Exception("Please enter a valid email address.");

                _address = value;
            }
        }


    }
}
