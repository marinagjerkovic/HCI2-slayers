using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
    public class Classroom
    {
        public enum OpSystem
        {
            Windows,
            Linux,
            Both
        };
        private string id;
        private string description;
        private int num_of_seats;
        private bool projector;
        private bool board;
        private bool smart_board;
        private OpSystem os;
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

        public int Num_of_seats
        {
            get
            {
                return num_of_seats;
            }

            set
            {
                num_of_seats = value;
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

        internal OpSystem Os
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

        public Classroom() { }




    }
}
