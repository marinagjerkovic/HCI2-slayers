using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
    
    public class Subject
    {
        private string id;
        private string name;
        private string description;
        private Course course;
        private int size_of_group;
        private int duration_of_period;
        private int num_of_periods;
        private bool board;
        private bool smart_board;
        private bool projector;
        private Classroom.OpSystem os;
        private List<Software> software;

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

        public int Size_of_group
        {
            get
            {
                return size_of_group;
            }

            set
            {
                size_of_group = value;
            }
        }

        public int Duration_of_period
        {
            get
            {
                return duration_of_period;
            }

            set
            {
                duration_of_period = value;
            }
        }

        public int Num_of_periods
        {
            get
            {
                return num_of_periods;
            }

            set
            {
                num_of_periods = value;
            }
        }

        public bool Board
        {
            get
            {
                return board;
            }

            set
            {
                board = value;
            }
        }

        public bool Smart_board
        {
            get
            {
                return smart_board;
            }

            set
            {
                smart_board = value;
            }
        }

        public bool Projector
        {
            get
            {
                return projector;
            }

            set
            {
                projector = value;
            }
        }

        public Classroom.OpSystem Os
        {
            get
            {
                return os;
            }

            set
            {
                os = value;
            }
        }

        public Course Course
        {
            get
            {
                return course;
            }

            set
            {
                course = value;
            }
        }

        public List<Software> Software
        {
            get
            {
                return software;
            }

            set
            {
                software = value;
            }
        }

        public string Course_name
        {
            get
            {
                return course.Name;
            }

            set
            {
                course.Name = value;
            }
        }


        public Subject()
        {
            this.Software = new List<HCI2___Back_To_Slay.Software>();
        }

        public Subject(string v1, string v2, string v3, Course course, int v4, int v5, int v6, bool v7, bool v8, bool v9, Classroom.OpSystem linux, List<Software> s)
        {
            id = v1;
            name = v2;
            description = v3;
            size_of_group = v4;
            duration_of_period = v5;
            num_of_periods= v6;
            board = v7;
            smart_board = v8;
            projector = v9;
            this.course = course;
            os = linux;
            software = s;
        }
    }
}
