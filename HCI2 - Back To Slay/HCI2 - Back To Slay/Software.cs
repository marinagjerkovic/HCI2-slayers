using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI2___Back_To_Slay
{
 
    public class Software
    {
        private string id;
        private string name;
        private Classroom.OpSystem os;
        private string maker;
        private string site;
        private int year;
        private double price;
        private string description;

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

        public string Maker
        {
            get
            {
                return maker;
            }

            set
            {
                maker = value;
            }
        }

        public string Site
        {
            get
            {
                return site;
            }

            set
            {
                site = value;
            }
        }

        public int Year
        {
            get
            {
                return year;
            }

            set
            {
                year = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
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

        public Software() { }

        public Software(string v1, string v2, Classroom.OpSystem linux, string v3, string v4, int v5, double v6, string v7)
        {
            id = v1;
            name = v2;
            os = linux;
            maker = v3;
            site = v4;
            year = v5;
            price = v6;
            description = v7;
        }
    }
}
