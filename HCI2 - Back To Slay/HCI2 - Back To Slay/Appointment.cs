using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
    public class Appointment
    {
        private Classroom classroom;
        private Subject subject;
        private DateTime start;
        private DateTime end;

        public DateTime Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public DateTime End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public Classroom Classroom
        {
            get
            {
                return classroom;
            }

            set
            {
                classroom = value;
            }
        }
        public Subject Subject
        {
            get
            {
                return subject;
            }

            set
            {
                subject = value;
            }
        }

        public Appointment() { }
    }
}
