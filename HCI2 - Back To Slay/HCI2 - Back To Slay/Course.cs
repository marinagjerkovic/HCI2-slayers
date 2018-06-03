using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
    
    public class Course
    {
        private string id;
        private string name;
        private string description;
        private DateTime date_of_conception;


        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }

            set
            {
                description = value;
            }
        }

        public DateTime Date_of_conception
        {
            get
            {
                return date_of_conception;
            }

            set
            {
                date_of_conception = value;
            }
        }

        public Course() { }

        public Course(string v1, string v2, string v3, DateTime now)
        {
            this.id = v1;
            this.name = v2;
            this.description = v3;
            this.date_of_conception = now;
        }
    }
}
