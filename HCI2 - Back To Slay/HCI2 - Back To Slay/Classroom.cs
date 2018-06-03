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
            WindowsAndLinux
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

        public OpSystem Os
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

        public Classroom()
        {
            Software = new List<HCI2___Back_To_Slay.Software>();
        }

        public Classroom(string v1, string v2, int v3, bool v4, bool v5, bool v6, OpSystem both, List<Software> ss)
        {
            id = v1;
            description = v2;
            num_of_seats = v3;
            projector = v4;
            board = v5;
            smart_board = v6;
            os = both;
            software = ss;
        }

        public string softwares()
        {
            string s_display = "none";
            if (this.Software.Count > 0)
            {
                s_display = this.Software.First().Name;
            }
            if (this.Software.Count > 1)
            {
                foreach (Software s in this.Software)
                {
                    if (this.Software.First().Id != s.Id)
                    {
                        s_display += "; " + s.Name;
                    }
                }
            }
            return s_display;
        }
    }
}
