﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Models
{
    public class World
    {
        private readonly List<Location> _locations = new List<Location>();

        internal void AddLocation (Location location)
        {
            _locations.Add(location);
        }

        public Location LocationAt (int xCoordinate, int yCoordinate)
        {
            foreach (Location loc in _locations)
            {
                if(loc.X == xCoordinate && loc.Y==yCoordinate)
                {
                    return loc;
                }
            }
            return null;
        }

        //internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        //{
        //    _locations.Add(new Location(xCoordinate, yCoordinate, name, description, "/GameEngine;component/Images/Locations/" + imageName));

        //}
        //public Location LocationAt(int xCoordinate, int yCoordinate)
        //{
        //    foreach(Location loc in _locations)
        //    {
        //       if( xCoordinate==loc.XCoordinate&& yCoordinate==loc.YCoordinate)
        //        { return loc; }
        //    }
        //    return null;
        //}




        //private List<Location> _location = new List<Location>();
        //internal void AddLocation(int xCoordinate, int yCoordinate, string name, string description, string imageName)
        //{
        //    Location loc = new Location();
        //    loc.XCoordinate = xCoordinate;
        //    loc.YCoordinate = yCoordinate;
        //    loc.Name = name;
        //    loc.Description = description;
        //    loc.ImageName = "/GameEngine;component/Images/Locations/"+imageName;
        //    _location.Add(loc);
        //}
        //public Location LocationAt (int xCoordinate, int yCoordinate)
        //{
        //    foreach(Location loc in _location)
        //    {
        //        if(loc.XCoordinate==xCoordinate && loc.YCoordinate==yCoordinate)
        //        {
        //            return loc;
        //        }
        //    }
        //    return null;
        //}

    }
}
