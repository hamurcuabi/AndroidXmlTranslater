using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XmlTranslaterWeb.Models
{
    public class CountryMockData
    {
        public List<Country> GetCountryList()
        {
            List<Country> countries = new List<Country>
        {
            new Country { name = "tr", code = "Turkish" },
             new Country { name = "en", code = "English" },
             new Country { name = "ar", code = "Arabic" },
             new Country { name = "de", code = "German" },
            new Country { name = "az", code = "Azerbaijan" },
            new Country { name = "fr", code = "French" },
            new Country { name = "it", code = "Italian" },
            new Country { name = "hi", code = "Hindi" },
            new Country { name = "es", code = "Spanish" },
            new Country { name = "ru", code = "Russian" },
            new Country { name = "pl", code = "Polish" },
            new Country { name = "sv", code = "Swedish" },
            new Country { name = "sq", code = "Albanian" },
            new Country { name = "am", code = "Amharic" },
            new Country { name = "zh", code = "Chinese" },
            new Country { name = "ja", code = "Japanese" },
            new Country { name = "pt", code = "Portuguese" },

        };
            return countries;
        }
    }
}