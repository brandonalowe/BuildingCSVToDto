using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CryptoVillages.Common;
using CsvHelper.Configuration;

namespace BuildingCSVtoDto
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Set up csv read
            using var streamReader = new StreamReader("/home/jaan/RiderProjects/BuildingCSVToDto/BuildingCSVToDto/test.csv"); // insert csv dir here yeah?!
            using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            
            // Context mapping to match csv headers with BuildingDto 
            csvReader.Context.RegisterClassMap<BuildingDtoClassMap>();
            var buildingList = csvReader.GetRecords<CommonCvTypes.Building>().ToList();
            
            foreach (var building in buildingList)
            {
                building.Id = Guid.NewGuid();
                building.LastFed = DateTime.Now;
            }
        }
        
        // Insert header names here for value mapping ease -- idk what mitch's csv looks like
        public class BuildingDtoClassMap : ClassMap<CommonCvTypes.Building>
        {
            public BuildingDtoClassMap()
            {
                Map(m => m.Owner).Name("owner");
                Map(m => m.BuildingType).Name("building_type");
            }
        }

    }
}