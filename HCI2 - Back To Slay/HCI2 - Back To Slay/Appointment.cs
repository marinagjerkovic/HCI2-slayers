using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.UI.Xaml.Schedule;

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

        public Appointment(Classroom classroom, Subject subject, DateTime start, DateTime end)
        {
            this.classroom = classroom;
            this.subject = subject;
            this.start = start;
            this.end = end;
        }

        public Appointment(ScheduleAppointment app)
        {

            foreach(Classroom cr in MainWindow.allClassrooms)
            {
                
                Console.WriteLine("app location:" + app.Location+"\n app name"+app.Subject);
                if (cr.Id == app.Location)
                {
                    
                    this.classroom = cr;
                    break;
                }
            }
            foreach (Subject sub in MainWindow.allSubjects)
            {
                if (app.Subject.Contains(sub.Name))
                {
                    this.subject = sub;
                    app.Subject += "\n" + subject.Course.Name;
                    break;
                }
            }

        }

        public void printApp()
        {
            Console.WriteLine("(" + classroom.Id + "," + subject.Name + "," + start.ToShortTimeString() + ","+end.ToShortTimeString()+")");
        }
    }
}
