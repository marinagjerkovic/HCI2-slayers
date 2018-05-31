using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
    class Subject
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

        internal Classroom.OpSystem Os
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

        internal Course Course
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

        internal List<Software> Software
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

        public Subject() { }
    }
}
